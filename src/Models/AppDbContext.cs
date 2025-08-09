using Microsoft.EntityFrameworkCore;

namespace HoneywellFactoryDashboard.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MachineStatus> MachineStatuses { get; set; }
        public DbSet<ProductionReport> ProductionReports { get; set; }
    }
}
