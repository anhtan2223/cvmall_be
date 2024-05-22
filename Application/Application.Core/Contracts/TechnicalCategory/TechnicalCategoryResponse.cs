using Domain.Entities;

namespace Application.Core.Contracts
{
    public class TechnicalCategoryResponse
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public bool? IsActived { get; set; }

        public ICollection<TechnicalResponse>? Technicals { get; set; }

    }
}
