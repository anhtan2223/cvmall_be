namespace Domain.Abstractions
{
    public interface IEntity<Guid>
    {
        Guid id { get; set; }
    }
}
