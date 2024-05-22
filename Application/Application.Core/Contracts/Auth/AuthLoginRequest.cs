using Application.Common.Extensions;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class AuthLoginRequest
    {
        public string? user_name { get; set; }
        public string? password { get; set; }
    }

    public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
    {
        public AuthLoginRequestValidator()
        {
            RuleFor(_ => _.user_name).NotNullOrEmpty();
            RuleFor(_ => _.password).NotNullOrEmpty();
        }
    }
}
