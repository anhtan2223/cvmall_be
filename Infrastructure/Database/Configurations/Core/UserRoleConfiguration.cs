using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("a_user_role", "public");

            builder.Property(t => t.role_cd).HasMaxLength(15).IsRequired();
            builder.Property(t => t.user_cd).HasMaxLength(15).IsRequired();

            builder
            .HasOne(x => x.user)
            .WithMany(y => y.user_roles)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.user_cd)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.role)
            .WithMany(y => y.user_role)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.role_cd)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
