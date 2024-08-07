using System.Diagnostics;
using System.Text.Json;
using UserProvider.Data.Entities;
using UserProvider.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserProvider.Data.Factories;

public class UserFactory
{
    /// <summary>
    /// Create UserProfile Entity
    /// </summary>
    public static UserEntity? Create(UserAccountModel user)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(user?.FirstName) || user is null)
                return null;

            return new UserEntity()
            {
                Email = user!.Email,
                PhoneNumber = user?.PhoneNumber
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine("UserFactory : " + ex.Message);
        }

        return null;
    }       
    
    /// <summary>
    /// Create UserProfile Entity
    /// </summary>
    public static UserProfileEntity? CreateProfile(UserAccountModel user)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(user?.FirstName) || user is null)
                return null;

            return new UserProfileEntity()
            {
                UserId = user?.UserId ?? "",
                FirstName = user!.FirstName,
                LastName = user!.LastName
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine("UserFactory : " + ex.Message);
        }

        return null;
    }

    public static UserAccountModel? CreateUserAccount(UserEntity user, UserProfileEntity profile)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(user?.Email) || user is null || profile is null)
                return null;

            return new UserAccountModel()
            {
                UserId = user.Id,
                FirstName = profile.FirstName, 
                LastName = profile.LastName, 
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Location = profile.Location,
                ImageUrl = profile.ImageUrl
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine("UserFactory : " + ex.Message);
            return null;
        }
    }
}
