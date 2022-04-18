using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
