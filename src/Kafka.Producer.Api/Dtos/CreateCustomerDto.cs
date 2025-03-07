using System.ComponentModel.DataAnnotations;

namespace Kafka.Producer.Api.Dtos
{
    public struct CreateCustomerDto
    {
        [Required]
        public string Name { get; set; }
    }
}
