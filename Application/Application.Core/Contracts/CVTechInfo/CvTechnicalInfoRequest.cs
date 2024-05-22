using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class CvTechnicalInfoRequest
    {
        public Guid CvInfoId { get; set; }

        public Guid TechnicalId { get; set; }

        public int Value { get; set; }

        public class CvTechnicalInfoRequestValidator : AbstractValidator<CvTechnicalInfoRequest>
        {
            public CvTechnicalInfoRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<CvTechnicalInfo>();

                RuleFor(_ => _.CvInfoId).NotNullOrEmpty();
                RuleFor(_ => _.TechnicalId).NotNullOrEmpty();
                RuleFor(_ => _.Value).NotNullOrEmpty();
            }
        }
    }
}
