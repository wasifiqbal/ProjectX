using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectX.Data.EFCore;
using System;
using System.Reflection;

namespace ProjectX.Tests
{
	public abstract class TestBase
	{
		internal readonly ProjectXDbContext _context;
		internal readonly IMapper _mapper;
		public TestBase()
		{
			//Load InMemory DB
			var options = new DbContextOptionsBuilder<ProjectXDbContext>()
					.UseInMemoryDatabase(databaseName: $"ProjectX{Guid.NewGuid()}")
					.Options;
			_context = new ProjectXDbContext(options);

			//Load AutoMapper Profiles
			_mapper = new MapperConfiguration(config =>
			{
				config.AddMaps(Assembly.Load("ProjectX.Core"));
			}).CreateMapper();

		}
	}
}