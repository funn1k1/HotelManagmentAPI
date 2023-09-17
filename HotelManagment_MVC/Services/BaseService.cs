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

        public BaseService(IHttpClientFactory httlClientFactory, ILogger<BaseService> logger)
        {
            _httlClientFactory = httlClientFactory;
            _logger = logger;
        }

        public async Task<APIResponse<string>> SendAsync<T>(APIRequest<T> apiRequest)
        {
            try
            {
                var httpClient = _httlClientFactory.CreateClient("HotelManagment_API");

                var httpRequest = new HttpRequestMessage();
                SetRequestUrl(httpRequest, apiRequest);
                SetHttpMethod(httpRequest, apiRequest);
                SetRequestHeader(httpRequest, apiRequest, "Accept");
                if (apiRequest.Data != null)
                {
                    SetRequestContent(httpRequest, JsonConvert.SerializeObject(apiRequest.Data), Encoding.Unicode, "application/json");
                }

                var httpResponse = await httpClient.SendAsync(httpRequest);

                var apiResponse = new APIResponse<string>();
                apiResponse.StatusCode = httpResponse.StatusCode;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    apiResponse.Result = responseContent;
                    apiResponse.IsSuccess = true;
                }
                else
                {
                    apiResponse.IsSuccess = false;
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    apiResponse.Result = responseContent;
                    _logger.LogError($"API request failed with statusCode: {apiResponse.StatusCode}");
                }
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while sending the request: {ex.Message}");
                return new APIResponse<string>
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

        private void SetRequestHeader<K>(HttpRequestMessage httpRequest, APIRequest<K> apiRequest, string headerName)
        {
            httpRequest.Headers.Add(headerName, apiRequest.Headers[headerName]);
        }

        private void SetRequestContent(
            HttpRequestMessage httpRequest,
            string content,
            Encoding encoding,
            string mediaType
        )
        {
            httpRequest.Content = new StringContent(content, encoding, mediaType);
        }
    }
}