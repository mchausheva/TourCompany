using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks.Dataflow;
using TourCompany.BL.Kafka.Serializer;
using TourCompany.DL.Interfaces;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;

namespace TourCompany.BL.Kafka
{
    public class ReservationProducer
    {
        private readonly ILogger<ReservationProducer> _logger;
        private readonly IOptions<KafkaConfig> _kafkaConfig;

        private readonly string _topicName;
        private readonly IProducer<int, Reservation> _producer;

        public ReservationProducer(ILogger<ReservationProducer> logger, IOptions<KafkaConfig> kafkaConfig)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig;
            _topicName = typeof(Reservation).Name;

            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaConfig.Value.BootstrapServers,
            };

            _producer = new ProducerBuilder<int, Reservation>(config)
                .SetKeySerializer(new MsgPackSerializer<int>())
                .SetValueSerializer(new MsgPackSerializer<Reservation>())
                .Build();
        }

        public async Task SendMessage(Reservation reservation, CancellationToken cancellationToken)
        {
            var transformBlock = new TransformBlock<Reservation, Message<int, Reservation>>(x =>
            {
                var msg = new Message<int, Reservation>()
                {
                    Key = reservation.ReservationId,
                    Value = reservation
                };

                return msg;
            });

            var actionBlock = new ActionBlock<Message<int, Reservation>>(async msg =>
            {
                var result = await _producer.ProduceAsync(_topicName, msg, cancellationToken);
                _logger.LogInformation($"Delivered Reservation Key {msg.Key} -> {msg.Value}");
            });

            transformBlock.LinkTo(actionBlock);

            transformBlock.Post(reservation);
        }
    }
}
