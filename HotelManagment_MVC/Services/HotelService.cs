using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace HotelManagment_MVC.Services
{
    public class HotelService : IHotelService
    {
        private readonly string? _apiUrl;
        private readonly IBaseService _baseService;

        public HotelService(IConfiguration configuration, IBaseService baseService)
        {
            _apiUrl = $"{configuration.GetValue<string>("HotelManagment_API:Domain")}/" +
                $"{configuration.GetValue<string>("HotelManagment_API:HotelApiUrl")}";
            _baseService = baseService;
        }

        public async Task<APIResponse<T>> CreateAsync<T, K>(K entity)
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = entity,
                Method = HttpMethod.Post,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "multipart/form-data" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}"
            };
            return await _baseService.SendAsync<T, K>(apiRequest, bearerExists: true);
        }

        public async Task<APIResponse<T>> DeleteAsync<T>(int id)
        {
            var apiRequest = new APIRequest<int>()
            {
                Method = HttpMethod.Delete,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}/{id}"
            };
            return await _baseService.SendAsync<T, int>(apiRequest, bearerExists: true);
        }

        public async Task<APIResponse<T>> GetAllAsync<T>()
        {
            var apiRequest = new APIRequest<T>()
            {
                Method = HttpMethod.Get,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" },
                },
                Url = $"{_apiUrl}"
            };
            return await _baseService.SendAsync<T, T>(apiRequest);
        }

        public async Task<APIResponse<T>> GetAsync<T>(int id)
        {
            var apiRequest = new APIRequest<int>()
            {
                Method = HttpMethod.Get,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" },
                },
                Url = $"{_apiUrl}/{id}"
            };
            return await _baseService.SendAsync<T, int>(apiRequest);
        }

        public async Task<APIResponse<T>> UpdateAsync<T, K>(K entity) where K : HotelUpdateDTO
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = entity,
                Method = HttpMethod.Put,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "multipart/form-data" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}/{entity.Id}"
            };
            return await _baseService.SendAsync<T, K>(apiRequest, bearerExists: true);
        }
    }
}