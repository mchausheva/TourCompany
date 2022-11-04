using MediatR;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR.Reservations
{
    public record DeleteReservationCommand(int reservationId, int customerId) : IRequest<ReservationResponse>
    {
    }
}
