using HotelManagment_API.Data;
using HotelManagment_API.Models;
using HotelManagment_API.Repository.Interfaces;

namespace HotelManagment_API.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext db) : base(db) { }

        public async Task UpdateAsync(Hotel hotel)
        {
            _db.Update(hotel);
            await SaveAsync();
        }
    }
}
