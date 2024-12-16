using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Controllers;
using UserManagement.API.Services;
using UserManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.API.Tests.Controllers
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
            var user = new User { Id = Guid.NewGuid() };

            // Act
            var result = await _controller.AddUser(user);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal(user.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            // Act
            var result = await _controller.UpdateUser(userId, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid() };

            // Act
            var result = await _controller.UpdateUser(userId, user);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}