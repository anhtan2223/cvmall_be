using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class EmployeeRequest
    {
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

        public List<EmployeePositionRequest>? EmployeePositions { get; set; }
        public List<EmployeeDepartmentRequest>? EmployeeDepartments { get; set; }
        // public List<Timesheet>? Timesheets {get; set;}


        public class EmployeeRequestValidator : AbstractValidator<Employee>
        {
            public EmployeeRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<Employee>();

                RuleFor(_ => _.employee_code).NotNullOrEmpty();
                RuleFor(_ => _.branch).NotNullOrEmpty();
                RuleFor(_ => _.full_name).NotNullOrEmpty();
                RuleFor(_ => _.initial_name).NotNullOrEmpty();
                RuleFor(_ => _.current_group).NotNullOrEmpty();
                RuleFor(_ => _.birthday).NotNullOrEmpty();
                RuleFor(_ => _.state).NotNullOrEmpty().ExclusiveBetween(0,4);
            }
        }
    }
}
