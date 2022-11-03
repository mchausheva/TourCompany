using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Models;
using Dapper;

namespace TourCompany.DL.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly ILogger<DestinationRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IList<Destination> _destination;
        public DestinationRepository(ILogger<DestinationRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _destination = new List<Destination>();
        }
        public async Task<IEnumerable<Destination>> GetAllDestination()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    //return await conn.QueryAsync<Destination>("prcSelectDestination");

                    var countries = await conn.QueryAsync<Country>("SELECT * FROM Country WITH(NOLOCK)");
                    var cities = await conn.QueryAsync<City>("SELECT * FROM City WITH(NOLOCK)");


                    foreach (var country in countries)
                    {
                        _destination.Add(new Destination
                        {
                            Country = country,
                            Cities = cities.Where(c => c.CountryId == country.CountryId).ToList()
                        });
                    }

                    return _destination;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAllDestination)} - {ex.Message}", ex);
                return Enumerable.Empty<Destination>();
            }
        }
    }
}
