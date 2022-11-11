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
        private TransformBlock<Reservation, Reservation> transformerBlock;
        private readonly IMapper _mapper;

        public ReservationConsumer(IOptions<KafkaConfig> kafkaConfig, ILogger<ReservationConsumer> logger, IReservationRepositoryMongo resesrvationRepository, IMapper mapper)
                    : base(kafkaConfig, logger)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig;

            _resesrvationRepositoryMongo = resesrvationRepository;
            _mapper = mapper;

            transformerBlock = new TransformBlock<Reservation, Reservation>(async res =>
            {
                try
                {
                    var reservation = _mapper.Map<ReservationMongoRequest>(res);
                    var result = await _resesrvationRepositoryMongo.GetById(reservation.ReservationId);
                    if (result != null)
                    {
                        if (result.ReservationDate < DateTime.Now)
                        {
                            _logger.LogInformation("DELETE from MongoDB");
                            var temp = await _resesrvationRepositoryMongo.DeleteReservationById(res.ReservationId, res.CustomerId);
                            return _mapper.Map<Reservation>(temp);
                        }
                        else
                        {
                            _logger.LogInformation("UPDATE MongoDB");
                            var temp = await _resesrvationRepositoryMongo.UpdateReservation(res);
                            return _mapper.Map<Reservation>(temp);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("SAVE in Mongo");
                        var temp = await _resesrvationRepositoryMongo.SaveReservation(res);
                        return _mapper.Map<Reservation>(temp);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception/Error in consumer -- > {ex.Message}");
                }
                return null;
            });

            var actionBlock = new ActionBlock<Reservation>(res =>
            {
                transformerBlock.Post(res);
                _logger.LogInformation($"RECEIVED Reservation Consumer ---> {res.ReservationId} ");
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
