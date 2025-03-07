using Confluent.Kafka;
using FluentAssertions;
using Kafka.Producer.Api.Configuration;
using Kafka.Producer.Api.Controllers;
using Kafka.Producer.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kafka.Producer.Api.Tests.Controllers
{
    public class CustomersControllerTests
    {
        private readonly CustomersController _controller;
        private readonly Mock<IKafkaProducer<Null, string>> _producerMock;
        private readonly Mock<ILogger<CustomersController>> _loggerMock;

        public CustomersControllerTests()
        {
            var configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<CustomersController>>();
            _producerMock = new Mock<IKafkaProducer<Null, string>>();

            var mockIConfigurationSection = new Mock<IConfigurationSection>();
            mockIConfigurationSection.Setup(x => x.Key).Returns("Kafka:FrivolousTopic");
            mockIConfigurationSection.Setup(x => x.Value).Returns("frivolous_topic_test");
            configurationMock.Setup(x => x.GetSection("Kafka:FrivolousTopic")).Returns(mockIConfigurationSection.Object);

            _controller = new CustomersController(_producerMock.Object, configurationMock.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = "Create Valid Customer")]
        public async Task CustomersController_CreateCustomerAsync_ShouldReturnOk()
        {
            // Arrange
            var model = new CreateCustomerDto() { Name = "Olivia" };

            _producerMock
                .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateCustomerAsync(model);

            // Assert
            result.Should().BeOfType<OkResult>();
            _producerMock.Verify(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>()), Times.Once);
        }

        [Fact(DisplayName = "Create Invalid Customer")]
        public async Task CustomersController_CreateCustomerAsync_ShouldReturnBadRequest()
        {
            // Arrange
            var model = new CreateCustomerDto() { Name = "" };

            _producerMock
                .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateCustomerAsync(model);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
