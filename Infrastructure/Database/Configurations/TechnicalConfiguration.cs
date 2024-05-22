using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class TechnicalConfiguration : IEntityTypeConfiguration<Technical>
    {
        public void Configure(EntityTypeBuilder<Technical> builder)
        {
            builder.ToTable("technical", "public");

            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.Property(t => t.IsActived).HasMaxLength(1).HasDefaultValue(false);

            builder
                .HasOne(x => x.TechnicalCategory)
                .WithMany(y => y.Technicals)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.TechnicalCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
