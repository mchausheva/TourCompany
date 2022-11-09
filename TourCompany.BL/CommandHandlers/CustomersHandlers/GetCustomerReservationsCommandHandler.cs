using MediatR;
using Microsoft.Extensions.Logging;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Models;

namespace TourCompany.BL.CommandHandlers.CustomersHandlers
{
    public record GetCustomerReservationsCommandHandler : IRequestHandler<GetCustomerReservationsCommand, IEnumerable<Reservation>>
    {
        private readonly ILogger<GetCustomerReservationsCommandHandler> _logger;
        private readonly ICustomerRespository _customerRespository;

        public GetCustomerReservationsCommandHandler(ILogger<GetCustomerReservationsCommandHandler> logger, ICustomerRespository customerRespository)
        {
            _logger = logger;
            _customerRespository = customerRespository;
        }

        public async Task<IEnumerable<Reservation>> Handle(GetCustomerReservationsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler --> Get Customer's Reservations");

            var customer = _customerRespository.GetCustomerById(request.customerId).Result;

            if (customer == null) return null;

            return await _customerRespository.GetReservations(request.customerId);
        }
    }
}
