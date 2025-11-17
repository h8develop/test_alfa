using Microsoft.EntityFrameworkCore;
using RobotStats.Models;

namespace RobotStats.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RobotRun> RobotRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RobotRun>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RobotName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ErrorMessage).HasMaxLength(500);
            });
        }
    }
}