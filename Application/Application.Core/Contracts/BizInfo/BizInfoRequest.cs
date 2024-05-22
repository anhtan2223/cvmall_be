using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class BizInfoRequest
    {
        public string prj_name { get; set; }

        public string prj_content { get; set; }

        public int period { get; set; }

        public bool? system_analysis { get; set; } = false;

        public bool? overview_design { get; set; } = false;

        public bool? basic_design { get; set; } = false;

        public bool? functional_design { get; set; } = false;

        public bool? detailed_design { get; set; } = false;

        public bool? coding { get; set; } = false;

        public bool? unit_test { get; set; } = false;

        public bool? operation { get; set; } = false;

        public string os_db { get; set; }

        public string language { get; set; }

        public string role { get; set; }

        public Guid cvInfoId { get; set; }

        public class BizInfoRequestValidator : AbstractValidator<BizInfoRequest>
        {
            public BizInfoRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<BizInfo>();

                RuleFor(_ => _.prj_name).NotNullOrEmpty().MaximumLength(250);
                RuleFor(_ => _.prj_content).NotNullOrEmpty().MaximumLength(500);
                RuleFor(_ => _.period).NotNullOrEmpty();
                RuleFor(_ => _.os_db).MaximumLength(50);
                RuleFor(_ => _.language).MaximumLength(50);
                RuleFor(_ => _.role).MaximumLength(50);
            }
        }
    }
}
