using HotelManagmentAPI.Models;

namespace HotelManagmentAPI.Repository.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task UpdateAsync(Room entity);
    }
}
