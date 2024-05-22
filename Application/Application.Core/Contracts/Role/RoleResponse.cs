namespace Application.Core.Contracts
{
    public class RoleResponse
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string? description { get; set; }

        public IList<string>? permissions { get; set; }
    }
}
