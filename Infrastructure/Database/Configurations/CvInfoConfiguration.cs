using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class CvInfoConfiguration : IEntityTypeConfiguration<CvInfo>
    {
        public void Configure(EntityTypeBuilder<CvInfo> builder)
        {
            builder.ToTable("cv_info", "public");

            builder.Property(t => t.furigana).HasMaxLength(50).IsRequired();
            builder.Property(t => t.is_actived).HasMaxLength(1).HasDefaultValue(false);
            builder.Property(t => t.name).HasMaxLength(50).IsRequired();
            builder.Property(t => t.gender).HasMaxLength(1).HasDefaultValue("0");
            builder.Property(t => t.last_university_name).HasMaxLength(250);
            builder.Property(t => t.subject).HasMaxLength(50);
            builder.Property(t => t.certificate1_name).HasMaxLength(250);
            builder.Property(t => t.certificate2_name).HasMaxLength(250);
            builder.Property(t => t.certificate3_name).HasMaxLength(250);
            builder.Property(t => t.certificate4_name).HasMaxLength(250);
            builder.Property(t => t.work_process).HasMaxLength(500);
            builder.Property(t => t.note).HasMaxLength(500);

        }
    }
}
