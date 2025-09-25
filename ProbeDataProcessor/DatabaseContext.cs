using Microsoft.EntityFrameworkCore;
using ProbeDataProcessor.Models;

namespace ProbeDataProcessor
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<TemperatureStatistic> TemperatureStatistic { get; set; }
        public DbSet<Probe> Probe { get; set; }
        public DbSet<ProbeData> ProbeData { get; set; }
        public DbSet<GarageDistance> GarageDistance { get; set; }
    }
}
