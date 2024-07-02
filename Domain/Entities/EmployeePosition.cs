using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class EmployeePosition : BaseEntity
    {
        public EmployeePosition()
        {
            id = Guid.NewGuid();
        }

        public Guid employee_id { get; set; }
        public Guid position_id { get; set; }

        public Employee Employee {get; set;}
        public Position Position {get; set;}


    }
}
