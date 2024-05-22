using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Technical : BaseOrgEntity
    {
        public string Name { get; set; }

        public bool? IsActived { get; set; }

        public Guid TechnicalCategoryId { get; set; }

        public TechnicalCategory TechnicalCategory { get; set; }

        public virtual ICollection<CvTechnicalInfo>? CvTechicalInfos { get; set; }

        public Technical()
        {
            id = Guid.NewGuid();
        }
    }
}
