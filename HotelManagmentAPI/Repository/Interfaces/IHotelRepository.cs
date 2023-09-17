using HotelManagment_API.Models;

namespace HotelManagment_API.Repository.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task UpdateAsync(Hotel entity);
    }
}
