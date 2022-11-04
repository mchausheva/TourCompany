using MediatR;
using TourCompany.Models.Requests;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR.Reservations
{
    public record UpdateReservationCommand(int id, ReservationRequest request) : IRequest<ReservationResponse>
    {
        public readonly int reservationId = id;
        public readonly ReservationRequest reservationRequest = request;
    }
}
