using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.BL.Kafka
{
    public class ReservationConsumer : ConsumerHostedService<int, Reservation>, IHostedService
    {

        private readonly ILogger<ReservationConsumer> _logger;
        private readonly IOptions<KafkaConfig> _kafkaConfig;
        private readonly IReservationRepositoryMongo _resesrvationRepositoryMongo;
        private TransformBlock<Reservation, (Reservation, ReservationMongoRequest)> transformerBlock;
        private readonly IMapper _mapper;

        public ReservationConsumer(IOptions<KafkaConfig> kafkaConfig, ILogger<ReservationConsumer> logger, IReservationRepositoryMongo resesrvationRepository, IMapper mapper)
                    : base(kafkaConfig, logger)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig;

            _resesrvationRepositoryMongo = resesrvationRepository;
            _mapper = mapper;

            transformerBlock = new TransformBlock<Reservation, (Reservation, ReservationMongoRequest)>(async res =>
            {
                var reservation = _mapper.Map<ReservationMongoRequest>(res);
                var result = await _resesrvationRepositoryMongo.GetById(reservation.ReservationId);
                return (res, result);
            });

            var actionBlock = new ActionBlock<(Reservation, ReservationMongoRequest)>( async res =>
            {
                try
                {
                    if (res.Item2 != null)
                    {
                        if (res.Item2.ReservationDate < DateTime.Now)
                        {
                            _logger.LogInformation("DELETE Reservation from MongoDB");
                            var temp = await _resesrvationRepositoryMongo.DeleteReservationById(res.Item1.ReservationId, res.Item1.CustomerId);
                        }
                        else
                        {
                            _logger.LogInformation("UPDATE Reservation MongoDB");
                            var temp = await _resesrvationRepositoryMongo.UpdateReservation(res.Item1);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("SAVE Reservation in Mongo");
                        var temp = await _resesrvationRepositoryMongo.SaveReservation(res.Item1);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception/Error in consumer reservation -- > {ex.Message}");
                }
            });

            transformerBlock.LinkTo(actionBlock);
        }

        public override void HandleMessage(Reservation value)
        {
            _logger.LogInformation("Handle Message");

            transformerBlock.SendAsync(value);
        }
    }
}
