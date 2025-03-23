using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using UserManagement.API.Constants;
using UserManagement.API.Messages;
using UserManagement.API.Models;
using UserManagement.API.Models.DTOs;

namespace UserManagement.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly HttpClient _messagingClient;
        private readonly string _messagingServiceUrl;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;

            // Initialize messaging client with a specific name for better tracking
            _messagingClient = _httpClientFactory.CreateClient("MessagingService");
            
            // Build the base URL for messaging service
            var messagingServiceDomain = _configuration["MessagingService:Domain"] ?? "messaging-service";
            var messagingServicePort = _configuration["MessagingService:Port"] ?? "3002";
            _messagingServiceUrl = $"http://{messagingServiceDomain}:{messagingServicePort}";
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUserModel userDto)
        {
            var user = new ApplicationUser
            {
                UserName = userDto.Email,
                Name = userDto.Name ?? "",
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                EmailConfirmed = true
            };

            var password = string.IsNullOrWhiteSpace(userDto.Password) ? "Password@123" : userDto.Password;
            var createUserResult = await _userManager.CreateAsync(user, password);

            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                throw new InvalidOperationException($"User creation failed: {string.Join(", ", errors)}");
            }

            var foundUser = await _userManager.FindByEmailAsync(user.Email);
            if (foundUser == null)
            {
                throw new InvalidOperationException("User creation succeeded but could not retrieve the user.");
            }

            var addUserToRoleResult = await _userManager.AddToRoleAsync(foundUser, Roles.User);
            if (!addUserToRoleResult.Succeeded)
            {
                var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                throw new InvalidOperationException($"Failed to assign role: {string.Join(", ", errors)}");
            }

            var message = new UserCreatedMessage
            {
                Id = foundUser.Id.ToString(),
                UserName = foundUser.UserName,
                Email = foundUser.Email,
                PhoneNumber = foundUser.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var response = await SendMessageAsync("message/user-created", message);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to notify the messaging service about user creation.");
            }

            return foundUser;
        }

        public async Task UpdateUserAsync(Guid id, ApplicationUserModel userDto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("User ID is required.", nameof(id));
            }

            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            existingUser.UserName = userDto.Email;
            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new InvalidOperationException($"Failed to update user: {string.Join(", ", errors)}");
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            if (!Guid.TryParse(id, out var userId) || userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user ID.", nameof(id));
            }

            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new InvalidOperationException($"Failed to delete user: {string.Join(", ", errors)}");
            }
        }

        private async Task<HttpResponseMessage> SendMessageAsync(string endpoint, object message)
        {
            var url = $"{_messagingServiceUrl}/{endpoint}";
            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            return await _messagingClient.PostAsync(url, content);
        }
    }
} 