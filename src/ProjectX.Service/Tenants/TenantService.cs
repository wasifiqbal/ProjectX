using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectX.Core.Tenants;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Data.EFCore;

namespace ProjectX.Service.Tenants
{
	public class TenantService : ITenantService
	{
		private readonly IProjectXRepository<Tenant, int> _repository;
		protected readonly IMapper _mapper;

		public TenantService(IProjectXRepository<Tenant, int> repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public IEnumerable<TenantDto> GetAll()
		{
			var tenants = _repository.GetAll().AsNoTracking().ToList();
			return _mapper.Map<List<TenantDto>>(tenants);
		}

		public async Task<TenantDto> GetAsync(int id)
		{
			var tenant = await _repository.GetByPrimaryKeyAsync(id);
			return _mapper.Map<TenantDto>(tenant);
		}

		public async Task<TenantDto> CreateAsync(CreateTenantDto input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));

			var tenant = _mapper.Map<Tenant>(input);
			var result = await _repository.InsertAsync(tenant);
			return _mapper.Map<TenantDto>(result);
		}

		public async Task UpdateAsync(UpdateTenantDto input)
		{
			var tenant = await _repository.GetByPrimaryKeyAsync(input.Id);
			_mapper.Map<UpdateTenantDto, Tenant>(input, tenant);
			await _repository.UpdateAsync(tenant);
		}

		public async Task DeleteAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
	}
}
