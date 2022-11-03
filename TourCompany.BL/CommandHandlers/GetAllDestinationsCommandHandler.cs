using MediatR;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR;
using TourCompany.Models.Models;

namespace TourCompany.BL.CommandHandlers
{
    public record GetAllDestinationsCommandHandler : IRequestHandler<GetAllDestinationCommand, IEnumerable<Destination>>
    {
        private readonly IDestinationRepository _destinationRepository;

        public GetAllDestinationsCommandHandler(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }

        public async Task<IEnumerable<Destination>> Handle(GetAllDestinationCommand request, CancellationToken cancellationToken)
        {
            return await _destinationRepository.GetAllDestination();
        }
    }
}
