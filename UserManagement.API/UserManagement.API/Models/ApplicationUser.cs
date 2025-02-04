using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
