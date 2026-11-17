using Hotel.Grandioso.Data;
using Hotel.Grandioso.Services;
using Hotel.Grandioso.Services;
using Hotel.Grandioso.UI;
using Microsoft.EntityFrameworkCore;

using var db = new HotelContext();
// Skapar databasen och migratoiner vid start 
db.Database.Migrate();

IHotelRepository repo = new HotelRepository(db);
new ConsoleMenu(repo).Run();