using Hotel.Grandioso.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Grandioso.Data;

public class HotelContext : DbContext
{
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<SpaBooking> SpaBookings { get; set; }

    //SQLite valdes eftersom det inte kräver någon installation utan migrationen skapar det vid start 
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=hotel.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Seed data läggs in via migrationer så att alla som klonat projektet får samma rum  
        modelBuilder.Entity<RoomType>().HasData(
            new RoomType { Id = 1, Name = "Standard", Description = "Standardrum" },
            new RoomType { Id = 2, Name = "Lyx", Description = "Lyxsvit"},
            new RoomType { Id = 3, Name = "Bröllop", Description = "Bröllopsvit" });

        var rooms = new List<Room>();
        int id = 1;
        
        //Hjälper till att skapa fler rum/våning så slipper jag skriva ut alla rum för hand
        void Add(int floor, int from, int to, int size, decimal price, int typeId)
        {
            for (int nr = from; nr <= to; nr++)
            {
                rooms.Add(new Room
                {
                    Id = id++, RoomNumber = nr, Floor = floor, 
                    SizeM2 = size, PricePerNight = price, RoomTypeId = typeId
                });
            }
        }

        Add(3, 301, 303, 120, 1200, 1);
        Add(3, 304, 304, 180, 2800, 2);
        Add(3, 305, 305, 220, 3500, 3);
        Add(2, 201, 205, 100, 1000, 1);
        Add(1, 101, 105, 80, 800, 1);

        modelBuilder.Entity<Room>().HasData(rooms);

        modelBuilder.Entity<Treatment>().HasData(
            new Treatment { Id = 1, Name = "Entré (per timme)", DurationMinutes = 60, Price = 150},
            new Treatment { Id = 2, Name = "Massage (60 min)", DurationMinutes = 60, Price = 950},
            new Treatment { Id = 3, Name = "Pedikyr (stortå)", DurationMinutes = 30, Price = 350},
            new Treatment { Id = 4, Name = "Pedikyr (fot)", DurationMinutes = 60, Price = 650});
    }
}