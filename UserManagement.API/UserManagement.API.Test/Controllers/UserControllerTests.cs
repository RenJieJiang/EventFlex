using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Controllers;
using UserManagement.API.Services;
using UserManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.API.DTOs;
using Microsoft.Extensions.Configuration;

namespace UserManagement.API.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ITenantService> _mockTenantService;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly UsersController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTenantService = new Mock<ITenantService>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfiguration = new Mock<IConfiguration>();

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

            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(),
                new IRoleValidator<IdentityRole>[0],
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<ILogger<RoleManager<IdentityRole>>>()
            );

            _controller = new UsersController(
                _mockUserService.Object,
                _mockTenantService.Object,
                _mockHttpClientFactory.Object,
                _mockConfiguration.Object,
                _mockUserManager.Object,
                _mockRoleManager.Object
            );
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { Id = Guid.NewGuid() } };
            _mockUserService.Setup(service => service.GetAllUsersAsync())
                            .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<User>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                            .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, returnValue.Id);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                            .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddUser_ReturnsCreatedAtAction_WithUser()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Email = "test@example.com" };
            var user = new User { Id = Guid.NewGuid(), UserName = userDto.UserName, Email = userDto.Email };

            _mockUserService.Setup(service => service.AddUserAsync(It.IsAny<User>()))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal(userDto.UserName, returnValue.UserName);
            Assert.Equal(userDto.Email, returnValue.Email);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { UserName = "updateduser", Email = "updated@example.com" };
            var user = new User { Id = userId, UserName = userDto.UserName, Email = userDto.Email };

            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                            .ReturnsAsync(user);
            _mockUserService.Setup(service => service.UpdateUserAsync(It.IsAny<User>()))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { Id = Guid.NewGuid().ToString(), UserName = "updateduser", Email = "updated@example.com" };

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUserService.Setup(service => service.DeleteUserAsync(userId))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userId.ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}