using Hotel.Grandioso.Data;
using Hotel.Grandioso.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Grandioso.Services;

public class HotelRepository
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
}