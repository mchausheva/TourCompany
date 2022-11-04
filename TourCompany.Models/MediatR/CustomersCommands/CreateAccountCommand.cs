using MediatR;
using TourCompany.Models.Requests;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR.Customers
{
    public record CreateAccountCommand(CustomerRequest request) : IRequest<CustomerResponse>
    {
        public readonly CustomerRequest customerRequest = request;
    }
}
