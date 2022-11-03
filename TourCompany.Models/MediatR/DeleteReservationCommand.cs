using MediatR;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR
{
    public record DeleteReservationCommand(int reservationId, int customerId) : IRequest<ReservationResponse>
    {
    }
}
