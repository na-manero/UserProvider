using UserProvider.Data.Entities;

namespace UserProvider.Data.Repo
{
    public interface IUserRepository
    {
        Task CreateUser(UserEntity user);
        Task DeleteUser(string userId);
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<UserEntity> GetUserByIdAsync(string userId);
        Task UpdateUser(UserEntity user);
    }
}