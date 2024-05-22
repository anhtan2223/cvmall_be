using Domain.Abstractions;

namespace Domain.Entities
{
    public class LogAction : BaseEntity
    {
        public Guid? user_id { get; set; }
        public string? user_name { get; set; }
        public string method { get; set; }
        public string body { get; set; }
        public string description { get; set; }
        public User user { get; set; }
    }
}
