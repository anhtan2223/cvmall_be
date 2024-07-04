using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class EmployeePositionRequest
    {
        public Guid id { get; set; }
        public Guid employee_id { get; set; }
        public Guid position_id { get; set; }

        public class PositionRequestValidator : AbstractValidator<EmployeePosition>
        {
            public PositionRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<EmployeePosition>();

                RuleFor(_ => _.employee_id).NotNullOrEmpty();
                RuleFor(_ => _.position_id).NotNullOrEmpty();
                
            }
        }
    }
}
