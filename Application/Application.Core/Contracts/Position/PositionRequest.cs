using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class PositionRequest
    {
        public string name { get; set; }

        public class PositionRequestValidator : AbstractValidator<Position>
        {
            public PositionRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<Position>();

                RuleFor(_ => _.name).NotNullOrEmpty();
                
            }
        }
    }
}
