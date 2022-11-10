using MediatR;
using Microsoft.Extensions.Logging;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR;
using TourCompany.Models.Models;

namespace TourCompany.BL.CommandHandlers
{
    public record GetAllDestinationsCommandHandler : IRequestHandler<GetAllDestinationCommand, IEnumerable<Destination>>
    {
        private readonly ILogger<GetAllDestinationsCommandHandler> _logger;
        private readonly IDestinationRepository _destinationRepository;

        public GetAllDestinationsCommandHandler(ILogger<GetAllDestinationsCommandHandler> logger, IDestinationRepository destinationRepository)
        {
            _logger = logger;
            _destinationRepository = destinationRepository;
        }

        public async Task<IEnumerable<Destination>> Handle(GetAllDestinationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler --> Get Destinations ");
            try
            {
                return await _destinationRepository.GetAllDestination();
            }
            catch (Exception)
            {
                _logger.LogWarning("Failed to get all Destinations");
                return null;
            }
        }
    }
}
