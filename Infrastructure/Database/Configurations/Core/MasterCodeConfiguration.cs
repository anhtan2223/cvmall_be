using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Model.Core.Configurations
{
    public class MasterCodeConfiguration : IEntityTypeConfiguration<MasterCode>
    {
        public void Configure(EntityTypeBuilder<MasterCode> builder)
        {
            builder.ToTable("m_master_code", "public");

            builder.Property(t => t.type).HasMaxLength(100).IsRequired();
            builder.Property(t => t.key).HasMaxLength(200).IsRequired();
        }
    }
}
