using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("m_resource", "public");

            builder.Property(t => t.lang).HasMaxLength(10);
            builder.Property(t => t.module).HasMaxLength(100);
            builder.Property(t => t.screen).HasMaxLength(100);
            builder.Property(t => t.key).HasMaxLength(200).IsRequired();
        }
    }
}
