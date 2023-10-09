using System.Text;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_Utility.Enums;
using Newtonsoft.Json;

namespace HotelManagment_MVC.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httlClientFactory;
        private readonly ILogger<BaseService> _logger;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httlClientFactory, ILogger<BaseService> logger, ITokenProvider tokenProvider)
        {
            _httlClientFactory = httlClientFactory;
            _logger = logger;
            _tokenProvider = tokenProvider;
        }

        public async Task<APIResponse<T>> SendAsync<T, K>(APIRequest<K> apiRequest, bool bearerExists = false)
        {
            try
            {
                var httpClient = _httlClientFactory.CreateClient("HotelManagment_API");

                var httpReqMess = new HttpRequestMessage();
                SetRequestUrl(httpReqMess, apiRequest);
                SetHttpMethod(httpReqMess, apiRequest);
                SetRequestHeader(httpReqMess, apiRequest, "Accept");
                SetRequestContent(httpReqMess, apiRequest);
                if (bearerExists && !string.IsNullOrEmpty(_tokenProvider.GetToken().AccessToken))
                {
                    var token = _tokenProvider.GetToken().AccessToken;
                    httpReqMess.Headers.Add("Authorization", $"Bearer {token}");
                }

                var httpRespMess = await httpClient.SendAsync(httpReqMess);

                var response = new APIResponse<T>();
                response.StatusCode = httpRespMess.StatusCode;
                if (httpRespMess.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse<T>>(await httpRespMess.Content.ReadAsStringAsync());
                    response.Result = apiResponse.Result;
                    response.IsSuccess = apiResponse.IsSuccess;
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
                    else
                    {
                        response.AddErrorMessage($"API request failed with statusCode: {response.StatusCode}");
                    }
                    _logger.LogError($"API request failed with statusCode: {response.StatusCode}");
                }
                return response;
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

        private void SetRequestUrl<K>(HttpRequestMessage httpRequest, APIRequest<K> apiRequest)
        {
            httpRequest.RequestUri = new Uri(apiRequest.Url);
        }

        private void SetHttpMethod<K>(HttpRequestMessage httpRequest, APIRequest<K> apiRequest)
        {
            switch (apiRequest.Method)
            {
                case APIHttpMethod.GET:
                    httpRequest.Method = new HttpMethod(nameof(APIHttpMethod.GET));
                    break;
                case APIHttpMethod.POST:
                    httpRequest.Method = new HttpMethod(nameof(APIHttpMethod.POST));
                    break;
                case APIHttpMethod.DELETE:
                    httpRequest.Method = new HttpMethod(nameof(APIHttpMethod.DELETE));
                    break;
                case APIHttpMethod.PUT:
                    httpRequest.Method = new HttpMethod(nameof(APIHttpMethod.PUT));
                    break;
                case APIHttpMethod.PATCH:
                    httpRequest.Method = new HttpMethod(nameof(APIHttpMethod.PATCH));
                    break;
            }
        }

        private void SetRequestHeader<K>(
            HttpRequestMessage httpReqMess,
            APIRequest<K> apiRequest,
            string headerName
        )
        {
            if (apiRequest.Headers.TryGetValue(headerName, out var headerValue))
            {
                httpReqMess.Headers.Add(headerName, headerValue);
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
    }
}