using System.Diagnostics;
using UserProvider.Data.Entities;
using UserProvider.Data.Factories;
using UserProvider.Data.Models;
using UserProvider.Data.Repo;

namespace UserProvider.Api.Services;

public class UserAccountService(IUserProfileRepository userProfileRepository, IUserRepository userRepository) : IUserAccountService
{
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> CreateProfileAsync(UserAccountModel model)
    {
        if (model == null)
            return false;

        try
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user != null)
            {
                var newUserProfile = UserFactory.CreateProfile(model);

                if (newUserProfile == null)
                    return false;

                await _userProfileRepository.CreateUserAsync(newUserProfile);
                return true;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(UserAccountModel model)
    {
        if (model == null)
            return false;

        try
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user != null)
            {
                var updatedUser = UserFactory.Create(model);

                if (updatedUser == null)
                    return false;

                await _userRepository.UpdateUserAsync(updatedUser);

                var userProfile = await _userProfileRepository.GetUserByIdAsync(user.Id);

                if (userProfile != null)
                {
                    var updatedUserProfile = UserFactory.CreateProfile(model);

                    if (updatedUserProfile == null)
                        return false;

                    await _userProfileRepository.UpdateUserAsync(updatedUserProfile);
                }
                else
                    await CreateProfileAsync(model);
            }
            else
                return false;

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<UserAccountModel> GetUserByIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null!;

        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return null!;

            var profile = await _userProfileRepository.GetUserByIdAsync(userId);

            return UserFactory.CreateUserAccount(user, profile)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<UserAccountModel> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null!;

        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return null!;

            var profile = await _userProfileRepository.GetUserByIdAsync(user.Id);

            return UserFactory.CreateUserAccount(user, profile)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}
