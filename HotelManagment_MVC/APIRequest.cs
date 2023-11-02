using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace HotelManagment_MVC
{
    public class APIRequest<T>
    {
        public HttpMethod Method { get; set; }

        public T? Data { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public string Url { get; set; }
    }
}
