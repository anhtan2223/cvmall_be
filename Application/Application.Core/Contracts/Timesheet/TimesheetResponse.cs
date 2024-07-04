using Domain.Entities;

namespace Application.Core.Contracts
{
    public class TimesheetResponse
    {
        public Guid id { get; set; }
        
        public Guid employee_id {get; set;}

        public string group {get; set;}

        public DateTime month_year { get; set; }

        public float? project_participation_hours {get; set;}

        public float? consumed_hours {get; set;}

        public int? late_early_departures {get; set;}

        public float? absence_hours {get; set;}

        public Employee Employee {get; set;}
    }

}
