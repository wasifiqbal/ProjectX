using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectX.Core.Tenants;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Data.EFCore;
using ProjectX.Service.Tenants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectX.Tests.Tenants
{
	public class Tenants_Test_Mock : TestBase
	{
		private readonly ILogger<TenantService> _logger;
		private readonly TenantService _tenantService;
		private readonly Mock<IProjectXRepository<ProjectX.Core.Tenants.Tenant, int>> _repository = new();

		public Tenants_Test_Mock()
		{
			_logger = new Mock<ILogger<TenantService>>().Object;
			_tenantService = new TenantService(_repository.Object, _mapper, _logger);
		}

		[Fact]
		public  void GetAll_Should_Return_ListOfTenants()
		{
			//arrange
			var id = 1;
			var mockTenants = new List<Tenant>() { new Tenant { Name = "Default", Id = id, Code = "D01" }, new Tenant { Name = "Default", Id = id, Code = "D01" } };
			_repository.Setup(x => x.GetAll()).Returns(mockTenants.AsQueryable());
			//act
			var tenant = _tenantService.GetAll();
			//assert
			tenant.Should().HaveCount(2);
		}

		[Fact]
		public  void GetAll_Should_Return_EmptyList()
		{
			//arrange
			var mockTenants = new List<Tenant>() {  };
			_repository.Setup(x => x.GetAll()).Returns(mockTenants.AsQueryable()); ;
			//act
			var tenant = _tenantService.GetAll();
			//assert
			tenant.Should().HaveCount(0);
		}

		[Fact]
		public async void Get_Should_Return_Tenant_WhenTenantExists()
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
		public async void Get_Should_Return_Nothing_WhenTenantNotExists()
		{
			//arrange
			_repository.Setup(x => x.GetByPrimaryKeyAsync(It.IsAny<int>())).ReturnsAsync(() => null);
			//act
			var tenant = await _tenantService.GetAsync(new Random().Next());
			//assert
			tenant.Should().BeNull();
		}

		[Fact]
		public async void Create_Should_CreateTenant()
		{
			//arrange
			var mockTenant = _repository.Setup(x => x.InsertAsync(It.IsAny<Tenant>()));
			var model = new CreateTenantDto() { Name = "Default", Code = "D01" };
			//act
			var tenant = await _tenantService.CreateAsync(model);
			//assert
			_repository.Verify(x => x.InsertAsync(It.IsAny<Tenant>()), Times.Once());
		}


		[Fact]
		public async Task Create_Should_NotCreate_NullInput()
		{
			//arrange
			CreateTenantDto? input = default;
			//act
			var res = () => _tenantService.CreateAsync(input);
			//assert
			await Assert.ThrowsAsync<ArgumentNullException>(res);
		}

		[Fact]
		public async void Create_Should_NotCreate_ModelValidationError()
		{
			//arrange
			var model = new CreateTenantDto() { Name = string.Empty, Code = "D01" };
			//act
			var res = () => _tenantService.CreateAsync(model);
			//assert
			await Assert.ThrowsAsync<ValidationException>(res);
			_repository.Verify(x => x.InsertAsync(It.IsAny<Tenant>()), Times.Never());
		}


		[Fact]
		public async void Update_Should_UpdateTenant()
		{
			//arrange
			var id = new Random().Next();
			var tenant = new Tenant() { Id = id, Name = "Mock", Code = "M01" };
			_repository.Setup(x => x.GetByPrimaryKeyAsync(id)).ReturnsAsync(tenant);
			_repository.Setup(x => x.UpdateAsync(It.IsAny<Tenant>()));
			var model = new UpdateTenantDto() { Id = id, Name = "Default", Code = "D01" };
			//act
			await _tenantService.UpdateAsync(model);
			//assert
			_repository.Verify(x => x.UpdateAsync(It.IsAny<Tenant>()), Times.Once());
		}

		[Fact]
		public async void Update_Should_NotUpdate_ModelValidationError()
		{
			//arrange
			var id = new Random().Next();
			var tenant = new Tenant() { Id = id, Name = "Mock", Code = "M01" };
			_repository.Setup(x => x.GetByPrimaryKeyAsync(id)).ReturnsAsync(tenant);
			_repository.Setup(x => x.UpdateAsync(It.IsAny<Tenant>()));
			var model = new UpdateTenantDto() { Id = id, Name = String.Empty, Code = "D01" };
			//act
			var res = () => _tenantService.UpdateAsync(model);
			//assert
			await Assert.ThrowsAsync<ValidationException>(res);
			_repository.Verify(x => x.InsertAsync(It.IsAny<Tenant>()), Times.Never());
		}

		[Fact]
		public async void Delete_Should_DeleteTenant()
		{
			//arrange
			var id = new Random().Next();
			var tenant = new Tenant() { Id = id, Name = "Mock", Code = "M01" };
			_repository.Setup(x => x.GetByPrimaryKeyAsync(id)).ReturnsAsync(tenant);
			//act
			await _tenantService.DeleteAsync(id);
			//assert
			_repository.Verify(x => x.DeleteAsync(id), Times.Once());
		}

		[Fact]
		public async void Delete_Should_NotDelete()
		{
			//arrange
			var id = new Random().Next();
			_repository.Setup(x => x.GetByPrimaryKeyAsync(id)).ReturnsAsync(() => null);
			//act
			var res = () => _tenantService.DeleteAsync(id);
			//assert
			await Assert.ThrowsAsync<NullReferenceException>(res);
			_repository.Verify(x => x.DeleteAsync(id), Times.Never());
		}
	}
}
