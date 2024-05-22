using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class LogActionConfiguration : IEntityTypeConfiguration<LogAction>
    {
        public void Configure(EntityTypeBuilder<LogAction> builder)
        {
            builder.ToTable("m_log_action");

            builder.Property(t => t.method).HasMaxLength(20);
            //builder.Property(t => t.body).HasMaxLength(300);
            //builder.Property(t => t.description).HasMaxLength(300);

            builder
               .HasOne(x => x.user)
               .WithMany(y => y.log_actions)
               .HasPrincipalKey(w => w.id)
               .HasForeignKey(z => z.user_id)
               .OnDelete(DeleteBehavior.Restrict)
               ;
        }
    }
}
