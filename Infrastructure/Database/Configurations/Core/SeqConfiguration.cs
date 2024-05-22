using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class SeqConfiguration : IEntityTypeConfiguration<Seq>
    {
        public void Configure(EntityTypeBuilder<Seq> builder)
        {
            builder.ToTable("m_seq", "public");

            builder.Property(t => t.code).IsRequired();
            builder.Property(t => t.no).IsRequired();
        }
    }
}
