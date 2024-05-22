using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class TechnicalCategory : BaseOrgEntity
    {
        public TechnicalCategory()
        {
            Technicals = new HashSet<Technical>();
            id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public bool? IsActived { get; set; }

        public virtual ICollection<Technical>? Technicals { get; set; }

    }
}
