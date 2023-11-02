using HotelManagment_MVC.Models.DTO.Room;

namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IRoomService
    {
        Task<APIResponse<T>> GetAllAsync<T>();

        Task<APIResponse<T>> GetAsync<T>(int id);

        Task<APIResponse<T>> CreateAsync<T, K>(K entity);

        Task<APIResponse<T>> UpdateAsync<T, K>(K entity) where K : RoomUpdateDTO;

        Task<APIResponse<T>> DeleteAsync<T>(int id);
    }
}
