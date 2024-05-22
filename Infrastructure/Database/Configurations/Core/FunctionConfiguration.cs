
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Model.Core.Configurations
{
    public class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("a_function", "public");

            builder.Property(t => t.module).HasMaxLength(15);
            builder.Property(t => t.code).HasMaxLength(15).IsRequired();
            builder.Property(t => t.name).HasMaxLength(250);
            builder.Property(t => t.description).HasMaxLength(500);
            builder.Property(t => t.url).HasMaxLength(250);
            builder.Property(t => t.path).HasMaxLength(250);
            builder.Property(t => t.method).HasMaxLength(10);
            builder.Property(t => t.parent_cd).HasMaxLength(15);

            builder
           .HasOne(x => x.parent)
           .WithMany(y => y.childs)
           .HasPrincipalKey(w => w.code)
           .HasForeignKey(z => z.parent_cd)
           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
