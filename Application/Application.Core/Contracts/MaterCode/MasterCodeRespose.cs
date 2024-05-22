namespace Application.Core.Contracts
{
    public class MasterCodeRespose
    {
        public Guid id { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public DateTime created_at { get; set; }
    }
}
