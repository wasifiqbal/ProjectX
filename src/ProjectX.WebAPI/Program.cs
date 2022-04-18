using Microsoft.EntityFrameworkCore;
using ProjectX.Data.EFCore;
using ProjectX.WebAPI;

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

builder.Services.AddTransient<SeedHelper>();

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

app.Run();

