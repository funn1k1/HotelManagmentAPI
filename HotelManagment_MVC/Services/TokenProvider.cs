using HotelManagment_MVC.Models.DTO.Account;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_Utility;

namespace HotelManagment_MVC.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public TokenProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public TokenDTO GetToken()
        {
            if (_accessor.HttpContext != null && _accessor.HttpContext.Request.Cookies.TryGetValue(Constants.AccessToken, out var token))
            {
                return new TokenDTO { Token = token };
            };
            return new TokenDTO();
        }

        public void SetToken(string token)
        {
            var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(15) };
            _accessor.HttpContext?.Response.Cookies.Append(Constants.AccessToken, token, cookieOptions);
        }

        public void ClearToken()
        {
            _accessor.HttpContext?.Response.Cookies.Delete(Constants.AccessToken);
        }
    }
}
