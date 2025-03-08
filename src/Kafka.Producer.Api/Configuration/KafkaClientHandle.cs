using Confluent.Kafka;
using System.Text.Json;

namespace Kafka.Producer.Api.Configuration
{
    public class KafkaClientHandle : IDisposable
    {
        private readonly IProducer<byte[], byte[]> _kafkaProducer;

        public KafkaClientHandle(IConfiguration config)
        {
            var producerConfig = new ProducerConfig();
            config.GetSection("Kafka:ProducerSettings").Bind(producerConfig);
            Console.WriteLine(JsonSerializer.Serialize(producerConfig));
            _kafkaProducer = new ProducerBuilder<byte[], byte[]>(producerConfig).Build();
        }

        public Handle Handle { get => _kafkaProducer.Handle; }

        public void Dispose()
        {
            _kafkaProducer.Flush();
            _kafkaProducer.Dispose();
        }
    }
}
