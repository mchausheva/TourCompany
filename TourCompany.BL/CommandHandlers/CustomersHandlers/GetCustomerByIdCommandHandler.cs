using MediatR;
using Microsoft.Extensions.Logging;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Models;

namespace TourCompany.BL.CommandHandlers.CustomersHandlers
{
    public record GetCustomerByIdCommandHandler : IRequestHandler<GetCustomerByIdCommand, Customer>
    {
        private readonly ILogger<GetCustomerByIdCommandHandler> _logger;
        private readonly ICustomerRespository _customerRespository;

        public GetCustomerByIdCommandHandler(ILogger<GetCustomerByIdCommandHandler> logger, ICustomerRespository customerRespository)
        {
            _logger = logger;
            _customerRespository = customerRespository;
        }

        public async Task<Customer> Handle(GetCustomerByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler --> Get Customer");

            try
            {
                return await _customerRespository.GetCustomerById(request.customerId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to get Customer --> {ex.Message}");
                return null;
            }
        }
    }
}
