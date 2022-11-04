using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;

namespace TourCompany.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SqlHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await conn.OpenAsync(cancellationToken);
                }
                catch (SqlException ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message);
                }
                return HealthCheckResult.Healthy("SQL coonection is OK");
            }
        }
    }
}
