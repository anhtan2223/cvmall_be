using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Position : BaseEntity
    {
        public Position()
        {
            id = Guid.NewGuid();
        }

        public string name { get; set; }

        public virtual ICollection<EmployeePosition>? EmployeePositions { get; set; }
        
    }
}
