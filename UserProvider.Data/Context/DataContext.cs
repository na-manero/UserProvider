using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserProvider.Data.Entities;

namespace UserProvider.Data.Context;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<UserProfileEntity> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the primary key of UserProfileEntity to be UserId
        modelBuilder.Entity<UserProfileEntity>()
            .HasKey(up => up.UserId);

        // Configure one-to-one relationship between UserEntity and UserProfileEntity
        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfileEntity>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Configure cascading delete

        // Ensure UserProfileEntity's UserId is unique
        modelBuilder.Entity<UserProfileEntity>()
            .HasIndex(up => up.UserId)
            .IsUnique();
    }
}
