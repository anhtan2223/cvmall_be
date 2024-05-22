using Domain.Abstractions;

namespace Domain.Entities
{
    public partial class Seq : BaseEntity
    {

        public string code { get; set; }

        public int no { get; set; }

        public Seq()
        {
            id = Guid.NewGuid();
        }

    }
}
