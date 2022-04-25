using FluentAssertions;
using Moq;
using ProjectX.Core.Tenants;
using ProjectX.Data.EFCore;
using ProjectX.Service.Tenants;
using System;
using Xunit;

namespace ProjectX.Tests.Tenants
{
	public class Tenants_Test_Mock : TestBase
	{
		private readonly TenantService _tenantService;
		private readonly Mock<IProjectXRepository<ProjectX.Core.Tenants.Tenant, int>> _repository = new();

		public Tenants_Test_Mock()
		{
			_tenantService = new TenantService(_repository.Object, _mapper);
		}

		[Fact]
		public async void GetById_ShouldReturnTenants_WhenTenantExists()
		{
			//arrange
			var id = 1;
			var mockTenant = new Tenant { Name = "Default", Id = id, Code = "D01" };
			_repository.Setup(x => x.GetByPrimaryKeyAsync(id)).ReturnsAsync(mockTenant);
			//act
			var tenant = await _tenantService.GetAsync(id);
			//assert
			tenant.Id.Should().Be(id);
			tenant.Name.Should().Be(mockTenant.Name);
			tenant.Code.Should().Be(mockTenant.Code);
		}

		[Fact]
		public async void GetById_ShouldReturnTenants_WhenTenantNotExists()
		{
			//arrange
			_repository.Setup(x => x.GetByPrimaryKeyAsync(It.IsAny<int>())).ReturnsAsync(() => null);
			//act
			var tenant = await _tenantService.GetAsync(new Random().Next());
			//assert
			tenant.Should().BeNull();
		}
	}
}
