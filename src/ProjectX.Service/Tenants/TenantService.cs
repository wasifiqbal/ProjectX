using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectX.Core.Tenants;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Data.EFCore;
using System.ComponentModel.DataAnnotations;

namespace ProjectX.Service.Tenants
{
	public class TenantService : ITenantService
	{
		private readonly IProjectXRepository<Tenant, int> _repository;
		protected readonly IMapper _mapper;
		private readonly ILogger<TenantService> _logger;

		public TenantService(IProjectXRepository<Tenant, int> repository, IMapper mapper,
			ILogger<TenantService> logger)
		{
			_repository = repository;
			_mapper = mapper;
			_logger = logger;
		}

		public IList<TenantDto> GetAll()
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
			Validator.ValidateObject(input, new ValidationContext(input));
			var tenant = _mapper.Map<Tenant>(input);
			var result = await _repository.InsertAsync(tenant);
			return _mapper.Map<TenantDto>(result);
		}

		public async Task<TenantDto> UpdateAsync(UpdateTenantDto input)
		{
			Validator.ValidateObject(input, new ValidationContext(input));
			var tenant = await _repository.GetByPrimaryKeyAsync(input.Id);
			_mapper.Map<UpdateTenantDto, Tenant>(input, tenant);
			var result = await _repository.UpdateAsync(tenant);
			return _mapper.Map<TenantDto>(result);
		}

		public async Task DeleteAsync(int id)
		{
			var tenant = await _repository.GetByPrimaryKeyAsync(id);
			if (tenant == null)
				throw new NullReferenceException();

			await _repository.DeleteAsync(id);
		}
	}
}
