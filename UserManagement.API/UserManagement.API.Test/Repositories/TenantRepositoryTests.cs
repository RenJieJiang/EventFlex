using Microsoft.EntityFrameworkCore;
using Moq;
using UserManagement.API.Data;
using UserManagement.API.Models;
using UserManagement.API.Repositories;

namespace UserManagement.API.Tests.Repositories
{
    public class TenantRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly TenantRepository _tenantRepository;

        public TenantRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EventFlex.UserManagement")
                .Options;

            _context = new ApplicationDbContext(options);
            _tenantRepository = new TenantRepository(_context);
        }

        [Fact]
        public async Task GetAllTenantsAsync_ReturnsListOfTenants()
        {
            // Arrange
            var tenants = new List<Tenant> { new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" } };
            _context.Tenants.AddRange(tenants);
            await _context.SaveChangesAsync();

            // Act
            var result = await _tenantRepository.GetAllTenantsAsync();

            // Assert
            Assert.Equal(tenants, result);
        }

        [Fact]
        public async Task GetTenantByIdAsync_ReturnsTenant()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant { Id = tenantId, Name = "Tenant1" };
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _tenantRepository.GetTenantByIdAsync(tenantId);

            // Assert
            Assert.Equal(tenant, result);
        }

        [Fact]
        public async Task AddTenantAsync_AddsTenant()
        {
            // Arrange
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" };

            // Act
            await _tenantRepository.AddTenantAsync(tenant);

            // Assert
            var addedTenant = await _context.Tenants.FindAsync(tenant.Id);
            Assert.Equal(tenant, addedTenant);
        }

        [Fact]
        public async Task UpdateTenantAsync_UpdatesTenant()
        {
            // Arrange
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" };
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            tenant.Name = "UpdatedTenant";

            // Act
            await _tenantRepository.UpdateTenantAsync(tenant);

            // Assert
            var updatedTenant = await _context.Tenants.FindAsync(tenant.Id);
            Assert.Equal("UpdatedTenant", updatedTenant.Name);
        }

        [Fact]
        public async Task DeleteTenantAsync_DeletesTenant()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant { Id = tenantId, Name = "Tenant1" };
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            // Act
            await _tenantRepository.DeleteTenantAsync(tenantId);

            // Assert
            var deletedTenant = await _context.Tenants.FindAsync(tenantId);
            Assert.Null(deletedTenant);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}