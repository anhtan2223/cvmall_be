using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class TechnicalCategoryRequest
    {
        public string Name { get; set; }

        public bool? IsActived { get; set; }

        public class TechnicalCategoryRequestValidator : AbstractValidator<TechnicalCategoryRequest>
        {
            public TechnicalCategoryRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<User>();

                RuleFor(_ => _.Name).NotNullOrEmpty().MaximumLength(50);
            }
        }
    }
}
