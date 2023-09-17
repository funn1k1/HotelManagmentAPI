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

        public async Task<APIResponse<string>> CreateAsync(HotelCreateDTO hotelCreateDto)
        {
            var apiRequest = new APIRequest<HotelCreateDTO>()
            {
                Data = hotelCreateDto,
                Method = APIHttpMethod.POST,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI"
            };
            return await SendAsync(apiRequest);
        }

        public async Task<APIResponse<string>> DeleteAsync(int id)
        {
            var apiRequest = new APIRequest<int>()
            {
                Method = APIHttpMethod.DELETE,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI/{id}"
            };
            return await SendAsync(apiRequest);
        }

        public async Task<APIResponse<string>> GetAllAsync()
        {
            var apiRequest = new APIRequest<HotelDTO>()
            {
                Method = APIHttpMethod.GET,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI"
            };
            return await SendAsync(apiRequest);
        }

        public async Task<APIResponse<string>> GetAsync(int id)
        {
            var apiRequest = new APIRequest<HotelDTO>()
            {
                Method = APIHttpMethod.GET,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI/{id}"
            };
            return await SendAsync(apiRequest);
        }

        public async Task<APIResponse<string>> UpdateAsync(HotelUpdateDTO hotelUpdateDto)
        {
            var apiRequest = new APIRequest<HotelUpdateDTO>()
            {
                Data = hotelUpdateDto,
                Method = APIHttpMethod.PUT,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                },
                Url = $"{apiUrl}/api/HotelAPI/{hotelUpdateDto.Id}"
            };
            return await SendAsync(apiRequest);
        }
    }
}
