using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class TechnicalCategoryConfiguration : IEntityTypeConfiguration<TechnicalCategory>
    {
        public void Configure(EntityTypeBuilder<TechnicalCategory> builder)
        {
            builder.ToTable("technical_category", "public");

            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.Property(t => t.IsActived).HasMaxLength(1).HasDefaultValue(false);
        }
    }
}
