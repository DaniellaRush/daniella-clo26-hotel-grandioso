namespace Hotel.Grandioso.Models;

public class Guest
{
    // SParar bara namn och mejl med tanke på GDPR principerna dataminimering
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";

    public List<Booking> Bookings { get; set; } = new();
    public List<SpaBooking> SpaBookings { get; set; } = new();
}