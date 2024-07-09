using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class EmployeePositionConfiguration : IEntityTypeConfiguration<EmployeePosition>
    {
        public void Configure(EntityTypeBuilder<EmployeePosition> builder)
        {
            builder.ToTable("e_employee_positon", "public");
            
            builder.Property(t => t.employee_id).IsRequired();
            builder.Property(t => t.position_id).IsRequired();


            builder
                .HasOne(x => x.Employee)
                .WithMany(y => y.EmployeePositions)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.employee_id)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .HasOne(x => x.Position)
                .WithMany(y => y.EmployeePositions)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.position_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
