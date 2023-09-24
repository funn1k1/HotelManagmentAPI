using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_Utility.Enums;

namespace HotelManagment_MVC.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly string _apiUrl;

        public AccountService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<AccountService> logger
        ) : base(httpClientFactory, logger)
        {
            _apiUrl = $"{configuration.GetValue<string>("HotelManagment_API:Domain")}/" +
                $"{configuration.GetValue<string>("HotelManagment_API:AccountApiUrl")}";
        }

        public async Task<APIResponse<T>> Register<T, K>(K entity)
        {
            var apiRequest = new APIRequest<K>
            {
                Data = entity,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" }
                },
                Method = APIHttpMethod.POST,
                Url = $"{_apiUrl}/register"
            };

            return await SendAsync<T, K>(apiRequest);
        }

        public async Task<APIResponse<T>> Login<T, K>(K entity)
        {
            var apiRequest = new APIRequest<K>
            {
                Data = entity,
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" }
                },
                Method = APIHttpMethod.POST,
                Url = $"{_apiUrl}/login"
            };
            return await SendAsync<T, K>(apiRequest);
        }
    }
}
