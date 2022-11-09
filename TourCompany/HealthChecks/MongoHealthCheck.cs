using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;

namespace TourCompany.HealthChecks
{
    public class MongoHealthCheck : IHealthCheck
    {
        private readonly IOptions<MongoDbConfiguration> _options;

        public MongoHealthCheck(IOptions<MongoDbConfiguration> options)
        {
            _options = options;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var dbClient = new MongoClient(_options.Value.ConnectionString);
                var database = dbClient.GetDatabase(_options.Value.DatabaseName);
                var collection = database.GetCollection<Reservation>(_options.Value.CollectionName);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }
            return HealthCheckResult.Healthy("MongoDB connection is OK");
        }
    }
}
