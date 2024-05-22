using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class BizInfo : BaseOrgEntity
    {
        public string prj_name { get; set; }

        public string prj_content { get; set; }

        public int period { get; set; }

        public bool system_analysis { get; set; }

        public bool overview_design { get; set; }

        public bool basic_design { get; set; }

        public bool functional_design { get; set; }

        public bool detailed_design { get; set; }

        public bool coding { get; set; }

        public bool unit_test { get; set; }

        public bool operation { get; set; }

        public string os_db { get; set; }

        public string language { get; set; }

        public string role { get; set; }

        public Guid cvInfoId { get; set; }

        public CvInfo cvInfo { get; set; }

        public BizInfo()
        {
            id = Guid.NewGuid();
        }
    }
}
