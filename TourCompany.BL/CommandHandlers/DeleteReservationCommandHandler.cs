using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers
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
            _logger.LogInformation("Command Handler -> DELETE");
            var result = await _reservationRepository.DeleteReservationById(request.reservationId, request.customerId);

            return new ReservationResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Message = "You Successfully Deleted Your Reservation."
            };
        }
    }
}
