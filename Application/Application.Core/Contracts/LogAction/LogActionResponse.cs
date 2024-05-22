using Domain.Entities;

namespace Application.Core.Contracts
{
    public class LogActionResponse : LogAction
    {
        public string created_at_format { get; set; }
    }
}
