using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Timesheet : BaseEntity
    {
        public Guid employee_id {get; set;}

        public string group {get; set;}

        public DateTime month_year { get; set; }

        public float? project_participation_hours {get; set;}

        public float? consumed_hours {get; set;}

        public int? late_early_departures {get; set;}

        public float? absence_hours {get; set;}

        public Employee Employee {get; set;}

        public Timesheet()
        {
            id = Guid.NewGuid();
        }
    }
}
