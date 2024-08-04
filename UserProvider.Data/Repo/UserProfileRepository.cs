using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UserProvider.Data.Context;
using UserProvider.Data.Entities;

namespace UserProvider.Data.Repo;

public class UserProfileRepository(DataContext context) : IUserProfileRepository
{
    private readonly DataContext _context = context;

    // Create User
    public async Task CreateUserAsync(UserProfileEntity user)
    {
        try
        {
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    // Read User by ID
    public async Task<UserProfileEntity> GetUserByIdAsync(string userId)
    {
        try
        {
            return await _context.UserProfiles.FindAsync(userId) ?? null!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return null!;
    }

    // Read All Users
    public async Task<UserProfileEntity> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email) ?? null!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return null!;
    }

    // Update User
    public async Task UpdateUserAsync(UserProfileEntity user)
    {
        try
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    // Delete User
    public async Task DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _context.UserProfiles.FindAsync(userId);
            if (user != null)
            {
                _context.UserProfiles.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

}
