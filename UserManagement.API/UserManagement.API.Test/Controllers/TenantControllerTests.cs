using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Controllers;
using UserManagement.API.Services;
using UserManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.API.Tests.Controllers
{
    public class TenantControllerTests
    {
        private readonly Mock<ITenantService> _mockTenantService;
        private readonly TenantController _controller;

        public TenantControllerTests()
        {
            _mockTenantService = new Mock<ITenantService>();
            _controller = new TenantController(_mockTenantService.Object);
        }

        [Fact]
        public async Task GetAllTenants_ReturnsOkResult_WithListOfTenants()
        {
            // Arrange
            var tenants = new List<Tenant> { new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" } };
            _mockTenantService.Setup(service => service.GetAllTenantsAsync()).ReturnsAsync(tenants);

            // Act
            var result = await _controller.GetAllTenants();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Tenant>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetTenantById_ReturnsOkResult_WithTenant()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant { Id = tenantId, Name = "Tenant1" };
            _mockTenantService.Setup(service => service.GetTenantByIdAsync(tenantId))
                              .ReturnsAsync(tenant);

            // Act
            var result = await _controller.GetTenantById(tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Tenant>(okResult.Value);
            Assert.Equal(tenantId, returnValue.Id);
        }

        [Fact]
        public async Task GetTenantById_ReturnsNotFound_WhenTenantDoesNotExist()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _mockTenantService.Setup(service => service.GetTenantByIdAsync(tenantId))
                              .ReturnsAsync((Tenant)null);

            // Act
            var result = await _controller.GetTenantById(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddTenant_ReturnsCreatedAtAction_WithTenant()
        {
            // Arrange
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" };

            // Act
            var result = await _controller.AddTenant(tenant);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Tenant>(createdAtActionResult.Value);
            Assert.Equal(tenant.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateTenant_ReturnsNoContent()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant { Id = tenantId, Name = "Tenant1" };

            // Act
            var result = await _controller.UpdateTenant(tenantId, tenant);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTenant_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Tenant1" };

            // Act
            var result = await _controller.UpdateTenant(tenantId, tenant);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTenant_ReturnsNoContent()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteTenant(tenantId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}