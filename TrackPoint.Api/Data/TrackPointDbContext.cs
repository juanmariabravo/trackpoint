using Microsoft.EntityFrameworkCore;
using TrackPoint.Api.Models;

namespace TrackPoint.Api.Data;

public class TrackPointDbContext : DbContext
{
    public TrackPointDbContext(
        DbContextOptions<TrackPointDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.LicensePlate)
            .IsUnique();
    }
}