using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class CvTechnicalInfo : BaseOrgEntity
    {
        public Guid CvInfoId { get; set; }

        public CvInfo CvInfo { get; set; }

        public Guid TechnicalId { get; set; }

        public Technical Technical { get; set; }

        public int Value { get; set; }

        public CvTechnicalInfo()
        {
            id = Guid.NewGuid();
        }
    }
}
