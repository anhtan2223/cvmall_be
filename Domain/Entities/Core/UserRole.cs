using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class UserRole : BaseEntity
    {
        public string user_cd { get; set; }

        public User user { get; set; }

        public string role_cd { get; set; }

        public Role role { get; set; }

        public UserRole()
        {
            id = Guid.NewGuid();
        }

    }
}
