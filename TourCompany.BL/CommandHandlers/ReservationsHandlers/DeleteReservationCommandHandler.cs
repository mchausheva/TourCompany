using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers.ReservationsHandlers
{
    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, ReservationResponse>
    {
        private readonly ILogger<DeleteReservationCommandHandler> _logger;
        private readonly IReservationRepository _reservationRepository;

        public DeleteReservationCommandHandler(ILogger<DeleteReservationCommandHandler> logger, IReservationRepository reservationRepository)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
        }

        public async Task<ReservationResponse> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler -> DELETE Reservation");

            try
            {
                var getResult = await _reservationRepository.GetById(request.reservationId, request.customerId);
                if (getResult == null)
                {
                    return new ReservationResponse
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = $"Not Found Reservation with Id = {request.reservationId} and Customer Id = {request.customerId}"
                    };
                }

                var result = await _reservationRepository.DeleteReservationById(request.reservationId, request.customerId);

                return new ReservationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You Successfully Deleted Your Reservation."
                };
            }
            catch (Exception ex) 
            {
                _logger.LogWarning($"The deletion of the reservation failed --> {ex.Message}");
                return new ReservationResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "The deletion of the reservation failed!"
                };
            }
        }
    }
}
