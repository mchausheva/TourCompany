using MediatR;
using Microsoft.Extensions.Logging;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR.Customers
{
    public record DeleteAccountCommand (int customerId) : IRequest<CustomerResponse>
    {
    }
}
