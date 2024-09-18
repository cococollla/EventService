using EventProcessor.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventProcessor.WebApi.Data
{
    /// <summary>
    /// Представляет контекст базы данных для службы EventProcessor.
    /// </summary>
    public class ProcessorDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        public ProcessorDbContext(DbContextOptions<ProcessorDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Incident>()
                .HasMany(i => i.Events)
                .WithOne(e => e.Incident)
                .HasForeignKey(e => e.IncidentId);
        }
    }
}
