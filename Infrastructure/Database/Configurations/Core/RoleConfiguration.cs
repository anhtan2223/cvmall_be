using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("a_role", "public");

            builder.Property(t => t.code).HasMaxLength(15).IsRequired();
            builder.Property(t => t.name).HasMaxLength(160).IsRequired();
            builder.Property(t => t.description).HasMaxLength(240);
            builder.Property(t => t.is_actived).HasMaxLength(1).IsRequired();
        }
    }
}
