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

        public async Task<APIResponse<T>> SendAsync<T, K>(APIRequest<K> apiRequest)
        {
            try
            {
                var httpClient = _httlClientFactory.CreateClient("HotelManagment_API");

                var httpReqMess = new HttpRequestMessage();
                SetRequestUrl(httpReqMess, apiRequest);
                SetHttpMethod(httpReqMess, apiRequest);
                SetRequestHeader(httpReqMess, apiRequest, "Accept");
                SetRequestHeader(httpReqMess, apiRequest, "Authorization");
                SetRequestContent(httpReqMess, apiRequest);

                var httpRespMess = await httpClient.SendAsync(httpReqMess);

                var response = new APIResponse<T>();
                response.StatusCode = httpRespMess.StatusCode;
                if (httpRespMess.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse<T>>(await httpRespMess.Content.ReadAsStringAsync());
                    response.Result = apiResponse.Result;
                    response.IsSuccess = httpRespMess.IsSuccessStatusCode;
                }
                else
                {
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse<T>>(await httpRespMess.Content.ReadAsStringAsync());
                    if (apiResponse != null)
                    {
                        response.Result = apiResponse.Result;
                        response.ErrorMessages = apiResponse.ErrorMessages;
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
            HttpRequestMessage httpRequest,
            APIRequest<K> apiRequest,
            string headerName
        )
        {
            if (apiRequest.Headers.TryGetValue(headerName, out var headerValue))
            {
                httpRequest.Headers.Add(headerName, headerValue);
            }
        }

        private void SetRequestContent<K>(HttpRequestMessage httpRequest, APIRequest<K> apiRequest)
        {
            httpRequest.Content = new StringContent(
                JsonConvert.SerializeObject(apiRequest.Data),
                Encoding.Unicode,
                "application/json"
            );
        }
    }
}