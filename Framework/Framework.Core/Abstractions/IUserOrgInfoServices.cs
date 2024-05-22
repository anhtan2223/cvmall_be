namespace Framework.Core.Abstractions
{
    public interface IUserOrgInfoServices
    {
        Guid? _orgId { get; set; }
        Guid? _groupId { get; set; }
        int? _accYear { get; set; }
    }
}