using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class MasterCode : BaseEntity
    {

        public string type { get; set; }

        public string key { get; set; }

        public string value { get; set; }

        public MasterCode()
        {
            id = Guid.NewGuid();
        }
    }
}