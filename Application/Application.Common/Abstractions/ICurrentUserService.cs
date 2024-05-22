namespace Application.Common.Abstractions
{
    public interface ICurrentUserService
    {
        Guid? user_id { get; set; }
        string? user_name { get; set; }
        string? full_name { get; set; }
    }
}
