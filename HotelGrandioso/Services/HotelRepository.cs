using Hotel.Grandioso.Data;
using Hotel.Grandioso.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Grandioso.Services;

public class HotelRepository : IHotelRepository
{
    //Kontext skickas in utifrån så klassen själv inte bestämmer hur databasen skapas
    private readonly HotelContext _db;

    public HotelRepository(HotelContext db)
    {
        _db = db;
    }

    public List<Room> GetAllRooms()
    {
        return _db.Rooms
            //.Include för att EF Core inte kan hämta kopplade objekt själv
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
        //Två bokningsdatum överlappar när den ena börjar före den andras slut
        //Använder < och > istället för <= & >= så kan en gäst checka in samma dag som en annan checkar ut
        return !_db.Bookings.Any(b =>
            b.RoomId == roomId &&
            b.CheckIn < checkOut &&
            b.CheckOut > checkIn);
    }   

    public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
    {
        return _db.Rooms
            .Include(r => r.RoomType)
            //Samma fråga som i IsRoomAvailable men istället en enda fråga mot databasen ist för en fråga per rum
            .Where(r => !_db.Bookings.Any(b =>
                b.RoomId == r.Id &&
                b.CheckIn < checkOut &&
                b.CheckOut > checkIn))
            .OrderBy(r => r.RoomNumber)
            .ToList();
    }

    public Booking? CreateBooking(int guestId, int roomId, DateTime checkIn, DateTime checkOut)
    {
        //Gör att dubbelbokning är omöhligt oavsett vem som anropar metoden (Margareta)
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
        //Antal nätter är skillnad i dagar, utchekcning räknas inte som en natt
        int nights = (booking.CheckOut - booking.CheckIn).Days;
        return nights * booking.Room.PricePerNight;
    }

    public List<Treatment> GetAllTreatments()
    {
        return _db.Treatments.OrderBy(t => t.Id).ToList();
    }

    public SpaBooking? CreateSpaBooking(int guestId, int treatmentId, DateTime date)
    {
        // Spabokning kräver inte att man är gäst i hotellet
        var spaBooking = new SpaBooking
        {
            GuestId = guestId,
            TreatmentId = treatmentId,
            Date = date
        };
        _db.SpaBookings.Add(spaBooking);
        return TrySave() ? spaBooking : null;
    }

    public List<SpaBooking> GetAllSpaBookings()
    {
        return _db.SpaBookings
            .Include(sb => sb.Guest)
            .Include(sb => sb.Treatment)
            .OrderBy(sb => sb.Date)
            .ToList();
    }

    //Gör att programmet inte kraschar, så metoder kan returnera null eller false ist i det fall att databasen är otillgänglig
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