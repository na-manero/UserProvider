using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using UserProvider.Data.Entities;
using UserProvider.Data.Factories;
using UserProvider.Data.Models;
using UserProvider.Data.Repo;

namespace UserProvider.Api.Services;

public class UserAccountService(IUserProfileRepository userProfileRepository, IUserRepository userRepository, UserManager<UserEntity> userManager) : IUserAccountService
{
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<bool> CreateProfileAsync(UserAccountModel model)
    {
        if (model == null)
            return false;

        try
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user != null)
            {
                model.UserId ??= user.Id;
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
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result == null)
                    return false;

                var userProfile = await _userProfileRepository.GetUserByIdAsync(model.UserId);

                if (userProfile != null)
                {
                    userProfile.FirstName = model.FirstName;
                    userProfile.LastName = model.LastName;
                    userProfile.Location = model.Location;
                    userProfile.ImageUrl = model.ImageUrl;

                    await _userProfileRepository.UpdateUserAsync(userProfile);
                }
                else
                {
                    await CreateProfileAsync(model);
                }

                return true;
            }
            else
            {
                return false;
            }
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

            if (profile == null)
                return null!;

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

            if (profile == null)
                return null!;

            return UserFactory.CreateUserAccount(user, profile)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}
