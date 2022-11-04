using MediatR;
using TourCompany.Models.Models;

namespace TourCompany.Models.MediatR.Customers
{
    public record GetCustomerReservationsCommand (int customerId) : IRequest<IEnumerable<Reservation>>
    {
    }
}
