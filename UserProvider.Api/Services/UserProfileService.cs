using System.Diagnostics;
using UserProvider.Data.Entities;
using UserProvider.Data.Repo;

namespace UserProvider.Api.Services;

public class UserProfileService(IUserProfileRepository userProfileRepository, IUserRepository userRepository) : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task CreateAsync(UserProfileEntity userProfile)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(userProfile.Email);

            if (user != null)
            {
                userProfile.UserId = user.Id;
                await _userProfileRepository.CreateUserAsync(userProfile);
            }
            else
                Debug.WriteLine("No user found.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
