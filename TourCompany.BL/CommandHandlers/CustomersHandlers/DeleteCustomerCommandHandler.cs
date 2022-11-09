using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers.CustomersHandlers
{
    public record DeleteCustomerCommandHandler : IRequestHandler<DeleteAccountCommand, CustomerResponse>
    {
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;
        private readonly ICustomerRespository _customerRepository;

        public DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommandHandler> logger, ICustomerRespository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler -> DELETE Account");
            var result = await _customerRepository.DeleteAccount(request.customerId);

            return new CustomerResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Message = "You Successfully Deleted Your Account."
            };
        }
    }
}
