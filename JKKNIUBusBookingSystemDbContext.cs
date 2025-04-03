using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection.Metadata;
using JKKNIUBusBookingSystem.Entites;

namespace JKKNIUBusBookingSystem.db;

public class JKKNIUBusBookingSystemDbContext : DbContext
{

    public JKKNIUBusBookingSystemDbContext(DbContextOptions<JKKNIUBusBookingSystemDbContext> options) : base(options) { }
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ScheduleBusSeats> ScheduleBusSeats { get; set; }
    public DbSet<ScheduleBus> ScheduleBuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ScheduleBusSeats>().HasKey(s => new { s.ScheduleId, s.SeatName });
    }
}