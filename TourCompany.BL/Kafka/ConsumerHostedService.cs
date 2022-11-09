using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TourCompany.BL.Kafka.Serializer;
using TourCompany.Models.Configurations;

namespace TourCompany.BL.Kafka
{
    public abstract class ConsumerHostedService<TKey, TValue> : IHostedService
    {
        private readonly ILogger<ConsumerHostedService<TKey, TValue>> _logger;
        private readonly IOptions<KafkaConfig> _kafkaConfig;
        private readonly string _topicName;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConsumer<TKey, TValue> _consumerBuilder;

        public ConsumerHostedService(IOptions<KafkaConfig> options, ILogger<ConsumerHostedService<TKey, TValue>> logger)
        {
            _kafkaConfig = options;
            _logger = logger;
            _topicName = typeof(TValue).Name;

            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = _kafkaConfig.Value.BootstrapServers,
                AutoOffsetReset = (AutoOffsetReset?)_kafkaConfig.Value.AutoOffsetReset,
                GroupId = _kafkaConfig.Value.GroupId
            };
            _consumerBuilder = new ConsumerBuilder<TKey, TValue>(_consumerConfig)
                .SetKeyDeserializer(new MsgPackDeserializer<TKey>())
                .SetValueDeserializer(new MsgPackDeserializer<TValue>())
                .Build();
            _consumerBuilder.Subscribe(_topicName);
        }

        public abstract void HandleMessage(TValue value);


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start -> {nameof(ConsumerHostedService<TKey, TValue>)}");

            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumerBuilder.Consume();

                        if (cr != null)
                        {
                            HandleMessage(cr.Message.Value);
                            _logger.LogWarning($"RECEIVED ---> {cr.Message.Value} ");
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Error occured: {ex.Error.Reason}");
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumerBuilder.Dispose();

            return Task.CompletedTask;
        }
    }
}
