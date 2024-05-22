using Domain.Entities;

namespace Application.Core.Contracts
{
    public class CvTechInfoResponse
    {
        public Guid id { get; set; }

        public CvInfo cvInfo { get; set; }

        public Guid TechnicalId { get; set; }

        public Technical Technical { get; set; }

        public int value { get; set; }

        public DateTime created_at { get; set; }

        public Guid created_by { get; set; }

        public DateTime? updated_at { get; set; }

        public Guid? updated_by { get; set; }
    }
}
