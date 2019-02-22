using Microsoft.EntityFrameworkCore;

namespace xUnitMockApi.Models
{
    public class MockContext : DbContext
    {
        // Allow for connection string to be passed in
        public MockContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Wheel> Wheels { get; set; }

        public DbSet<VehicleEngine> VehicleEngines { get; set; }
        public DbSet<VehicleWheel> VehicleWheels { get; set; }

        /// <summary>
        /// Function to control the specifics of DB items and linkages.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add foreign key for many to many relationship
            modelBuilder.Entity<VehicleEngine>().HasKey(x => new { x.VehicleId, x.EngineId });
            modelBuilder.Entity<VehicleEngine>()
                .HasOne(x => x.Vehicle)
                .WithOne(x => x.Engine);

            modelBuilder.Entity<VehicleWheel>().HasKey(x => new { x.VehicleId, x.WheelId });
            modelBuilder.Entity<VehicleWheel>()
                .HasOne(x => x.Vehicle)
                .WithMany(x => x.Wheels)
                .HasForeignKey(x => x.VehicleId);
        }
    }
}
