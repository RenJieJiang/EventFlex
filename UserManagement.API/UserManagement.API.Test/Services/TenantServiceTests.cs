using Moq;
using Xunit;
using UserManagement.API.Models;
using UserManagement.API.Repositories;
using UserManagement.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.API.Tests.Services
{
    public class TenantServiceTests
    {
        private readonly Mock<ITenantRepository> _mockTenantRepository;
        private readonly TenantService _tenantService;

        public TenantServiceTests()
        {
            _mockTenantRepository = new Mock<ITenantRepository>();
            _tenantService = new TenantService(_mockTenantRepository.Object);
        }

        [Fact]
        public async Task GetAllTenantsAsync_ReturnsListOfTenants()
        {
            // Arrange
            var tenants = new List<Tenant> { new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" } };
            _mockTenantRepository.Setup(repo => repo.GetAllTenantsAsync())
                                 .ReturnsAsync(tenants);

            // Act
            var result = await _tenantService.GetAllTenantsAsync();

            // Assert
            Assert.Equal(tenants, result);
        }

        [Fact]
        public async Task GetTenantByIdAsync_ReturnsTenant()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant { Id = tenantId, Name = "Tenant1" };
            _mockTenantRepository.Setup(repo => repo.GetTenantByIdAsync(tenantId))
                                 .ReturnsAsync(tenant);

            // Act
            var result = await _tenantService.GetTenantByIdAsync(tenantId);

            // Assert
            Assert.Equal(tenant, result);
        }

        [Fact]
        public async Task GetTenantByIdAsync_ReturnsNull_WhenTenantDoesNotExist()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _mockTenantRepository.Setup(repo => repo.GetTenantByIdAsync(tenantId))
                                 .ReturnsAsync((Tenant)null);

            // Act
            var result = await _tenantService.GetTenantByIdAsync(tenantId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddTenantAsync_CallsRepository()
        {
            // Arrange
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" };

            // Act
            await _tenantService.AddTenantAsync(tenant);

            // Assert
            _mockTenantRepository.Verify(repo => repo.AddTenantAsync(tenant), Times.Once);
        }

        [Fact]
        public async Task UpdateTenantAsync_CallsRepository()
        {
            // Arrange
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" };

            // Act
            await _tenantService.UpdateTenantAsync(tenant);

            // Assert
            _mockTenantRepository.Verify(repo => repo.UpdateTenantAsync(tenant), Times.Once);
        }

        [Fact]
        public async Task DeleteTenantAsync_CallsRepository()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            // Act
            await _tenantService.DeleteTenantAsync(tenantId);

            // Assert
            _mockTenantRepository.Verify(repo => repo.DeleteTenantAsync(tenantId), Times.Once);
        }
    }
}