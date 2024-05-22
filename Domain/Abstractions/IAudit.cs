namespace Domain.Abstractions
{
    public interface IAudit
    {
        DateTime created_at { get; set; }
        Guid created_by { get; set; }
        DateTime? updated_at { get; set; }
        Guid? updated_by { get; set; }
    }
}
