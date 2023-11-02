using HotelManagment_MVC.Models.DTO.Hotel;

namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IHotelService
    {
        Task<APIResponse<T>> GetAllAsync<T>();

        Task<APIResponse<T>> GetAsync<T>(int id);

        Task<APIResponse<T>> CreateAsync<T, K>(K entity);

        Task<APIResponse<T>> UpdateAsync<T, K>(K entity) where K : HotelUpdateDTO;

        Task<APIResponse<T>> DeleteAsync<T>(int id);
    }
}
