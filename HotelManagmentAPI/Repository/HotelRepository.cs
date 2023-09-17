using HotelManagment_API.Data;
using HotelManagment_API.Models;
using HotelManagment_API.Repository.Interfaces;

namespace HotelManagment_API.Repository
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
