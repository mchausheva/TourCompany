using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace TourCompany.HealthChecks
{
    //public class VersionHealthCheck : IHealthCheck
    //{
        //private readonly AppConfig _config;

        //public VersionHealthCheck(IOptions<AppConfig> options)
        //{
        //    _config = options.Value;
        //}

        //public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        //            CancellationToken cancellationToken = new CancellationToken())
        //{
        //    return Task.FromResult(string.IsNullOrEmpty(_config.Version)
        //        ? HealthCheckResult.Unhealthy()
        //        : HealthCheckResult.Healthy());
        //}
   // }
}
