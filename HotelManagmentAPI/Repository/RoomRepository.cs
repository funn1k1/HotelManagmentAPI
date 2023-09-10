using HotelManagmentAPI.Data;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Repository.Interfaces;

namespace HotelManagmentAPI.Repository
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
