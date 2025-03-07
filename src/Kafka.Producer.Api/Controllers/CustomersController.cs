using Confluent.Kafka;
using Kafka.Producer.Api.Configuration;
using Kafka.Producer.Api.Dtos;
using Kafka.Producer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kafka.Producer.Api.Controllers
{
    [ApiController]
    [Route("api/v1/customer")]
    public class CustomersController : ControllerBase
    {
        private readonly IKafkaProducer<Null, string> _producer;
        private readonly string _topic = "frivolous_topic";
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IKafkaProducer<Null, string> producer,
                                   IConfiguration configuration,
                                   ILogger<CustomersController> logger)
        {
            _producer = producer;
            _topic = configuration.GetValue<string>("Kafka:FrivolousTopic") ?? throw new NullReferenceException(nameof(_topic));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateCustomerDto model)
        {
            var customer = new Customer(model.Name);
            if (!customer.IsValid()) return BadRequest(customer.ValidationResult);

            var customerJson = JsonSerializer.Serialize(customer);

            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = customerJson });
            return Ok();
        }
    }
}
