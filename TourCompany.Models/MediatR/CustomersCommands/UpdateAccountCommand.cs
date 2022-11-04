using MediatR;
using TourCompany.Models.Requests;
using TourCompany.Models.Responses;

namespace TourCompany.Models.MediatR.Customers
{
    public record UpdateAccountCommand (int customerId, CustomerRequest request) : IRequest<CustomerResponse>   
    {
        private readonly int id = customerId;
        private readonly CustomerRequest customerRequest = request;
    }
}
