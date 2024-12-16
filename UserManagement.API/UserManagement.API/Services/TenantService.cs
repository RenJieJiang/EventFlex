using UserManagement.API.Models;
using UserManagement.API.Repositories;

namespace UserManagement.API.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
        {
            return await _tenantRepository.GetAllTenantsAsync();
        }

        public async Task<Tenant> GetTenantByIdAsync(Guid tenantId)
        {
            return await _tenantRepository.GetTenantByIdAsync(tenantId);
        }

        public async Task AddTenantAsync(Tenant tenant)
        {
            await _tenantRepository.AddTenantAsync(tenant);
        }

        public async Task UpdateTenantAsync(Tenant tenant)
        {
            await _tenantRepository.UpdateTenantAsync(tenant);
        }

        public async Task DeleteTenantAsync(Guid tenantId)
        {
            await _tenantRepository.DeleteTenantAsync(tenantId);
        }
    }
}
