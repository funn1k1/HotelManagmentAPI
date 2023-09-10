using System.Net;
using HotelManagmentAPI.Models.DTO.Hotel;

namespace HotelManagmentAPI
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public T? Result { get; set; }

        public List<string> ErrorMessages { get; set; }

        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }

        public APIResponse(T result, HttpStatusCode statusCode)
        {
            Result = result;
            StatusCode = statusCode;
            IsSuccess = statusCode >= HttpStatusCode.OK && statusCode < HttpStatusCode.MultipleChoices;
            ErrorMessages = new List<string>();
        }

        public bool IsSuccessStatusCode()
        {
            return IsSuccess && StatusCode == HttpStatusCode.OK;
        }

        public void AddErrorMessage(string message)
        {
            ErrorMessages.Add(message);
        }

        public static implicit operator APIResponse<T>(APIResponse<List<HotelDTO>> v)
        {
            throw new NotImplementedException();
        }
    }
}
