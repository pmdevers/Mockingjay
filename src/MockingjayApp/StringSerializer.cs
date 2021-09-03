using Confluent.Kafka;
using System;
using System.Text;

namespace MockingjayApp
{
    public class StringSerializer : ISerializer<string>, ISerializer<Ignore>
    {
        public byte[] Serialize(string data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public byte[] Serialize(Ignore data, SerializationContext context)
        {
            return Array.Empty<byte>();
        }
    }
}
