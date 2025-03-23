using UserManagement.API.Models;
using UserManagement.API.Models.DTOs;

namespace UserManagement.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(Guid id);
        Task<ApplicationUser> CreateUserAsync(ApplicationUserModel userDto);
        Task UpdateUserAsync(Guid id, ApplicationUserModel userDto);
        Task DeleteUserAsync(string id);
    }
} 