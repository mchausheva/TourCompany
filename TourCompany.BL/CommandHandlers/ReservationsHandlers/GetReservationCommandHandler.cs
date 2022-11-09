using MediatR;
using Microsoft.Extensions.Logging;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Models;

namespace TourCompany.BL.CommandHandlers.ReservationsHandlers
{
    public class GetReservationCommandHandler : IRequestHandler<GetReservationCommand, Reservation>
    {
        private readonly ILogger<GetReservationCommandHandler> _logger;
        private readonly IReservationRepository _reservationRepository;

        public GetReservationCommandHandler(ILogger<GetReservationCommandHandler> logger, IReservationRepository reservationRepository)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> Handle(GetReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler - GET Reservation");

            try
            {
                return await _reservationRepository.GetById(request.reservationId, request.customerId);
            }
            catch (Exception ex) 
            {
                _logger.LogWarning($"Failed to get reservation --> {ex.Message}");
                return null;
            }
        }
    }
}
