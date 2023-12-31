﻿using HotelManagment_MVC.Services.Interfaces;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace HotelManagment_MVC.Services
{
    public class TokenService : ITokenService
    {
        private readonly IBaseService _baseService;
        private readonly string _apiUrl;

        public TokenService(IBaseService baseService, IConfiguration configuration)
        {
            _baseService = baseService;
            _apiUrl = $"{configuration.GetValue<string>("HotelManagment_API:Domain")}/" +
                $"{configuration.GetValue<string>("HotelManagment_API:TokenApiUrl")}";
        }

        public async Task<APIResponse<T>> RevokeTokenAsync<T>()
        {
            var apiRequest = new APIRequest<string>
            {
                Method = HttpMethod.Post,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}/revoke"
            };

            return await _baseService.SendAsync<T, string>(apiRequest, bearerExists: true);
        }

        public async Task<APIResponse<T>> RefreshTokenAsync<T, K>(K entity)
        {
            var apiRequest = new APIRequest<K>
            {
                Data = entity,
                Method = HttpMethod.Post,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = $"{_apiUrl}/refresh"
            };

            return await _baseService.SendAsync<T, K>(apiRequest, bearerExists: true);
        }
    }
}