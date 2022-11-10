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
    public record UpdateCustomerCommandHandler : IRequestHandler<UpdateAccountCommand, CustomerResponse>
    {
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        private readonly ICustomerRespository _customerRespository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, ICustomerRespository customerRespository, IMapper mapper)
        {
            _logger = logger;
            _customerRespository = customerRespository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exist = await _customerRespository.GetCustomerById(request.customerId);
                if(exist == null)
                {
                    return new CustomerResponse
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "The account doesn't exist."
                    };
                }
                
                var customer = _mapper.Map<Customer>(request.request);
                var result = await _customerRespository.UpdateAccount(request.customerId, customer);

                if (result == null)
                {
                    return new CustomerResponse
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "The account could not be updated."
                    };
                }

                return new CustomerResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You Successfully Updated Your Account!",
                    Customer = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"The update of your account failed.");
                return new CustomerResponse
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "The update of your account failed!"
                };
            }
        }
    }
}
