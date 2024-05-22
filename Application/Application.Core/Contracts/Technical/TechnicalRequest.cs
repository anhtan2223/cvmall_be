using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class TechnicalRequest
    {
        public string Name { get; set; }

        public bool? IsActived { get; set; }

        public Guid TechnicalCategoryId { get; set; }

        public class TechnicalRequestValidator : AbstractValidator<TechnicalRequest>
        {
            public TechnicalRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<Technical>();

                RuleFor(_ => _.Name).NotNullOrEmpty().MaximumLength(50);
                RuleFor(_ => _.TechnicalCategoryId).NotNullOrEmpty();
            }
        }
    }
}
