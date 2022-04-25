using ProjectX.Core.Tenants.DTO;

namespace ProjectX.Service.Tenants
{
	public interface ITenantService
	{
		IEnumerable<TenantDto> GetAll();
		Task<TenantDto> GetAsync(int id);
		Task<TenantDto> CreateAsync(CreateTenantDto input);
		Task UpdateAsync(UpdateTenantDto input);
		Task DeleteAsync(int id);
	}
}
