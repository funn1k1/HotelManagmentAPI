using HotelManagment_MVC.Models.DTO.Hotel;

namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IHotelService
    {
        Task<APIResponse<string>> GetAllAsync();

        Task<APIResponse<string>> GetAsync(int id);

        Task<APIResponse<string>> CreateAsync(HotelCreateDTO hotelCreateDto);

        Task<APIResponse<string>> UpdateAsync(HotelUpdateDTO hotelUpdateDto);

        Task<APIResponse<string>> DeleteAsync(int id);
    }
}
