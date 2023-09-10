using HotelManagmentAPI.Data;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Repository.Interfaces;

namespace HotelManagmentAPI.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _db;

        public HotelRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Hotel entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
