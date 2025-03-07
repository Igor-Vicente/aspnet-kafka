using FluentValidation;

namespace Kafka.Producer.Api.Models
{
    public class Customer : Entity
    {
        public string Name { get; set; }
        public bool Active { get; set; }

        public Customer(string name)
        {
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new CustomerValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
