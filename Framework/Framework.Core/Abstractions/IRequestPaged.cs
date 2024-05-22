namespace Framework.Core.Abstractions
{
    public interface IRequestPaged
    {
        int page { get; }
        int size { get; }
        string sort { get; }
    }
}
