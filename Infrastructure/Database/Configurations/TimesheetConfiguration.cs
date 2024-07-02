using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using DocumentFormat.OpenXml.Bibliography;

namespace Domain.Configurations
{
    public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
    {
        public void Configure(EntityTypeBuilder<Timesheet> builder)
        {
            builder.ToTable("e_timesheet", "public");

            builder.Property(t => t.id).IsRequired();
            builder.Property(t => t.employee_id).IsRequired();
            builder.Property(t => t.group).IsRequired();
            builder.Property(t => t.month_year).IsRequired();

            builder
                .HasOne(x => x.Employee)
                .WithMany(y => y.Timesheets) 
                .HasPrincipalKey(w => w.id)
                .HasForeignKey(z => z.employee_id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
