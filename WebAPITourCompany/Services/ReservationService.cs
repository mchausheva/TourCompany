using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using WebAPITourCompany.Configurations;
using WebAPITourCompany.Models;
using WebAPITourCompany.Serializer;

namespace WebAPITourCompany.Services
{
    public class ReservationService : IHostedService
    {
        private readonly IOptions<KafkaConfig> _kafkaConfig;
        private readonly IOptions<ReservationSettings> _reservationSettings;
        private readonly IProducer<int, Reservation> _producer;

        public ReservationService(IOptions<KafkaConfig> kafkaConfig, IOptions<ReservationSettings> reservationSettings)
        {
            _kafkaConfig = kafkaConfig;
            _reservationSettings = reservationSettings;

            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaConfig.Value.BootstrapServers,
            };

            _producer = new ProducerBuilder<int, Reservation>(config)
                .SetKeySerializer(new MsgPackSerializer<int>())
                .SetValueSerializer(new MsgPackSerializer<Reservation>())
                .Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var transformBlock = new TransformBlock<Reservation, Message<int, Reservation>>(x =>
            {
                var data = GenerateReservation();

                var msg = new Message<int, Reservation>();
                //{
                //    Key = data.Id,
                //    Value = data
                //};

                return msg;
            });

            var actionBlock = new ActionBlock<Message<int, Reservation>>(msg =>
            {
                var result = _producer.ProduceAsync(_reservationSettings.Value.Name, msg, cancellationToken);
                Console.WriteLine($"Delivered {msg.Key} -> {msg.Value.ReservationId}");
            });

            transformBlock.LinkTo(actionBlock);

            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    transformBlock.Post(new Reservation());
                    await Task.Delay(5000);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        private object GenerateReservation()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
