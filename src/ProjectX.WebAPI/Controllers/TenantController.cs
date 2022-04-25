using Microsoft.AspNetCore.Mvc;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Service.Tenants;

namespace ProjectX.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TenantController : ControllerBase
	{
		private readonly ITenantService _tenantService;

		public TenantController(ITenantService tenantService)
		{
			_tenantService = tenantService;
		}
		[HttpGet]
		public IEnumerable<TenantDto> Get()
		{
			return _tenantService.GetAll();
		}

		[HttpGet("{id}")]
		public async Task<TenantDto> Get(int id)
		{
			return await _tenantService.GetAsync(id);
		}

		[HttpPost]
		public async Task PostAsync([FromBody] CreateTenantDto input)
		{
			await _tenantService.CreateAsync(input);
		}

		[HttpPut("{id}")]
		public async Task PutAsync(int id, [FromBody] UpdateTenantDto input)
		{
			input.Id = id;
			await _tenantService.UpdateAsync(input);
		}

		[HttpDelete("{id}")]
		public async Task DeleteAsync(int id)
		{
			await _tenantService.DeleteAsync(id);
		}
	}
}
