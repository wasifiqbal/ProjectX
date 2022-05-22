using FluentAssertions;
using ProjectX.Core.Tenants;
using ProjectX.Core.Tenants.DTO;
using ProjectX.Data.EFCore;
using ProjectX.Service.Tenants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
		public async Task GetAll_Should_Return_ListOfTenantsAsync()
		{
			//arrange
			var mockTenants = new List<CreateTenantDto>() { new CreateTenantDto { Name = "Default 1",  Code = "D01" }, new CreateTenantDto { Name = "Default 2", Code = "D02" } };
			foreach(var input in mockTenants)
				await _tenantService.CreateAsync(input);
			//act
			var tenant = _tenantService.GetAll();
			//assert
			tenant.Should().HaveCount(2);
		}

		[Fact]
		public void GetAll_Should_Return_EmptyList()
		{
			//arrange
			//act
			var tenant = _tenantService.GetAll();
			//assert
			tenant.Should().HaveCount(0);
		}

		[Fact]
		public async void Get_Should_Return_Tenant_WhenTenantExists()
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
		public async void Get_Should_Return_Nothing_WhenTenantNotExists()
		{
			//act
			var tenant = await _tenantService.GetAsync(new Random().Next());
			//assert
			tenant.Should().BeNull();
		}

		[Fact]
		public async void Create_Should_CreateTenants()
		{
			//arrange
			var input = new CreateTenantDto { Name = "Default", Code = "D01" };
			//act
			var saved = await _tenantService.CreateAsync(input);
			//assert
			saved.Id.Should().BeGreaterThan(0);
			var db = await _tenantService.GetAsync(saved.Id);
			db.Should().NotBe(null);
			db.Name.Should().Be(input.Name);
			db.Code.Should().Be(input.Code);
		}

		[Fact]
		public async Task Create_Should_NotCreate_NullInput()
		{
			//arrange
			CreateTenantDto? input = null;
			//act
			var res = () => _tenantService.CreateAsync(input);
			//assert
			await Assert.ThrowsAsync<ArgumentNullException>(res);
		}

		[Fact]
		public async Task Create_Should_NotCreate_ModelValidationError()
		{
			//arrange
			var input = new CreateTenantDto { Name = String.Empty, Code = "D01" };
			//act
			var res = () => _tenantService.CreateAsync(input);
			//assert
			await Assert.ThrowsAsync<ValidationException>(res);
		}

		[Fact]
		public async void Update_Should_UpdateTenant()
		{
			//arrange
			var tenant = new CreateTenantDto() {  Name = "Mock", Code = "M01" };
			var savedTenant = await _tenantService.CreateAsync(tenant);
			var model = new UpdateTenantDto() { Id = savedTenant.Id, Name = "Default", Code = "D01" };
			//act
			var updatedTenant = await _tenantService.UpdateAsync(model);
			//assert
			updatedTenant.Name.Should().Be(model.Name);
			updatedTenant.Code.Should().Be(model.Code);
			updatedTenant.Name.Should().NotBe(tenant.Name);
			updatedTenant.Code.Should().NotBe(tenant.Code);
		}

		[Fact]
		public async void Update_Should_NotUpdate_ModelValidationError()
		{
			//arrange
			var tenant = new CreateTenantDto() { Name = "Mock", Code = "M01" };
			var savedTenant = await _tenantService.CreateAsync(tenant);
			var model = new UpdateTenantDto() { Id = savedTenant.Id, Name = String.Empty, Code = "D01" };
			//act
			var res = () => _tenantService.UpdateAsync(model);
			//assert
			await Assert.ThrowsAsync<ValidationException>(res);
		}

		[Fact]
		public async void Delete_Should_DeleteTenant()
		{
			//arrange
			var tenant = new CreateTenantDto() { Name = "Mock", Code = "M01" };
			var savedTenant = await _tenantService.CreateAsync(tenant);
			//act
			await _tenantService.DeleteAsync(savedTenant.Id);
			//assert
			var tenantAfterDelete = await _tenantService.GetAsync(savedTenant.Id);
			tenantAfterDelete.Should().BeNull();
		}

		[Fact]
		public async void Delete_Should_NotDelete()
		{
			//arrange
			var id = new Random().Next();
			//act
			var res = () => _tenantService.DeleteAsync(id);
			//assert
			await Assert.ThrowsAsync<NullReferenceException>(res);
		}
	}
}
