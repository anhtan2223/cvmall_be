using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Function : BaseEntity
    {
        public string? module { get; set; }

        public string? code { get; set; }

        public string name { get; set; }

        public string? description { get; set; }

        public string? url { get; set; }

        public string? path { get; set; }

        public string? method { get; set; }

        public string? parent_cd { get; set; }

        public int? order { get; set; }

        public bool is_active { get; set; }

        public string? icon { get; set; }

        public Function? parent { get; set; }

        public virtual ICollection<Function>? childs { get; set; }

        public virtual ICollection<Permission>? permissions { get; set; }


        public Function()
        {
            id = Guid.NewGuid();
        }
    }
}
