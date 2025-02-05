using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using UserManagement.API.Constants;
using UserManagement.API.Models;
using UserManagement.API.Models.DTOs;
using UserManagement.API.Messages;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        //private readonly IUserService _userService;
        //private readonly ITenantService _tenantService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            //IUserService userService,
            //ITenantService tenantService,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //_userService = userService;
            //_tenantService = tenantService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(ApplicationUserModel userDto)
        {
            // Convert DTO to entity
            var user = new ApplicationUser
            {
                UserName = userDto.Email,
                Name = userDto.UserName ?? "",
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                //TenantId = string.IsNullOrEmpty(userDto.TenantId) ? (Guid?)null : Guid.Parse(userDto.TenantId),
                EmailConfirmed = true
            };

            // Set initial password if none is provided
            var password = string.IsNullOrWhiteSpace(userDto.Password) ? "Password@123" : userDto.Password;

            // Create user
            var createUserResult = await _userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                return BadRequest($"User creation failed: {string.Join(", ", errors)}");
            }

            // Ensure the user exists before assigning a role
            var foundUser = await _userManager.FindByEmailAsync(user.Email);
            if (foundUser == null)
            {
                return StatusCode(500, "User creation succeeded but could not retrieve the user.");
            }

            // Assign role
            var addUserToRoleResult = await _userManager.AddToRoleAsync(foundUser, Roles.User);
            if (!addUserToRoleResult.Succeeded)
            {
                var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                return BadRequest($"Failed to assign role: {string.Join(", ", errors)}");
            }

            // Notify messaging service
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
                return StatusCode((int)response.StatusCode, "Failed to notify the messaging service.");
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, foundUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, ApplicationUserModel userDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            // Retrieve the existing user
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update the properties of the existing user
            existingUser.UserName = userDto.UserName;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            //existingUser.TenantId = string.IsNullOrEmpty(userDto.TenantId) ? (Guid?)null : Guid.Parse(userDto.TenantId);


            // Validate TenantId
            //if (existingUser.TenantId != null)
            //{
            //    var tenant = await _tenantService.GetTenantByIdAsync(existingUser.TenantId.Value);
            //    if (tenant == null)
            //    {
            //        return BadRequest("Tenant does not exist.");
            //    }
            //}

            await _userManager.UpdateAsync(existingUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id) || id == Guid.Empty.ToString())
            {
                return BadRequest("User ID is required.");
            }

            // Retrieve the existing user
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(existingUser);
            return NoContent();
        }

        private async Task<HttpResponseMessage> SendMessageAsync(string endpoint, object message)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var messagingServiceDomain = _configuration["MessagingService:Domain"] ?? "http://host.docker.internal";
            var messagingServicePort = _configuration["MessagingService:Port"] ?? "3000";
            var url = $"http://{messagingServiceDomain}:{messagingServicePort}/{endpoint}";
            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(url, content);
        }
    }
}