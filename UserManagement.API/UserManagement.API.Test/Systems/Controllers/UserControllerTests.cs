using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Controllers;
using UserManagement.API.Models;
using UserManagement.API.Models.DTOs;
using UserManagement.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace UserManagement.API.Test.Systems.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Name = "User1" },
                new ApplicationUser { Id = "2", Name = "User2" }
            };

            _mockUserService.Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ApplicationUser>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId.ToString(), Name = "Test User" };

            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<ApplicationUser>(okResult.Value);
            Assert.Equal(userId.ToString(), model.Id);
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddUser_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var userDto = new ApplicationUserModel
            {
                Name = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };

            var createdUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = userDto.Name,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber
            };

            _mockUserService.Setup(service => service.CreateUserAsync(userDto))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsType<ApplicationUser>(createdResult.Value);
            Assert.Equal(createdUser.Id, model.Id);
        }

        [Fact]
        public async Task AddUser_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var userDto = new ApplicationUserModel
            {
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserService.Setup(service => service.CreateUserAsync(userDto))
                .ThrowsAsync(new InvalidOperationException("Invalid user data"));

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user data", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new ApplicationUserModel
            {
                Name = "Updated User",
                Email = "updated@example.com"
            };

            _mockUserService.Setup(service => service.UpdateUserAsync(userId, userDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new ApplicationUserModel
            {
                Name = "Updated User",
                Email = "updated@example.com"
            };

            _mockUserService.Setup(service => service.UpdateUserAsync(userId, userDto))
                .ThrowsAsync(new KeyNotFoundException($"User with ID {userId} not found."));

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _mockUserService.Setup(service => service.DeleteUserAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _mockUserService.Setup(service => service.DeleteUserAsync(userId))
                .ThrowsAsync(new KeyNotFoundException($"User with ID {userId} not found."));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}