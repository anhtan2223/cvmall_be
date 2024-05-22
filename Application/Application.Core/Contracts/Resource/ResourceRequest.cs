using Application.Common.Extensions;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class ResourceRequest
    {
        public string lang { get; set; }
        public string module { get; set; }
        public string screen { get; set; }
        public string key { get; set; }
        public string text { get; set; }
    }

    public class ResourceRequestValidator : AbstractValidator<ResourceRequest>
    {
        public ResourceRequestValidator()
        {
            RuleFor(_ => _.lang).MaximumLength(10);
            RuleFor(_ => _.module).MaximumLength(100);
            RuleFor(_ => _.screen).MaximumLength(100);
            RuleFor(_ => _.key).NotNullOrEmpty().MaximumLength(200);
        }
    }
}
