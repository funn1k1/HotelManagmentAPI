using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_Utility.Enums;

namespace HotelManagment_MVC.Services
{
    public class HotelService : BaseService, IHotelService
    {
        private readonly string? _apiUrl;

        public HotelService(
            IConfiguration configuration,
            IHttpClientFactory httlClientFactory,
            ILogger<BaseService> logger
        ) : base(httlClientFactory, logger)
        {
            _apiUrl = $"{configuration.GetValue<string>("HotelManagment_API:Domain")}/" +
                $"{configuration.GetValue<string>("HotelManagment_API:HotelAPIUrl")}";
        }

        public async Task<APIResponse<T>> CreateAsync<T, K>(K entity, string token)
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = entity,
                Method = APIHttpMethod.POST,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Bearer " + token }
                },
                Url = $"{_apiUrl}"
            };
            return await SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> DeleteAsync<T, K>(K id, string token)
        {
            var apiRequest = new APIRequest<K>()
            {
                Method = APIHttpMethod.DELETE,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Bearer " + token }
                },
                Url = $"{_apiUrl}/{id}"
            };
            return await SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> GetAllAsync<T>()
        {
            var apiRequest = new APIRequest<T>()
            {
                Method = APIHttpMethod.GET,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{_apiUrl}"
            };
            return await SendAsync<T, T>(apiRequest);
        }

        public async Task<APIResponse<T>> GetAsync<T, K>(K id)
        {
            var apiRequest = new APIRequest<K>()
            {
                Method = APIHttpMethod.GET,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{_apiUrl}/{id}"
            };
            return await SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> UpdateAsync<T, K>(K entity, string token) where K : HotelUpdateDTO
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = entity,
                Method = APIHttpMethod.PUT,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Bearer " + token }
                },
                Url = $"{_apiUrl}/{entity.Id}"
            };
            return await SendAsync<T, K>(apiRequest);
        }
    }
}
