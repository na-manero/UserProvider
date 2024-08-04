using System.Diagnostics;
using UserProvider.Data.Factories;
using UserProvider.Api.Handlers;
using UserProvider.Data.Entities;

namespace UserProvider.Api.Services;

public class NewUserMessageReceiver(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    private async Task ProcessMessageAsync(UserProfileEntity parameters)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var userProfileService = scope.ServiceProvider.GetRequiredService<IUserProfileService>();

        try
        {
            // Create the UserProfileEntity
            var userProfile = UserFactory.Create(parameters);

            if (userProfile != null)
            {
                // Create user profile
                await userProfileService.CreateAsync(userProfile);
            }
            else
            {
                // Handle the case where userProfile is null
                Debug.WriteLine("User could not be created.");
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during processing
            Debug.WriteLine($"Error processing parameters: {ex.Message}");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var serviceBusHandler = scope.ServiceProvider.GetRequiredService<IServiceBusHandler>();

        await serviceBusHandler.ReceiveMessagesAsync<UserProfileEntity>("newuser-queue", ProcessMessageAsync);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
