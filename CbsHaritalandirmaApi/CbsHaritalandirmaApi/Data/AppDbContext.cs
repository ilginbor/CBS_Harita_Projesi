using CbsHaritalandirmaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CbsHaritalandirmaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<MapLocation> MapLocations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DrawPoint> DrawPoints { get; set; }

        public DbSet<DrawLine> DrawLines { get; set; }

        public DbSet<DrawPolygon> DrawPolygons { get; set; }
    }
}