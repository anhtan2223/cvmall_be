using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Department : BaseEntity
    {
        public Department()
        {
            id = Guid.NewGuid();
        }

        public string name { get; set; }

        public virtual ICollection<EmployeeDepartment>? EmployeeDepartments { get; set; }

    }
}
