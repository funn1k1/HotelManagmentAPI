using HotelManagment_API.Models;

namespace HotelManagment_API.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateAsync(User user);
    }
}
