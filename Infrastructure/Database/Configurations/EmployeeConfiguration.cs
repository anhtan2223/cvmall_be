using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Domain.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("e_employee", "public");

            builder.Property(t => t.employee_code).IsRequired();
            builder.Property(t => t.full_name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.initial_name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.company_email).HasMaxLength(100);
            builder.Property(t => t.personal_email).HasMaxLength(100);
            builder.Property(t => t.phone).HasMaxLength(15);
            builder.Property(t => t.id_number).HasMaxLength(20);

        }
    }
}
