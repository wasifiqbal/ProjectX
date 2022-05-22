using ProjectX.Core.Tenants.DTO;

namespace ProjectX.Service.Tenants
{
	public interface ITenantService
	{
		IList<TenantDto> GetAll();
		Task<TenantDto> GetAsync(int id);
		Task<TenantDto> CreateAsync(CreateTenantDto input);
		Task<TenantDto> UpdateAsync(UpdateTenantDto input);
		Task DeleteAsync(int id);
	}
}
