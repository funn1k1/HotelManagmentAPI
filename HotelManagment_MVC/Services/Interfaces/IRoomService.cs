using HotelManagment_MVC.Models.DTO.Room;

namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IRoomService
    {
        Task<APIResponse<T>> GetAllAsync<T>();

        Task<APIResponse<T>> GetAsync<T, K>(K id);

        Task<APIResponse<T>> CreateAsync<T, K>(K hotelDto);

        Task<APIResponse<T>> UpdateAsync<T, K>(K hotelDto) where K : RoomUpdateDTO;

        Task<APIResponse<T>> DeleteAsync<T, K>(K id);
    }
}
