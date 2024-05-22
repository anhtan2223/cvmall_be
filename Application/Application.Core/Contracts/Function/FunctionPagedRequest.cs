using Framework.Core.Abstractions;

namespace Application.Core.Contracts
{
    public class FunctionPagedRequest : IRequestPaged
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? module { get; set; }

        public string? parent { get; set; }

        public string? code { get; set; }

        public string? name { get; set; }
    }
}
