namespace Domain.Abstractions
{
    public abstract class BaseOrgEntity : BaseEntity, IOrgUnit
    {
        public Guid org_id { get; set; }
    }
}
