using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class EmployeeDepartment : BaseEntity
    {
        public EmployeeDepartment()
        {
            id = Guid.NewGuid();
        }

        public Guid employee_id { get; set; }
        public Guid department_id { get; set; }

        public Employee Employee {get; set;}
        public Department Department {get; set;}

    }
}
