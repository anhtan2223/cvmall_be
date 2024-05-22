using FluentValidation;
using Application.Common.Extensions;

namespace Application.Core.Contracts
{
    public class RoleRequest
    {
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string>? permissions { get; set; }
    }

    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            // TODO : check code duplicate
            RuleFor(_ => _.code).NotNullOrEmpty().MaximumLength(50);
            RuleFor(_ => _.name).NotNullOrEmpty().MaximumLength(250);
            RuleFor(_ => _.description).MaximumLength(500);
        }
    }
}
