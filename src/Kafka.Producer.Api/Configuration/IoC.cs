using Confluent.Kafka;

namespace Kafka.Producer.Api.Configuration
{
    public static class IoC
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<KafkaClientHandle>();
            services.AddSingleton<IKafkaProducer<Null, string>, KafkaDependentProducer<Null, string>>();

            return services;
        }
    }
}
