using Domain.Entities;

namespace Application.Core.Contracts
{
    public class PositionResponse
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}
