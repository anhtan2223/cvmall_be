using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Model.Core.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("a_user_token", "public");

            builder.Property(t => t.user_cd).HasMaxLength(15).IsRequired();
            builder.Property(t => t.access_token).HasMaxLength(500).IsRequired();
            builder.Property(t => t.refresh_token).HasMaxLength(500);
            builder.Property(t => t.access_token_expired_date).HasColumnType(DataType.Date.ToString()).IsRequired();
            builder.Property(t => t.refresh_token_expired_date).HasColumnType(DataType.Date.ToString());
            builder.Property(t => t.ip).HasMaxLength(20).IsRequired();

            builder
            .HasOne(x => x.user)
            .WithMany(x => x.user_token)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.user_cd)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
