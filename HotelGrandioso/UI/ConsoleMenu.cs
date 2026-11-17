using System.Runtime.CompilerServices;
using Hotel.Grandioso.Services;

namespace Hotel.Grandioso.UI;

public class ConsoleMenu
{
    private readonly IHotelRepository _repo;

    public ConsoleMenu(IHotelRepository repo)
    {
        _repo = repo;
    }

    public void Run()
    {
        Console.WriteLine("Välkommen till Hotel Grandioso!");
        Console.WriteLine();
        bool running = true;

        while (running)
        {
            Console.WriteLine("1. Visa alla rum");
            Console.WriteLine("2. Visa lediga rum");
            Console.WriteLine("3. Boka rum");
            Console.WriteLine("4. Visa alla bokningar");
            Console.WriteLine("5. Boka spabehandling");
            Console.WriteLine("6. Visa spabokningar");
            Console.WriteLine("7. Avboka");
            Console.WriteLine("8. Avsluta");
            Console.Write("Val: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            if (choice == "1") ShowRooms(); 
            else if (choice == "2") ShowAvailableRooms();
            else if (choice == "3") BookRoom();
            else if (choice == "4") ShowBookings();
            else if (choice == "5") BookSpa();
            else if (choice == "6") ShowSpaBookings();
            else if (choice == "7") CancelBooking();
            else if (choice == "8") running = false;
            else Console.WriteLine("Ogiltigt val, försök igen.");
        }
    }
    private void ShowRooms()
    {
        foreach (var room in _repo.GetAllRooms())
        {
            Console.WriteLine($"Rum {room.RoomNumber}: {room.RoomType.Name}, {room.SizeM2} kvm, {room.PricePerNight} kr/natt");
        }
    }
    
    private void ShowAvailableRooms()
    {   
        DateTime checkIn = ReadDate("Incheckningsdatum (ÅÅÅÅ-MM-DD): ");
        DateTime checkOut = ReadDate("Utcheckningsdatum (ÅÅÅÅ-MM-DD): ");

        if (checkOut <= checkIn)
        {
            Console.WriteLine("Utcheckningsdatum måste vara efter incheckningsdatum.");
            return;
        }

        var rooms = _repo.GetAvailableRooms(checkIn, checkOut);
        if (rooms.Count == 0)
        {
            Console.WriteLine("Inga lediga rum för de valda datumen.");
            return;
        }

        foreach (var room in rooms)
        {
            Console.WriteLine($"Rum {room.RoomNumber}: {room.RoomType.Name}, {room.SizeM2} kvm, {room.PricePerNight} kr/natt");
        }
    }
    private void BookRoom()
    {
        int roomNumber = ReadInt("Rumsnummer: ");
        var room = _repo.GetRoomByNumber(roomNumber);
        if (room == null)
        {
            Console.WriteLine("Rummet finns inte.");
            return;
        }

        DateTime checkIn = ReadDate("Incheckningsdatum (ÅÅÅÅ-MM-DD): ");
        DateTime checkOut = ReadDate("Utcheckningsdatum (ÅÅÅÅ-MM-DD): ");

        if (checkOut <= checkIn)
        {
            Console.WriteLine("Utcheckningssdatum måste vara efter incheckningsdatum.");
            return;
        }

        if (!_repo.IsRoomAvailable(room.Id, checkIn, checkOut))
        {
            Console.WriteLine("Rummet är redan bokat under de valda datumen.");
            return;
        }
        Console.Write("Gästens namn: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Gästens e-post: ");
        string email = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Namn och e-post får inte vara tomma.");
            return;
        }

        var guest = _repo.AddGuest(name, email);
        var booking = guest == null ? null : _repo.CreateBooking(guest.Id, room.Id, checkIn, checkOut);

        if (booking == null)
        {
            Console.WriteLine("Bokningen kunde inte genomföras.");
            return;
        }

        Console.WriteLine($"Bokning {booking.Id} skapad. Totalpris: {_repo.CalculateTotalPrice(booking)} kr.");
    }
    private void ShowBookings()
    {
        var bookings = _repo.GetAllBookings();
        if (bookings.Count == 0)
        {
            Console.WriteLine("Det finns inga bokningar.");
            return;
        }
        foreach(var b in bookings)
        {
            Console.WriteLine($"Bokning {b.Id}: {b.Guest.Name}, {b.Room.RoomNumber}.");
            Console.WriteLine($"Period: {b.CheckIn:yyyy-MM-dd} - {b.CheckOut:yyyy-MM-dd}, {_repo.CalculateTotalPrice(b)} kr.");
        }
    }

    private void BookSpa()
    {
        var treatments = _repo.GetAllTreatments();
        foreach (var t in treatments)
        {
            Console.WriteLine($"Behandling {t.Id}: {t.Name}, {t.DurationMinutes} min, {t.Price} kr");
        }

        int treatmentId = ReadInt("Välj Behandlingsnummer: ");
        var treatment = treatments.FirstOrDefault(t => t.Id == treatmentId);
        if (treatment == null)
        {
            Console.WriteLine("Behandlingen finns inte.");
            return;
        }

        DateTime date = ReadDate("Datum och tid för behandling (ÅÅÅÅ-MM-DD HH:MM): ");
        if (date < DateTime.Now)
        {
            Console.WriteLine("Tiden har redan passerat.");
            return;
        }

        Console.Write("Namn: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("E-post: ");
        string email = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Namn och e-post får inte vara tomma.");
            return;
        }

        var guest = _repo.AddGuest(name, email);
        var spaBooking = guest == null ? null : _repo.CreateSpaBooking(guest.Id, treatment.Id, date);

        Console.WriteLine(spaBooking != null ? 
                          $"Spabokning {spaBooking.Id} skapad: {treatment.Name} för {treatment.Price} kr" 
                          : "Spabokningen kunde inte genomföras.");
    }

    private void ShowSpaBookings()
    {
        var bookings = _repo.GetAllSpaBookings();
        if (bookings.Count == 0)
        {
            Console.WriteLine("Det finns inga spabokningar.");
            return;
        }
        foreach (var s in bookings)
        {
            Console.WriteLine($"Spabokning {s.Id}: {s.Guest.Name}, {s.Treatment.Name}, {s.Date:yyyy-MM-dd HH:mm}, {s.Treatment.Price} kr");
        }
    }

    private void CancelBooking()
    {
        int id = ReadInt("Bokningsnummer att avboka: ");
        Console.WriteLine(_repo.CancelBooking(id) ? "Bokningen är avbokad" : "Hittade ingen bokning med det ID:t");
    }
        private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }
            Console.WriteLine("Skriv ett heltal");
        }
        
    }

    private static DateTime ReadDateTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dateTime))
            {
                return dateTime;
            }
            Console.WriteLine("Ogiltigt datum och tid, skriv i formatet ÅÅÅÅ-MM-DD HH:MM");
        }
    }

    private static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if(DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                return date.Date;
            }
            Console.WriteLine("Ogiltigt datum, skriv i formatet ÅÅÅÅ-MM-DD");
        }
    }
    }
