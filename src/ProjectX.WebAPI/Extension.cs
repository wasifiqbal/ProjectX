using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectX.Data.EFCore;

namespace ProjectX.WebAPI
{
	public static class Extension
	{
		public static WebApplication SeedData(this WebApplication app)
		{
			var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
			using (var scope = scopeFactory.CreateScope())
			{
				using ProjectXDbContext context = scope.ServiceProvider.GetService<ProjectXDbContext>();
				new SeedHelper(context).CreateData();
			}
			return app;
		}

		public static WebApplication UseHealthCheckSettings(this WebApplication app)
		{
			app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
				ResultStatusCodes =
				{
					[HealthStatus.Healthy] = StatusCodes.Status200OK,
					[HealthStatus.Degraded] = StatusCodes.Status200OK,
					[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
				},
				AllowCachingResponses = false
			});
			app.MapHealthChecks("/health-sql", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
			{
				Predicate = healthcheck => healthcheck.Tags.Contains("database"),
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
				ResultStatusCodes =
				{
					[HealthStatus.Healthy] = StatusCodes.Status200OK,
					[HealthStatus.Degraded] = StatusCodes.Status200OK,
					[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
				},
				AllowCachingResponses = false
			});
			app.MapHealthChecksUI();

			return app;
		}
	}
}
