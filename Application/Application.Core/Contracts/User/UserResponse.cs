namespace Application.Core.Contracts
{
    public class UserResponse
    {
        public Guid id { get; set; }
        //public string? role_cd { get; set; }
        public string? code { get; set; }
        public string? full_name { get; set; }
        public string? user_name { get; set; }
        public string? birthday { get; set; }
        public string? gender { get; set; }
        public string? org_info_code { get; set; }
        public string? mail { get; set; }
        public string? phone { get; set; }
        public bool? is_actived { get; set; }
        public List<string>? role_cds { get; set; }
        public string? role_names { get; set; }
        public List<string>? permissions { get; set; }
    }
}
