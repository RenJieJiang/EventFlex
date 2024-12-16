using Moq;
using Xunit;
using UserManagement.API.Models;
using UserManagement.API.Repositories;
using UserManagement.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.API.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
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
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                               .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
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
    }
}