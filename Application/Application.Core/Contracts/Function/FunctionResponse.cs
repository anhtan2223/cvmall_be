namespace Application.Core.Contracts
{
    public class FunctionResponse
    {
        public Guid id { get; set; }
        public string module { get; set; }
        public string code { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? url { get; set; }
        public string? path { get; set; }
        public string? method { get; set; }
        public int? order { get; set; }
        public Guid? parent_id { get; set; }
        public string? parent_cd { get; set; }
        public bool is_active { get; set; }
        public string? icon { get; set; }
        public List<FunctionResponse>? items { get; set; }
    }
}
