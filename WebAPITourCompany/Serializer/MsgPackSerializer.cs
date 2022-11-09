using Confluent.Kafka;
using MessagePack;

namespace WebAPITourCompany.Serializer
{ 
    public class MsgPackSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return MessagePackSerializer.Serialize(data);
        }
    }
}