using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class EmployeeDepartmentRequest
    {
        public Guid id { get; set; }
        public Guid employee_id { get; set; }
        public Guid department_id { get; set; }

        public class PositionRequestValidator : AbstractValidator<EmployeeDepartment>
        {
            public PositionRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<EmployeeDepartment>();

                RuleFor(_ => _.employee_id).NotNullOrEmpty();
                RuleFor(_ => _.department_id).NotNullOrEmpty();
            }
        }
    }
}
