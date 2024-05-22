using FluentValidation;
using Application.Common.Extensions;
namespace Application.Core.Contracts
{
    public class MasterCodeRequest
    {
        public string type { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
    public class MasterCodeRequestValidator : AbstractValidator<MasterCodeRequest>
    {
        public MasterCodeRequestValidator()
        {
            // TODO : check code duplicate
            RuleFor(_ => _.type).NotNullOrEmpty().MaximumLength(100);
            RuleFor(_ => _.key).NotNullOrEmpty().MaximumLength(200);
        }
    }
}
