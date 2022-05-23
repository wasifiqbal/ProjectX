using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProjectX.WebAPI.Filters
{
	public class ProjectXHealthCheck : IHealthCheck
	{
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			var isHealthy = true;

			//HealthCheck for ThirdPartyAPI
			//HealthCheck for Logging Storage

			if(isHealthy)
			{
				return Task.FromResult(HealthCheckResult.Healthy("Healthy result from ProjectXHealthCheck"));
			}
			return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "Unhealthy result from ProjectXHealthCheck"));
		}
	}
}
