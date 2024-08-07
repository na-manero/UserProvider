using Microsoft.AspNetCore.Identity;

namespace UserProvider.Data.Entities;

public class UserEntity : IdentityUser
{
    public UserProfileEntity? UserProfile { get; set; }
}
