using FluentAssertions;
using ProjectX.Core.Tenants;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Data.EFCore;
using ProjectX.Service.Tenants;
using System;
using Xunit;

namespace ProjectX.Tests.Tenants
{
	public class Tenants_Test_InMemory : TestBase
	{
		private readonly TenantService _tenantService;
		private readonly IProjectXRepository<ProjectX.Core.Tenants.Tenant, int> _repository;

		public Tenants_Test_InMemory()
		{
			_repository = new BaseProjectXRepository<Tenant, int>(_context);
			_tenantService = new TenantService(_repository, _mapper);
		}

		[Fact]
		public async void GetById_ShouldReturnTenants_WhenTenantExists()
		{
			//arrange
			var input = new CreateTenantDto { Name = "Default", Code = "D01" };
			var saved = await _tenantService.CreateAsync(input);
			//act
			var tenant = await _tenantService.GetAsync(saved.Id);
			//assert
			tenant.Id.Should().Be(saved.Id);
			tenant.Name.Should().Be(input.Name);
			tenant.Code.Should().Be(input.Code);
		}

		[Fact]
		public async void GetById_ShouldReturnTenants_WhenTenantNotExists()
		{
			//act
			var tenant = await _tenantService.GetAsync(new Random().Next());
			//assert
			tenant.Should().BeNull();
		}
	}
}
