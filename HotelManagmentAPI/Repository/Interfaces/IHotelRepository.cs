using HotelManagmentAPI.Models;

namespace HotelManagmentAPI.Repository.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task UpdateAsync(Hotel entity);
    }
}
