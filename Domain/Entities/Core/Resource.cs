using Domain.Abstractions;

namespace Domain.Entities
{
    public class Resource : BaseEntity
    {
        public Guid id { get; set; }

        public string lang { get; set; }

        public string module { get; set; }

        public string screen { get; set; }

        public string key { get; set; }

        public string text { get; set; }
    }
}
