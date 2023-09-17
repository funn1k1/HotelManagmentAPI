using HotelManagment_API.Data;
using HotelManagment_API.Models;
using HotelManagment_API.Repository.Interfaces;

namespace HotelManagment_API.Repository
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _db;

        public RoomRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Room entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
