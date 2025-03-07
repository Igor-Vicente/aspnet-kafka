using FluentValidation.Results;

namespace Kafka.Producer.Api.Models
{
    public abstract class Entity
    {
        public Guid Id { get; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public abstract bool IsValid();
    }
}
