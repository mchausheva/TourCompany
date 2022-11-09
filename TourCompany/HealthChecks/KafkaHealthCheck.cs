using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using TourCompany.Models.Configurations;

namespace TourCompany.HealthChecks
{
    public class KafkaHealthCheck : IHealthCheck
    {
        private readonly KafkaConfig _config;

        public KafkaHealthCheck(IOptions<KafkaConfig> options)
        {
            _config = options.Value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                    CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(string.IsNullOrEmpty(_config.BootstrapServers)
                ? HealthCheckResult.Unhealthy("Kafka settings are NULL")
                : HealthCheckResult.Healthy("Kafka settings are OK"));
        }
    }
}
