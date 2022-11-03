using MediatR;
using TourCompany.Models.Models;

namespace TourCompany.Models.MediatR
{
    public record GetReservationCommand (int reservationId, int customerId) : IRequest<Reservation>
    {
    }
}
