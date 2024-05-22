using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class UserToken : BaseEntity
    {
        public string user_cd { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public DateTime access_token_expired_date { get; set; }

        public DateTime refresh_token_expired_date { get; set; }

        public string ip { get; set; }

        public User? user { get; set; }

        public UserToken()
        {
            id = Guid.NewGuid();
        }
    }
}
