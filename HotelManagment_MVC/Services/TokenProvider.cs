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
            if (_accessor.HttpContext == null)
            {
                return new TokenDTO();
            }

            if (_accessor.HttpContext.Request.Cookies.TryGetValue(Constants.AccessToken, out var accessToken) &&
                _accessor.HttpContext.Request.Cookies.TryGetValue(Constants.RefreshToken, out var refreshToken))
            {
                return new TokenDTO { AccessToken = accessToken, RefreshToken = refreshToken };
            };

            if (_accessor.HttpContext.Request.Cookies.TryGetValue(Constants.RefreshToken, out refreshToken))
            {
                return new TokenDTO { RefreshToken = refreshToken };
            }

            return new TokenDTO();
        }

        public void SetToken(TokenDTO tokenDto)
        {
            //var accessCookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(1) };
            //var refreshCookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(1) };
            _accessor.HttpContext?.Response.Cookies.Append(Constants.AccessToken, tokenDto.AccessToken);
            _accessor.HttpContext?.Response.Cookies.Append(Constants.RefreshToken, tokenDto.RefreshToken);
        }

        public void DeleteToken()
        {
            _accessor.HttpContext?.Response.Cookies.Delete(Constants.AccessToken);
            _accessor.HttpContext?.Response.Cookies.Delete(Constants.RefreshToken);
        }
    }
}