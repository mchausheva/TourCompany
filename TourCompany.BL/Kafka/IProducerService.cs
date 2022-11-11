using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using TourCompany.BL.Kafka.Serializer;
using TourCompany.Models.Configurations;
using TourCompany.Models.Models;

namespace TourCompany.BL.Kafka
{
    public interface IProducerService<TKey, TValue>
    {
        public Task SendMessage(TValue value, CancellationToken cancellationToken);
    }
}
