
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Text.Json;

namespace Kafka.Consumer.Api.Consumers
{
    public class CustomersConsumer : BackgroundService
    {
        private readonly string _topic;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConsumer<Null, string> _kafkaConsumer;
        private readonly ILogger<CustomersConsumer> _logger;
        public CustomersConsumer(IConfiguration configuration,
                                 ILogger<CustomersConsumer> logger)
        {
            _consumerConfig = new ConsumerConfig();
            configuration.GetSection("Kafka:ConsumerSettings").Bind(_consumerConfig);
            _topic = configuration.GetValue<string>("Kafka:FrivolousTopic") ?? throw new NullReferenceException(nameof(_topic));
            _kafkaConsumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build();
            _logger = logger;

            /* REMOVE LATTER*/
            _logger.LogWarning(JsonSerializer.Serialize(_consumerConfig));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private async void StartConsumerLoop(CancellationToken cancellationToken)
        {
            await CreateTopicIfNotExists(_consumerConfig.BootstrapServers, _topic);

            _kafkaConsumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _kafkaConsumer.Consume(cancellationToken);
                    _logger.LogInformation(cr.Message.Value);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        /* TO BE AWARE: 
         * THIS METHOD WILL TRY TO CREATE A TOPIC EVERYTIME THE APPLICATION START/RESTART 
         * AND CREATING A TOPIC SHOULD NOT A TASK FOR AN APPLICATION TO DO
         * */
        private async Task CreateTopicIfNotExists(string bootstrapServers, string topicName)
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
            try
            {
                var topicSpec = new TopicSpecification
                {
                    Name = topicName,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                };
                await adminClient.CreateTopicsAsync(new[] { topicSpec });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
