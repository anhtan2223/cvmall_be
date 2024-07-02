using System.Security.AccessControl;
using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Employee : BaseEntity
    {
        public Employee()
        {
            id = Guid.NewGuid();
        }

        public string employee_code { get; set; }

        public string branch { get; set; }

        public string full_name { get; set; }

        public string initial_name { get; set; }

        public string current_group { get; set; }

        public int state {get; set;}

        public string? company_email {get; set;}
        
        public string? personal_email {get; set;}

        public string? phone {get; set;}

        public DateTime birthday { get; set; }

        public string? permanent_address {get; set;}

        public string? current_address {get; set;}

        public string? id_number {get; set;}

        public DateTime? date_issue {get; set;}

        public string? location_issue {get; set;}

        public bool is_married {get; set;}

        public virtual ICollection<Timesheet>? Timesheets { get; set; }
        public virtual ICollection<EmployeeDepartment>? EmployeeDepartments { get; set; }
        public virtual ICollection<EmployeePosition>? EmployeePositions { get; set; }
        

    }
}
