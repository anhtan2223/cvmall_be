using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
    {
        public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
        {
            builder.ToTable("e_employee_department", "public");

            builder.Property(t => t.employee_id).IsRequired();
            builder.Property(t => t.department_id).IsRequired();

            builder
                .HasOne(x => x.Employee)
                .WithMany(y => y.EmployeeDepartments)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.employee_id)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .HasOne(x => x.Department)
                .WithMany(y => y.EmployeeDepartments)
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.department_id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
