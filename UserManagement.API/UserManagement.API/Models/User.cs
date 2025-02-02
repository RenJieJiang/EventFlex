using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty; // store the user's name, UserName(email) is used for login

        public Guid? TenantId { get; set; }

        public Tenant? Tenant { get; set; }
    }
}
