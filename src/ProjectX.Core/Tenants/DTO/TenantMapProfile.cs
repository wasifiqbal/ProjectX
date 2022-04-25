using AutoMapper;

namespace ProjectX.Core.Tenants.DTO
{
	public class TenantMapProfile : Profile
	{
		public TenantMapProfile()
		{
			CreateMap<Tenant, TenantDto>();
			CreateMap<CreateTenantDto, Tenant>();
			CreateMap<UpdateTenantDto, Tenant>();
		}
	}
}
