using UserProvider.Data.Entities;

namespace UserProvider.Api.Services
{
    public interface IUserProfileService
    {
        Task CreateAsync(UserProfileEntity user);
    }
}