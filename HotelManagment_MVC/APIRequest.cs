using HotelManagment_Utility.Enums;

namespace HotelManagment_MVC
{
    public class APIRequest<T>
    {
        public APIHttpMethod Method { get; set; }

        public T? Data { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public string Url { get; set; }
    }
}
