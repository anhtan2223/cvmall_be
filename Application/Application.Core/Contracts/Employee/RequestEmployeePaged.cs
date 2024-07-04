using Framework.Core.Abstractions;

namespace Application.Core.Contracts
{
    public class RequestEmployeePaged : IRequestPaged
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? search { get; set; }

        public List<string>? group { get; set; }
        public List<string>? branch { get; set; }
        public List<string>? department { get; set; }
        public List<string>? position { get; set; }
        public List<int>? state { get; set; }


    }
}
