using Microsoft.AspNetCore.Identity;
using Moq;
using UserProvider.Api.Services;
using UserProvider.Data.Entities;
using UserProvider.Data.Factories;
using UserProvider.Data.Models;
using UserProvider.Data.Repo;

namespace UserProvider.Tests.UnitTests;

public class UserAccountServiceTests
{
    private readonly Mock<IUserProfileRepository> _mockUserProfileRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IUserAccountService _userAccountService;

    public UserAccountServiceTests()
    {
        _mockUserProfileRepository = new Mock<IUserProfileRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _userAccountService = new UserAccountService(_mockUserProfileRepository.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task CreateProfileAsync_ShouldReturnFalse_WhenModelIsNull()
    {
        // Act
        var result = await _userAccountService.CreateProfileAsync(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateProfileAsync_ShouldReturnFalse_WhenUserAlreadyExists()
    {
        // Arrange
        var userAccountModel = new UserAccountModel { Email = "test@example.com" };
        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(userAccountModel.Email)).ReturnsAsync(new UserEntity());

        // Act
        var result = await _userAccountService.CreateProfileAsync(userAccountModel);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateProfileAsync_ShouldReturnTrue_WhenProfileCreatedSuccessfully()
    {
        // Arrange
        var userAccountModel = new UserAccountModel
        {
            UserId = Guid.NewGuid().ToString(),
            Email = "test@example.com",
            FirstName = "Peter",
            LastName = "Stormare"
        };

        var existingUserEntity = new UserEntity
        {
            Id = userAccountModel.UserId,
            Email = userAccountModel.Email,
            PhoneNumber = "123-456-7890"
        };

        // Set up the repository to return an existing user with the email
        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(userAccountModel.Email)).ReturnsAsync(existingUserEntity);

        // Set up the repository to simulate the user profile creation
        _mockUserProfileRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<UserProfileEntity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _userAccountService.CreateProfileAsync(userAccountModel);

        // Assert
        Assert.True(result);

        // Verify that the methods were called as expected
        _mockUserRepository.Verify(repo => repo.GetUserByEmailAsync(userAccountModel.Email), Times.Once);
        _mockUserProfileRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<UserProfileEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenModelIsNull()
    {
        // Act
        var result = await _userAccountService.UpdateUserAsync(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserIdIsNullOrWhitespace()
    {
        // Act
        var result = await _userAccountService.GetUserByIdAsync(null!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenEmailIsNullOrWhitespace()
    {
        // Act
        var result = await _userAccountService.GetUserByEmailAsync(null!);

        // Assert
        Assert.Null(result);
    }
}
