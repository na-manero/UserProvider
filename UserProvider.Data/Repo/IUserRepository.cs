using UserProvider.Data.Entities;

namespace UserProvider.Data.Repo
{
    public interface IUserRepository
    {
        Task CreateUserAsync(UserEntity user);
        Task DeleteUserAsync(string userId);
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<UserEntity> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(UserEntity user);
    }
}