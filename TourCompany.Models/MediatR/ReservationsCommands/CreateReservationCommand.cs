using MediatR;
using TourCompany.Models.Requests;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR.Reservations
{
    public record CreateReservationCommand(ReservationRequest request) : IRequest<ReservationResponse>
    {
        public readonly ReservationRequest reservationRequest = request;
    }
}
