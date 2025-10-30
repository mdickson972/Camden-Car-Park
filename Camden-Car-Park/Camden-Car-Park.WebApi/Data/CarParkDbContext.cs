using Camden_Car_Park.WebApi.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace Camden_Car_Park.WebApi.Data;

public class CarParkDbContext : DbContext
{
    public CarParkDbContext(DbContextOptions<CarParkDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
        .Property(b => b.EmployeeId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Vehicle>()
        .Property(b => b.VehicleId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Booking>()
        .Property(b => b.BookingId)
        .ValueGeneratedOnAdd();
    }
}