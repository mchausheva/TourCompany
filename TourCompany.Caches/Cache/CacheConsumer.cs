using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TourCompany.BL.Kafka.Serializer;
using TourCompany.Models.Configurations;

namespace TourCompany.Caches.Cache
{
    public class CacheConsumer<TKey, TValue> 
    {
        private readonly ILogger<CacheConsumer<TKey, TValue>> _logger;
        private IOptions<KafkaConfig> _kafkaConfig;
        private readonly string _topicName;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConsumer<TKey, TValue> _consumerBuilder;

        public CacheConsumer(ILogger<CacheConsumer<TKey, TValue>> logger, IOptions<KafkaConfig> kafkaSettings)
        {
            _logger = logger;
            _kafkaConfig = kafkaSettings;
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

        public IDictionary<TKey, TValue> StartAsync(IDictionary<TKey, TValue> dict, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start {nameof(StartAsync)} --> TValue : {typeof(TValue)}");

            Task.Run( async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumerBuilder.Consume();

                    if (dict.ContainsKey(cr.Key))
                    {
                        dict[cr.Key] = cr.Value;
                    }
                    else
                    {
                        dict.Add(cr.Key, cr.Value);
                        _logger.LogInformation($"Delivered item! {cr.Key} --> {cr.Message.Value}");
                    }
                }
            }, cancellationToken);
            //return Task.CompletedTask;

            return dict;
        }
    }
}
