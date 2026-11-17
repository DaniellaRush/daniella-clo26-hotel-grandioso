using Hotel.Grandioso.Data;
using Hotel.Grandioso.Services;
using Hotel.Grandioso.Services;
using Hotel.Grandioso.UI;
using Microsoft.EntityFrameworkCore;

using var db = new HotelContext();
db.Database.Migrate();

IHotelRepository repo = new HotelRepository(db);
new ConsoleMenu(repo).Run();