using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.API.Data;
using UserManagement.API.Models;
using UserManagement.API.Repositories;
using Xunit;

namespace UserManagement.API.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { Id = Guid.NewGuid() } };
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task AddUserAsync_AddsUser()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };

            // Act
            await _userRepository.AddUserAsync(user);

            // Assert
            var addedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal(user, addedUser);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.UserName = "UpdatedUser";

            // Act
            await _userRepository.UpdateUserAsync(user);

            // Assert
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal("UpdatedUser", updatedUser.UserName);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            await _userRepository.DeleteUserAsync(userId);

            // Assert
            var deletedUser = await _context.Users.FindAsync(userId);
            Assert.Null(deletedUser);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}