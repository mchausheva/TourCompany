using MediatR;
using TourCompany.Models.Models;

namespace TourCompany.Models.MediatR.Reservations
{
    public record GetReservationCommand(int reservationId, int customerId) : IRequest<Reservation>
    {
    }
}
