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
        }
    }
}
