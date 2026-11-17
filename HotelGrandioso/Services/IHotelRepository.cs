using Hotel.Grandioso.Models;

namespace Hotel.Grandioso.Services;

//Säger till vad, inte hur, som kan göras. Gör att databasen kan bytas utan att UI:n ändras
public interface IHotelRepository
{
    List<Room> GetAllRooms();
    Room? GetRoomByNumber(int roomNumber);
    List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
    bool IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut);

    Guest? AddGuest(string name, string email);

    Booking? CreateBooking(int guestId, int roomId, DateTime checkIn, DateTime checkOut);
    List<Booking> GetAllBookings();
    bool CancelBooking(int bookingId);
    decimal CalculateTotalPrice(Booking booking);

    List<Treatment> GetAllTreatments();
    SpaBooking? CreateSpaBooking(int guestId, int treatmentId, DateTime date);
    List<SpaBooking> GetAllSpaBookings();
}
