using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class LogExceptionConfiguration : IEntityTypeConfiguration<LogException>
    {
        public void Configure(EntityTypeBuilder<LogException> builder)
        {
            builder.ToTable("log_exception", "public");

            builder.Property(t => t.function).HasMaxLength(250);
        }
    }
}
