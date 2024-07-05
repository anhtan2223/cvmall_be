using Domain.Entities;

namespace Application.Core.Contracts
{
    public class EmployeePositionResponse
    {
        public Guid id {get; set;}
        public Guid position_id { get; set; }
        public PositionResponse position {get; set;}
    }
}
