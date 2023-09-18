using HotelManagment_MVC.Models.DTO.Hotel;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_Utility.Enums;

namespace HotelManagment_MVC.Services
{
    public class HotelService : BaseService, IHotelService
    {
        private readonly string? apiUrl;

        public HotelService(IConfiguration configuration, IHttpClientFactory httlClientFactory, ILogger<BaseService> logger)
            : base(httlClientFactory, logger)
        {
            apiUrl = configuration.GetValue<string>("HotelManagment_API:Url");
        }

        public async Task<APIResponse<T>> CreateAsync<T, K>(K hotelDto)
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = hotelDto,
                Method = APIHttpMethod.POST,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI"
            };
            return await SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> DeleteAsync<T, K>(K id)
        {
            var apiRequest = new APIRequest<K>()
            {
                Method = APIHttpMethod.DELETE,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI/{id}"
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
                Url = $"{apiUrl}/api/HotelAPI"
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
                Url = $"{apiUrl}/api/HotelAPI/{id}"
            };
            return await SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> UpdateAsync<T, K>(K hotelDto) where K : HotelUpdateDTO
        {
            var apiRequest = new APIRequest<K>()
            {
                Data = hotelDto,
                Method = APIHttpMethod.PUT,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI/{hotelDto.Id}"
            };
            return await SendAsync<T, K>(apiRequest);
        }
    }
}
