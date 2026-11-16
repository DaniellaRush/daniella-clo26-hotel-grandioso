using Hotel.Grandioso.Data;
using Hotel.Grandioso.Services;
using Microsoft.EntityFrameworkCore;

using var db = new HotelContext();
db.Database.Migrate();

var repo = new HotelRepository(db);

Console.WriteLine("Välkommen till Hotel Grandioso!");
Console.WriteLine();
foreach (var room in repo.GetAllRooms())
{
    Console.WriteLine($"Rum: {room.RoomNumber} {room.RoomType.Name} ({room.SizeM2} kvm) - {room.PricePerNight} kr/natt");
}