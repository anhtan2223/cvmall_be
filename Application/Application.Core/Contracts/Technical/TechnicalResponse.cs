namespace Application.Core.Contracts
{
    public class TechnicalResponse
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public bool? IsActived { get; set; }

        public Guid TechnicalCategoryId { get; set; }

        public TechnicalCategoryResponse TechnicalCategory { get; set; }

        public DateTime created_at { get; set; }

        public Guid created_by { get; set; }

        public DateTime? updated_at { get; set; }

        public Guid? updated_by { get; set; }
    }
}
