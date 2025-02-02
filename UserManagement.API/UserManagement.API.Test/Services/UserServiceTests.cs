using Moq;
using Xunit;
using UserManagement.API.Services;
using UserManagement.API.Repositories;
using UserManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UserManagement.API.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<User>>(),
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>()
            );

            _userService = new UserService(_mockUserRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { Id = Guid.NewGuid() } };
            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync())
                               .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task AddUserAsync_CallsRepository()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_CallsRepository()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_CallsRepository()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsUser_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = new User { Id = Guid.NewGuid(), Email = email };

            _mockUserManager.Setup(manager => manager.FindByEmailAsync(email))
                            .ReturnsAsync(user);
            _mockUserManager.Setup(manager => manager.CheckPasswordAsync(user, password))
                            .ReturnsAsync(true);

            // Act
            var result = await _userService.AuthenticateAsync(email, password);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = new User { Id = Guid.NewGuid(), Email = email };

            _mockUserManager.Setup(manager => manager.FindByEmailAsync(email))
                            .ReturnsAsync(user);
            _mockUserManager.Setup(manager => manager.CheckPasswordAsync(user, password))
                            .ReturnsAsync(false);

            // Act
            var result = await _userService.AuthenticateAsync(email, password);

            // Assert
            Assert.Null(result);
        }
    }
}