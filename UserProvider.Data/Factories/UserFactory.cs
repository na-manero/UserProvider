using System.Diagnostics;
using UserProvider.Data.Entities;

namespace UserProvider.Data.Factories;

public class UserFactory
{
    public static UserProfileEntity Create(UserProfileEntity user)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(user?.Email))
                return null!;

            return new UserProfileEntity()
            {
                Email = user!.Email,
                FirstName = user!.FirstName,
                LastName = user!.LastName
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine("UserFactory : " + ex.Message);
        }

        return null!;
    }
}
