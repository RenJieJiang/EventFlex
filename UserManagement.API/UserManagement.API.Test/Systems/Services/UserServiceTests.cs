using Moq;
using Moq.Protected;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.API.Constants;
using UserManagement.API.Models;
using UserManagement.API.Models.DTOs;
using UserManagement.API.Services;
using UserManagement.API.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Net;
using System.Text;
using System.Net.Http.Json;

namespace UserManagement.API.Test.Systems.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>()
            );

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<UserService>>();

            // Set up HTTP client and configuration using helper
            HttpClientHelper.SetupHttpClientFactory(_mockHttpClientFactory, "MessagingService");
            HttpClientHelper.SetupConfiguration(_mockConfiguration);

            _service = new UserService(
                _mockUserManager.Object,
                _mockHttpClientFactory.Object,
                _mockConfiguration.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Name = "User1" },
                new ApplicationUser { Id = "2", Name = "User2" }
            };

            var asyncEnumerable = new AsyncEnumerableHelper.TestAsyncEnumerable<ApplicationUser>(users);
            _mockUserManager.Setup(x => x.Users)
                .Returns(asyncEnumerable);

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Id == "1");
            Assert.Contains(result, u => u.Id == "2");
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId.ToString(), Name = "Test User" };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId.ToString(), result.Id);
        }

        [Fact]
        public async Task CreateUserAsync_WithValidData_CreatesUserAndAssignsRole()
        {
            // Arrange
            var userDto = new ApplicationUserModel
            {
                Name = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userDto.Email,
                Name = userDto.Name,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email))
                .ReturnsAsync(user);

            _mockUserManager.Setup(x => x.AddToRoleAsync(user, Roles.User))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _service.CreateUserAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Email, result.Email);
            Assert.Equal(userDto.Name, result.Name);
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.AddToRoleAsync(user, Roles.User), Times.Once);
            _mockHttpClientFactory.Verify(x => x.CreateClient("MessagingService"), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_WithInvalidData_ThrowsException()
        {
            // Arrange
            var userDto = new ApplicationUserModel
            {
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid data" }));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.CreateUserAsync(userDto));
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidData_UpdatesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new ApplicationUserModel
            {
                Name = "Updated User",
                Email = "updated@example.com"
            };

            var existingUser = new ApplicationUser
            {
                Id = userId.ToString(),
                Name = "Original User",
                Email = "original@example.com"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(existingUser);

            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.UpdateUserAsync(userId, userDto);

            // Assert
            _mockUserManager.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => 
                u.Name == userDto.Name && 
                u.Email == userDto.Email)), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new ApplicationUserModel
            {
                Name = "Updated User",
                Email = "updated@example.com"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _service.UpdateUserAsync(userId, userDto));
        }

        [Fact]
        public async Task DeleteUserAsync_WithValidId_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ApplicationUser { Id = userId };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.DeleteUserAsync(userId);

            // Assert
            _mockUserManager.Verify(x => x.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var userId = "invalid-id";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.DeleteUserAsync(userId));
        }
    }
} 