using Confluent.Kafka;

namespace Kafka.Producer.Api.Configuration
{
    public class KafkaDependentProducer<K, V>
    {
        private readonly IProducer<K, V> _kafkaHandle;

        public KafkaDependentProducer(KafkaClientHandle kafkaHandle)
        {
            _kafkaHandle = new DependentProducerBuilder<K, V>(kafkaHandle.Handle).Build();
        }

        public Task ProduceAsync(string topic, Message<K, V> message)
            => _kafkaHandle.ProduceAsync(topic, message);

        public void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>> deliveryHandler = null)
            => _kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => _kafkaHandle.Flush(timeout);
    }
}
