using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class CvInfo : BaseOrgEntity
    {
        public CvInfo()
        {
            cvTechInfos = new HashSet<CvTechnicalInfo>();
            bizInfos = new HashSet<BizInfo>();
            id = Guid.NewGuid();
        }

        public string user_code { get; set; }

        public string branch { get; set; }

        public string furigana { get; set; }

        public bool? is_actived { get; set; }

        public string name { get; set; }

        public int gender { get; set; }

        public DateTime birthday { get; set; }

        public string? last_university_name { get; set; }
        public string? last_university_name_jp { get; set; }

        public string? subject { get; set; }
        public string? subject_jp { get; set; }

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
        public string? certificate1_name_jp { get; set; }

        public int? certificate1_year { get; set; }

        public string? certificate2_name { get; set; }
        public string? certificate2_name_jp { get; set; }

        public int? certificate2_year { get; set; }

        public string? certificate3_name { get; set; }
        public string? certificate3_name_jp { get; set; }

        public int? certificate3_year { get; set; }

        public string? certificate4_name { get; set; }

        public int? certificate4_year { get; set; }

        public string? work_process { get; set; }
        public string? work_process_jp { get; set; }

        public string? note { get; set; }
        public string? note_jp { get; set; }

        public virtual ICollection<CvTechnicalInfo>? cvTechInfos { get; set; }
        
        public virtual ICollection<BizInfo>? bizInfos { get; set; }    
    }
}
