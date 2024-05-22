using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("a_user", "public");

            builder.Property(t => t.code).HasMaxLength(15).IsRequired();
            builder.Property(t => t.user_name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.full_name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.gender).HasMaxLength(1).IsRequired();
            builder.Property(t => t.org_info_code).HasMaxLength(20).IsRequired();
            builder.Property(t => t.hashpass).HasMaxLength(100);
            builder.Property(t => t.salt).HasMaxLength(100);
            builder.Property(t => t.mail).HasMaxLength(100).IsRequired();
            builder.Property(t => t.phone).HasMaxLength(15);

        }
    }
}
