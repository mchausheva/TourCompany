using MediatR;
using Microsoft.Extensions.Logging;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR;
using TourCompany.Models.Models;

namespace TourCompany.BL.CommandHandlers
{
    public class GetReservationCommandHandler : IRequestHandler<GetReservationCommand, Reservation>
    {
        private readonly ILogger<GetAllDestinationsCommandHandler> _logger;
        private readonly IReservationRepository _reservationRepository;

        public GetReservationCommandHandler(ILogger<GetAllDestinationsCommandHandler> logger, IReservationRepository reservationRepository)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> Handle(GetReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler - GET Reservation");
            return await _reservationRepository.GetById(request.reservationId, request.customerId);
        }
    }
}
