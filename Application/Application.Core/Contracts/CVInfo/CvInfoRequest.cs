using Application.Common.Abstractions;
using Application.Common.Extensions;
using Domain.Entities;
using FluentValidation;

namespace Application.Core.Contracts
{
    public class CvInfoRequest
    {
        public string furigana { get; set; }

        public bool? is_actived { get; set; }

        public string user_code { get; set; }

        public string branch { get; set; }

        public string name { get; set; }

        public string gender { get; set; }

        public DateTime birthday { get; set; }

        public string? last_university_name { get; set; }

        public string? subject { get; set; }

        public int? graduation_year { get; set; }

        public int? lang1_hearing { get; set; }

        public int? lang1_speaking { get; set; }

        public int? lang1_reading { get; set; }

        public int? lang1_writing { get; set; }

        public int? lang2_hearing { get; set; }

        public int? lang2_speaking { get; set; }

        public int? lang2_reading { get; set; }

        public int? lang2_writing { get; set; }

        public string? certificate1_name { get; set; }

        public int? certificate1_year { get; set; }

        public string? certificate2_name { get; set; }

        public int? certificate2_year { get; set; }

        public string? certificate3_name { get; set; }

        public int? certificate3_year { get; set; }

        public string? certificate4_name { get; set; }

        public int? certificate4_year { get; set; }

        public string? work_process { get; set; }

        public string? note { get; set; }

        public class CvInfoRequestValidator : AbstractValidator<CvInfoRequest>
        {
            public CvInfoRequestValidator(IUnitOfWork _unitOfWork, ILocalizeServices _ls)
            {
                var userRepo = _unitOfWork.GetRepository<CvInfo>();

                RuleFor(_ => _.furigana).MaximumLength(50);
                RuleFor(_ => _.name).NotNullOrEmpty().MaximumLength(50);
                RuleFor(_ => _.gender).NotNullOrEmpty().MaximumLength(1);
                RuleFor(_ => _.birthday).IsValidDateTime(_ls);
            }
        }
    }
}
