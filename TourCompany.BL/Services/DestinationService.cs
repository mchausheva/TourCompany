using Microsoft.Extensions.Logging;
using TourCompany.BL.Interfaces;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Models;

namespace TourCompany.BL.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly ILogger<DestinationService> _logger;
        private readonly IDestinationRepository _destinationRepository;

        public DestinationService(ILogger<DestinationService> logger, IDestinationRepository destinationRepository)
        {
            _logger = logger;
            _destinationRepository = destinationRepository;
        }

        public async Task<IEnumerable<Destination>> GetAllDestinations()
        {
            _logger.LogInformation("GET all Destinations");
            return await _destinationRepository.GetAllDestination();
        }
    }
}
