using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using TourCompany.DL.Interfaces;
using TourCompany.DL.Repositories;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.BL.Kafka
{
    public class CustomerConsumer : ConsumerHostedService<int, Customer>, IHostedService
    {
        private readonly ILogger<CustomerConsumer> _logger;
        private readonly IOptions<KafkaConfig> _kafkaConfig;
        private readonly ICustomerRespositoryMongo _customerRepositoryMongo;
        private readonly ICustomerRespository _customerRepository;
        private TransformBlock<Customer, Customer> _transformerBlock;
        private readonly IMapper _mapper;

        public CustomerConsumer(IOptions<KafkaConfig> kafkaConfig, ILogger<CustomerConsumer> logger, 
                    ICustomerRespositoryMongo customerRespositoryMongo, ICustomerRespository customerRespository, IMapper mapper)
                    : base(kafkaConfig, logger)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig;
            _customerRepositoryMongo = customerRespositoryMongo;
            _customerRepository = customerRespository;
            _mapper = mapper;

            _transformerBlock = new TransformBlock<Customer, Customer>( async customer =>
            {
                try
                {
                    var customerReservation = await _customerRepository.GetReservations(customer.CustomerId);

                    var mapCustomer = _mapper.Map<CustomerMongoRequest>(customer);
                    var existedCustomer = await _customerRepositoryMongo.GetCustomerById(customer.CustomerId);

                    if (existedCustomer != null)
                    {
                        _logger.LogInformation("UPDATE Customer in Mongo");


                        var temp = await _customerRepositoryMongo.UpdateCustomer(customer);

                        existedCustomer.ReservationCount += customerReservation.Count();

                        if (existedCustomer.ReservationCount > 20 && existedCustomer.ReservationCount <= 40)
                            _logger.LogInformation("Send a promo code for 5% off via email.");
                        else if (existedCustomer.ReservationCount > 40 && existedCustomer.ReservationCount <= 60)
                            _logger.LogInformation("Send a promo code for 10% off via email.");
                        else if (existedCustomer.ReservationCount > 60)
                            _logger.LogInformation("Send a promo code for 15% off via email.");

                        return _mapper.Map<Customer>(temp);
                    }
                    else
                    {
                        _logger.LogInformation("SAVE Customer in Mongo");

                        var temp = await _customerRepositoryMongo.SaveCustomer(customer);

                        temp.ReservationCount = customerReservation.Count();

                        return _mapper.Map<Customer>(temp);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception/Error in customer consumer -- > {ex.Message}");
                }
                return null;
            });

            var actionBlock = new ActionBlock<Customer>(customer =>
            {
                _transformerBlock.Post(customer);
                _logger.LogInformation($"RECEIVED Reservation Consumer ---> {customer.CustomerId} ");
            });

            _transformerBlock.LinkTo(actionBlock);
        }

        public override void HandleMessage(Customer value)
        {
            _logger.LogInformation("Handle Message");
            _transformerBlock.SendAsync(value);
        }
    }
}
