using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class BizInfoConfiguration : IEntityTypeConfiguration<BizInfo>
    {
        public void Configure(EntityTypeBuilder<BizInfo> builder)
        {
            builder.ToTable("biz_info", "public");

            builder.Property(t => t.prj_name).HasMaxLength(250).IsRequired();
            builder.Property(t => t.prj_content).HasMaxLength(500);
            builder.Property(t => t.period).IsRequired();
            builder.Property(t => t.system_analysis).HasDefaultValue(false);
            builder.Property(t => t.overview_design).HasDefaultValue(false);
            builder.Property(t => t.basic_design).HasDefaultValue(false);
            builder.Property(t => t.functional_design).HasDefaultValue(false);
            builder.Property(t => t.detailed_design).HasDefaultValue(false);
            builder.Property(t => t.coding).HasDefaultValue(false);
            builder.Property(t => t.unit_test).HasDefaultValue(false);
            builder.Property(t => t.operation).HasDefaultValue(false);
            builder.Property(t => t.os_db).HasMaxLength(50);
            builder.Property(t => t.language).HasMaxLength(50);
            builder.Property(t => t.role).HasMaxLength(50);

            builder
                .HasOne(x => x.cvInfo)
                .WithMany(y => y.bizInfos)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.cvInfoId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
