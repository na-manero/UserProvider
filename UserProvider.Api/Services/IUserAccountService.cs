using UserProvider.Data.Models;

namespace UserProvider.Api.Services;

public interface IUserAccountService
{
    Task<bool> CreateProfileAsync(UserAccountModel user);

    Task<bool> UpdateUserAsync(UserAccountModel user);

    Task<UserAccountModel> GetUserByIdAsync(string userId);

    Task<UserAccountModel> GetUserByEmailAsync(string email);
}