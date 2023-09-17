using HotelManagment_API.Models;

namespace HotelManagment_API.Repository.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task UpdateAsync(Room entity);
    }
}
