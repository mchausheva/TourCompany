using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using TourCompany.BL.Kafka.Serializer;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TourCompany.BL.Kafka
{
    public class CustomerProducer : IProducerService<int, Customer>
    {
        private readonly ILogger<CustomerProducer> _logger;
        private readonly IOptions<KafkaConfig> _kafkaConfig;

        private readonly string _topicName;
        private readonly IProducer<int, Customer> _producer;

        public CustomerProducer(ILogger<CustomerProducer> logger, IOptions<KafkaConfig> kafkaConfig)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig;
            _topicName = typeof(Customer).Name;

            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaConfig.Value.BootstrapServers,
            };

            _producer = new ProducerBuilder<int, Customer>(config)
                .SetKeySerializer(new MsgPackSerializer<int>())
                .SetValueSerializer(new MsgPackSerializer<Customer>())
                .Build();
        }
        public async Task SendMessage(Customer customer, CancellationToken cancellationToken)
        {
            var transformBlock = new TransformBlock<Customer, Message<int, Customer>>(x =>
            {
                var msg = new Message<int, Customer>()
                {
                    Key = customer.CustomerId,
                    Value = customer
                };
                return msg;
            });

            var actionBlock = new ActionBlock<Message<int, Customer>>(async msg =>
            {
                var result = await _producer.ProduceAsync(_topicName, msg, cancellationToken);
                _logger.LogInformation($"Delivered Customer key {msg.Key} -> {msg.Value}");
            });

            transformBlock.LinkTo(actionBlock);

            transformBlock.Post(customer);
        }
    }
}
