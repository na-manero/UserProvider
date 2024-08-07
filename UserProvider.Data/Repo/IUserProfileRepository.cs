using UserProvider.Data.Entities;

namespace UserProvider.Data.Repo
{
    public interface IUserProfileRepository
    {
        Task CreateUserAsync(UserProfileEntity user);
        Task DeleteUserAsync(string userId);
        Task<UserProfileEntity> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(UserProfileEntity user);
    }
}