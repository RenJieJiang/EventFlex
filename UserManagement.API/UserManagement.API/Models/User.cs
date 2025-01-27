using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid? TenantId { get; set; }

        public Tenant? Tenant { get; set; }
    }
}
