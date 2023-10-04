using HotelManagment_MVC.Models.DTO.Room;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_Utility.Enums;

namespace HotelManagment_MVC.Services
{
    public class RoomService : IRoomService
    {
        private readonly string? _apiUrl;
        private readonly IBaseService _baseService;

        public RoomService(IConfiguration configuration, IBaseService baseService)
        {
            _apiUrl = $"{configuration.GetValue<string>("HotelManagment_API:Domain")}/" +
                $"{configuration.GetValue<string>("HotelManagment_API:RoomApiUrl")}";
            _baseService = baseService;
        }

        public async Task<APIResponse<T>> CreateAsync<T, K>(K roomDto)
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = roomDto,
                Method = APIHttpMethod.POST,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}"
            };
            return await _baseService.SendAsync<T, K>(apiRequest, bearerExists: true);
        }

        public async Task<APIResponse<T>> DeleteAsync<T, K>(K id)
        {
            var apiRequest = new APIRequest<K>()
            {
                Method = APIHttpMethod.DELETE,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}/{id}",
            };
            return await _baseService.SendAsync<T, K>(apiRequest, bearerExists: true);
        }

        public async Task<APIResponse<T>> GetAllAsync<T>()
        {
            var apiRequest = new APIRequest<T>()
            {
                Method = APIHttpMethod.GET,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" },
                },
                Url = $"{_apiUrl}"
            };
            return await _baseService.SendAsync<T, T>(apiRequest);
        }

        public async Task<APIResponse<T>> GetAsync<T, K>(K id)
        {
            var apiRequest = new APIRequest<K>()
            {
                Method = APIHttpMethod.GET,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" },
                },
                Url = $"{_apiUrl}/{id}"
            };
            return await _baseService.SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> UpdateAsync<T, K>(K roomDto) where K : RoomUpdateDTO
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = roomDto,
                Method = APIHttpMethod.PUT,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}/{roomDto.Id}"
            };
            return await _baseService.SendAsync<T, K>(apiRequest, bearerExists: true);
        }
    }
}
