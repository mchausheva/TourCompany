using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Models;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers.CustomersHandlers
{
    public record CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CustomerResponse>
    {
        private readonly ILogger<CreateAccountCommandHandler> _logger;
        private readonly ICustomerRespository _customerRespository;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(ILogger<CreateAccountCommandHandler> logger, ICustomerRespository customerRespository, IMapper mapper)
        {
            _logger = logger;
            _customerRespository = customerRespository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customerExist = _customerRespository.GetCustomerByEmail(request.customerRequest.Email).Result;

                if (customerExist != null)
                {
                    return new CustomerResponse
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "This account already exists."
                    };
                }

                var customer = _mapper.Map<Customer>(request.request);
                var result = await _customerRespository.CreateAccount(customer);

                return new CustomerResponse
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You Successfully Created Your account!",
                    Customer = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"The Account Creation Failed");
                throw;
            }
        }
    }
}
