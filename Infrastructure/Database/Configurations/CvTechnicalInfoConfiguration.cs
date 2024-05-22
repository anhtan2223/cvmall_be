using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class CvTechnicalInfoConfiguration : IEntityTypeConfiguration<CvTechnicalInfo>
    {
        public void Configure(EntityTypeBuilder<CvTechnicalInfo> builder)
        {
            builder.ToTable("cv_technical_info", "public");

            builder.Property(t => t.CvInfoId).IsRequired();
            builder.Property(t => t.TechnicalId).IsRequired();

            builder
                .HasOne(x => x.CvInfo)
                .WithMany(y => y.cvTechInfos)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.CvInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Technical)
                .WithMany(y => y.CvTechicalInfos)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.TechnicalId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
