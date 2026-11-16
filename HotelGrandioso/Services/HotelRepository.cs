using Hotel.Grandioso.Data;
using Hotel.Grandioso.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Grandioso.Services;

public class HotelRepository : IHotelRepository
{
    private readonly HotelContext _db;

    public HotelRepository(HotelContext db)
    {
        _db = db;
    }

    public List<Room> GetAllRooms()
    {
        return _db.Rooms
            .Include(r => r.RoomType)
            .OrderBy(r => r.RoomNumber)
            .ToList();
    }

    public Room? GetRoomByNumber(int roomNumber)
    {
        return _db.Rooms
            .Include(r => r.RoomType)
            .FirstOrDefault(r => r.RoomNumber == roomNumber);
    }

    public Guest? AddGuest(string name, string email)
    {
        var guest = new Guest
        {
            Name = name,
            Email = email
        };
        _db.Guests.Add(guest);
        return TrySave() ? guest:null;
    }

    public bool IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return !_db.Bookings.Any(b =>
            b.RoomId == roomId &&
            b.CheckIn < checkOut &&
            b.CheckOut > checkIn);
    }   

    public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
    {
        return _db.Rooms
            .Include(r => r.RoomType)
            .Where(r => !_db.Bookings.Any(b =>
                b.RoomId == r.Id &&
                b.CheckIn < checkOut &&
                b.CheckOut > checkIn))
            .OrderBy(r => r.RoomNumber)
            .ToList();
    }

    public Booking? CreateBooking(int guestId, int roomId, DateTime checkIn, DateTime checkOut)
    {
        if (checkOut <= checkIn || !IsRoomAvailable(roomId, checkIn, checkOut))
        {
            return null;
        }

        var booking = new Booking
        {
            GuestId = guestId,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut
        };

        _db.Bookings.Add(booking);
        return TrySave() ? booking:null;
    }

    public List<Booking> GetAllBookings()
    {
        return _db.Bookings
            .Include(b => b.Guest)
            .Include(b => b.Room)
            .OrderBy(b => b.CheckIn)
            .ToList();
    }

    public bool CancelBooking(int bookingId)
    {
        var booking = _db.Bookings.Find(bookingId);
        if (booking == null)
        {
            return false;
        }

        _db.Bookings.Remove(booking);
        _db.SaveChanges();
        return TrySave();    
    }

    public decimal CalculateTotalPrice(Booking booking)
    {
        int nights = (booking.CheckOut - booking.CheckIn).Days;
        return nights * booking.Room.PricePerNight;
    }

    private bool TrySave()
    {
        try
        {
            _db.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
}