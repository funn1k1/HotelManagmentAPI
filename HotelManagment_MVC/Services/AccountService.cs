using HotelManagment_MVC.Services.Interfaces;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace HotelManagment_MVC.Services
{
    public class AccountService : IAccountService
    {
        private readonly string _apiUrl;
        private readonly IBaseService _baseService;

        public AccountService(IConfiguration configuration, IBaseService baseService)
        {
            _apiUrl = $"{configuration.GetValue<string>("HotelManagment_API:Domain")}/" +
                $"{configuration.GetValue<string>("HotelManagment_API:AccountApiUrl")}";
            _baseService = baseService;
        }

        public async Task<APIResponse<T>> Register<T, K>(K entity)
        {
            var apiRequest = new APIRequest<K>
            {
                Data = entity,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Method = HttpMethod.Post,
                Url = $"{_apiUrl}/register"
            };

            return await _baseService.SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> Login<T, K>(K entity)
        {
            var apiRequest = new APIRequest<K>
            {
                Data = entity,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Method = HttpMethod.Post,
                Url = $"{_apiUrl}/login"
            };
            return await _baseService.SendAsync<T, K>(apiRequest);
        }
    }
}