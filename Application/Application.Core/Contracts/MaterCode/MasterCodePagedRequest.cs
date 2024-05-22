
namespace Application.Core.Contracts
{
    public class MasterCodePagedRequest
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? type { get; set; }
    }
}
