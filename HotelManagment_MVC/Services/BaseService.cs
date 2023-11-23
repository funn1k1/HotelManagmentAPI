using System.Net;
using System.Net.Http.Headers;
using System.Text;
using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Account;
using HotelManagment_MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace HotelManagment_MVC.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BaseService> _logger;
        private readonly ITokenProvider _tokenProvider;
        private TokenDTO? newToken;

        public BaseService(IHttpClientFactory httpClientFactory, ILogger<BaseService> logger, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenProvider = tokenProvider;
        }

        public async Task<APIResponse<T>> SendAsync<T, K>(APIRequest<K> apiRequest, bool bearerExists = false)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("HotelManagment_API");

                var httpReqMess = BuildRequestAsync(apiRequest, bearerExists);

                var httpRespMess = await httpClient.SendAsync(httpReqMess);
                if (!string.IsNullOrEmpty(_tokenProvider.GetToken().RefreshToken) &&
                    httpRespMess.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var tokenDto = await RefreshTokenAsync(httpClient);
                    if (tokenDto == null)
                    {
                        await RevokeTokenAsync(httpClient);
                        return new APIResponse<T>
                        {
                            IsSuccess = false,
                            ErrorMessages = new List<string> { "Unauthorized" }
                        };
                    }

                    newToken = tokenDto;
                    _tokenProvider.SetToken(newToken);
                    var tokenReqMess = BuildRequestAsync(apiRequest, bearerExists);
                    httpRespMess = await httpClient.SendAsync(tokenReqMess);
                }

                return await ProcessResponseAsync<T>(httpRespMess);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while sending the request: {ex.Message}");
                return new APIResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occured while sending the request" }
                };
            }
        }

        private void SetRequestContent<K>(HttpRequestMessage httpReqMess, APIRequest<K> apiRequest)
        {
            if (apiRequest.Data != null)
            {
                var contentType = apiRequest.Headers["Content-Type"];
                switch (contentType)
                {
                    case "application/json":
                        httpReqMess.Content = new StringContent(
                            JsonConvert.SerializeObject(apiRequest.Data),
                            Encoding.Unicode,
                            contentType
                        );
                        break;
                    case "multipart/form-data":
                        var formDataContent = new MultipartFormDataContent();
                        foreach (var prop in apiRequest.Data.GetType().GetProperties())
                        {
                            var value = prop.GetValue(apiRequest.Data);
                            if (value is FormFile)
                            {
                                var file = value as FormFile;
                                if (file != null)
                                {
                                    var streamContent = new StreamContent(file.OpenReadStream());
                                    formDataContent.Add(streamContent, prop.Name, file.FileName);
                                }
                            }
                            else
                            {
                                var stringContent = new StringContent(value?.ToString() ?? "");
                                formDataContent.Add(stringContent, prop.Name);
                            }
                        }
                        httpReqMess.Content = formDataContent;
                        break;
                }
            }
        }

        private async Task<TokenDTO?> RefreshTokenAsync(HttpClient httpClient)
        {
            var oldTokenDto = _tokenProvider.GetToken();
            var apiRequest = new APIRequest<TokenDTO>
            {
                Data = oldTokenDto,
                Method = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod.Post,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = "https://localhost:7238/api/v2/TokenAPI/refresh"
            };

            var httpReqMess = BuildRequestAsync(apiRequest);
            var httpRespMess = await httpClient.SendAsync(httpReqMess);
            var apiResponse = await ProcessResponseAsync<TokenDTO>(httpRespMess);

            return apiResponse.Result;
        }

        private async Task<Token?> RevokeTokenAsync(HttpClient httpClient)
        {
            var apiRequest = new APIRequest<TokenDTO>
            {
                Method = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod.Post,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                },
                Url = "https://localhost:7238/api/v2/TokenAPI/revoke"
            };

            var httpReqMess = BuildRequestAsync(apiRequest);
            var httpRespMess = await httpClient.SendAsync(httpReqMess);
            var apiResponse = await ProcessResponseAsync<Token>(httpRespMess);

            return apiResponse.Result;
        }

        private HttpRequestMessage BuildRequestAsync<K>(APIRequest<K> apiRequest, bool bearerExists = false)
        {
            var httpReqMess = new HttpRequestMessage
            {
                RequestUri = new Uri(apiRequest.Url),
                Method = new HttpMethod(apiRequest.Method.ToString())
            };

            if (apiRequest.Headers.TryGetValue("Accept", out var accept))
            {
                httpReqMess.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }

            if (bearerExists && !string.IsNullOrEmpty(newToken?.AccessToken))
            {
                httpReqMess.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken.AccessToken);
            }

            SetRequestContent(httpReqMess, apiRequest);
            return httpReqMess;
        }

        private async Task<APIResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage httpRespMess)
        {
            var response = new APIResponse<T>()
            {
                StatusCode = httpRespMess.StatusCode
            };

            if (httpRespMess.IsSuccessStatusCode)
            {
                var apiResponse = JsonConvert.DeserializeObject<APIResponse<T>>(await httpRespMess.Content.ReadAsStringAsync());
                if (apiResponse != null)
                {
                    response.Result = apiResponse.Result;
                    response.IsSuccess = apiResponse.IsSuccess;
                }
            }
            else
            {
                var apiResponse = JsonConvert.DeserializeObject<APIResponse<T>>(await httpRespMess.Content.ReadAsStringAsync());
                if (apiResponse != null)
                {
                    response.Result = apiResponse.Result;
                    response.ErrorMessages = apiResponse.ErrorMessages;
                    response.IsSuccess = apiResponse.IsSuccess;
                }

                response.AddErrorMessage($"API request failed with statusCode: {response.StatusCode}");
                _logger.LogError($"API request failed with statusCode: {response.StatusCode}");
            }

            return response;
        }
    }
}