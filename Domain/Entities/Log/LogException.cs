using Domain.Abstractions;

namespace Domain.Entities
{
    public class LogException : BaseEntity
    {
        public string? function { get; set; }

        public string? message { get; set; }

        public string? stack_trace { get; set; }

        public LogException()
        {
            id = Guid.NewGuid();
        }
    }
}
