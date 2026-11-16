namespace Hotel.Grandioso.Models;

public class SpaBooking
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;

    public int TreatmentId { get; set; }
    public Treatment Treatment { get; set; } = null!;
}