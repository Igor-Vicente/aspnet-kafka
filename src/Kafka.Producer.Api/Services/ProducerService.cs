namespace Kafka.Producer.Api.Services
{
    //public class ProducerService
    //{
    //    private readonly ILogger<ProducerService> _logger;

    //    public ProducerService(ILogger<ProducerService> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public async Task<string> SendMessage(string message)
    //    {
    //        var topic = _kafkaSettings.Topic;
    //        using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
    //        {
    //            var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    //            var response = $"Status: {result.Status.ToString()} || Message sent: {message}";
    //            _logger.LogInformation(response);
    //            return response;
    //        }
    //    }
    //}
}
