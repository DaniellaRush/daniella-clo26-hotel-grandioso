namespace Hotel.Grandioso.Models;

public class Booking
{
    public int Id { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
} 
