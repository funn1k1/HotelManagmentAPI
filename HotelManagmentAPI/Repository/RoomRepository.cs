using HotelManagment_API.Data;
using HotelManagment_API.Models;
using HotelManagment_API.Repository.Interfaces;

namespace HotelManagment_API.Repository
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext db) : base(db) { }

        public async Task UpdateAsync(Room room)
        {
            _db.Update(room);
            await SaveAsync();
        }
    }
}
