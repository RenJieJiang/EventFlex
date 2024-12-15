using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserManagement.API.Models;

namespace UserManagement.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<IdentityRole<Guid>>().Property(r => r.Id).HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Tenant>().Property(t => t.Id).HasDefaultValueSql("uuid_generate_v4()");

            // Seed data for Tenants
            var tenant1 = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Tenant 1",
                Description = "Description for Tenant 1"
            };

            var tenant2 = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Tenant 2",
                Description = "Description for Tenant 2"
            };

            modelBuilder.Entity<Tenant>().HasData(tenant1, tenant2);

            // Seed data for Users
            var hasher = new PasswordHasher<User>();

            var user1 = new User
            {
                Id = Guid.NewGuid(),
                UserName = "user1@example.com",
                NormalizedUserName = "USER1@EXAMPLE.COM",
                Email = "user1@example.com",
                NormalizedEmail = "USER1@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                SecurityStamp = string.Empty,
                TenantId = tenant1.Id
            };

            var user2 = new User
            {
                Id = Guid.NewGuid(),
                UserName = "user2@example.com",
                NormalizedUserName = "USER2@EXAMPLE.COM",
                Email = "user2@example.com",
                NormalizedEmail = "USER2@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                SecurityStamp = string.Empty,
                TenantId = tenant2.Id
            };

            modelBuilder.Entity<User>().HasData(user1, user2);
        }

        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
