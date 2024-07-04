using Domain.Entities;

namespace Application.Core.Contracts
{
    public class EmployeeDepartmentResponse
    {
        public Guid id {get; set;}
        public Guid department_id { get; set; }
        public Domain.Entities.Department Department {get; set;}

    }
}
