using FluentValidation;
using Application.Common.Abstractions;
using Domain.Entities;
using Application.Common.Extensions;

namespace Application.Core.Contracts 
{
    public class UserRequest
    {
        public string code { get; set; }
        public string full_name { get; set; }
        public string user_name { get; set; }
        public string gender { get; set; }
        public string? birthday { get; set; }
        public string org_info_code { get; set; }
        public string mail { get; set; }
        public string? phone { get; set; }
        public bool? is_actived { get; set; }
        public List<string>? role_cds { get; set; }

    }

    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
        {
            var userRepo = _unitOfWork.GetRepository<User>();

            RuleFor(_ => _.code).NotNullOrEmpty().MaximumLength(10).MasterMustNotExist(userRepo, _ls, "code");
            RuleFor(_ => _.full_name).NotNullOrEmpty().MaximumLength(100);
            RuleFor(_ => _.user_name).NotNullOrEmpty().MaximumLength(50);
            RuleFor(_ => _.mail).NotNullOrEmpty().EmailAddress().MaximumLength(50);
            RuleFor(_ => _.phone).NotNullOrEmpty().MaximumLength(15);
            RuleFor(_ => _.gender).NotNullOrEmpty().MaximumLength(1);
        }
    }
}
