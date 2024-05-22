using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Role>? Roles { get; set; }
        public virtual DbSet<Permission>? Permissions { get; set; }
        public virtual DbSet<Function>? Functions { get; set; }

        public virtual DbSet<MasterCode>? MasterCodes { get; set; }
        public virtual DbSet<Seq>? Seqs { get; set; }
        public virtual DbSet<Resource>? Resources { get; set; }

        public virtual DbSet<LogAction>? LogAction { get; set; }
        public virtual DbSet<LogException>? LogException { get; set; }

        public virtual DbSet<TechnicalCategory>? TechnicalCategories { get; set; }
        public virtual DbSet<Technical>? Technicals { get; set; }
        public virtual DbSet<CvInfo>? CvInfos { get; set; }
        public virtual DbSet<CvTechnicalInfo>? CvTechnicalInfos { get; set; }
        public virtual DbSet<BizInfo>? BizInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

    }
}
