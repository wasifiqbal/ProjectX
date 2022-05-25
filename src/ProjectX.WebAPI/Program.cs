using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectX.Data.EFCore;
using ProjectX.Service.Tenants;
using ProjectX.WebAPI;
using ProjectX.WebAPI.Filters;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProjectXDbContext>(options =>
{
	options.UseSqlServer(configuration.GetConnectionString("Default"),
			assembly => assembly.MigrationsAssembly(typeof(ProjectXDbContext).Assembly.FullName));
});
builder.Services.AddHealthChecks()
	.AddSqlServer(builder.Configuration.GetConnectionString("Default"), name: "ProjectXDB", failureStatus: HealthStatus.Unhealthy,  tags: new[] { "database" })
	.AddCheck<ProjectXHealthCheck>("Internal", HealthStatus.Degraded, new[] { "Internal" }, new TimeSpan(0, 0, 60));

builder.Host.UseSerilog((ctx, lc) => 
		lc
	   .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddHealthChecksUI().AddSqlServerStorage(builder.Configuration.GetConnectionString("HealthCheck"));

builder.Services.AddTransient<SeedHelper>();
builder.Services.AddTransient(typeof(IProjectXRepository<,>), typeof(BaseProjectXRepository<,>));
builder.Services.AddAutoMapper(System.Reflection.Assembly.Load("ProjectX.Core"));
builder.Services.AddTransient(typeof(ITenantService), typeof(TenantService));


var app = builder.Build();

app.SeedData();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.UseHealthCheckSettings();

app.UseSerilogRequestLogging();

app.Run();

