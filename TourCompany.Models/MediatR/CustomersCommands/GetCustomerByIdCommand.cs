using MediatR;
using TourCompany.Models.Models;

namespace TourCompany.Models.MediatR.Customers
{
    public record GetCustomerByIdCommand (int customerId) : IRequest<Customer>
    {
    }
}
