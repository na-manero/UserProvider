using System.Diagnostics;
using UserProvider.Data.Factories;
using UserProvider.Data.Entities;
using UserProvider.Api.Services;
using UserProvider.Data.Models;

namespace UserProvider.Api.Handlers;

public class NewUserMessageReceiver(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    private async Task ProcessMessageAsync(UserAccountModel model)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var userProfileService = scope.ServiceProvider.GetRequiredService<IUserAccountService>();

        try
        {
            await userProfileService.CreateProfileAsync(model);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to create user: {ex.Message}");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var serviceBusHandler = scope.ServiceProvider.GetRequiredService<IServiceBusHandler>();

        await serviceBusHandler.ReceiveMessagesAsync<UserAccountModel>("newuser-queue", ProcessMessageAsync);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
