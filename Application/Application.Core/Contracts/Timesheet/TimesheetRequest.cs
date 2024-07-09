using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class TimesheetRequest
    {
        public Guid id { get; set; }
        
        public Guid employee_id { get; set; }

        public string group { get; set; }

        public DateTime month_year { get; set; }

        public float? project_participation_hours { get; set; }

        public float? consumed_hours { get; set; }

        public int? late_early_departures { get; set; }

        public float? absence_hours { get; set; }

        public class TimesheetRequestValidator : AbstractValidator<TimesheetRequest>
        {
            public TimesheetRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                RuleFor(_ => _.employee_id).NotNullOrEmpty();
                RuleFor(_ => _.group);
                RuleFor(_ => _.month_year).NotNullOrEmpty().IsValidDate(_ls);
                RuleFor(_ => _.project_participation_hours).GreaterThan(0);
                RuleFor(_ => _.consumed_hours).GreaterThan(0);
                RuleFor(_ => _.late_early_departures).GreaterThan(0);
                RuleFor(_ => _.absence_hours).GreaterThan(0);
            }
        }
    }
}
