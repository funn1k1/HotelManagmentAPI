using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Account;
using HotelManagment_API.Repository.Interfaces;
using HotelManagment_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersionNeutral]
    [ApiController]
    public class TokenAPIController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepo;

        public TokenAPIController(ITokenService tokenService, ITokenRepository tokensRepo)
        {
            _tokenService = tokenService;
            _tokenRepo = tokensRepo;
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshTokenAsync(TokenDTO tokenDto)
        {
            try
            {
                var apiResponse = new APIResponse<TokenDTO>();
                var accessToken = tokenDto.AccessToken;
                var refreshToken = tokenDto.RefreshToken;

                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                var userName = principal.Identity?.Name;
                var userRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                var oldToken = await _tokenRepo.GetAsync(t => t.UserName == userName && t.IsActive);
                if (oldToken == null)
                {
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    apiResponse.ErrorMessages.Add("Refresh token not found");
                    return BadRequest(apiResponse);
                }

                if (oldToken.RefreshToken != refreshToken)
                {
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    apiResponse.ErrorMessages.Add("Refresh tokens do not match");
                    return BadRequest(apiResponse);
                }

                if (oldToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    oldToken.IsActive = false;
                    await _tokenRepo.UpdateAsync(oldToken);
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    apiResponse.ErrorMessages.Add("Refresh token is expired");
                    return BadRequest(apiResponse);
                }

                oldToken.IsActive = false;
                await _tokenRepo.UpdateAsync(oldToken);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, userRole)
                };
                var newAccessToken = _tokenService.GenerateAccessToken(claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                var newToken = new Token
                {
                    UserName = userName,
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                };
                await _tokenRepo.AddAsync(newToken);

                tokenDto.AccessToken = newAccessToken;
                tokenDto.RefreshToken = newRefreshToken;
                apiResponse.Result = tokenDto;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                return Ok(apiResponse);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while refreshing the token");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevokeTokenAsync()
        {
            try
            {
                var apiResponse = new APIResponse<Token>();
                var userName = User.Identity?.Name;
                var token = await _tokenRepo.GetAsync(t => t.UserName == userName && t.IsActive);
                if (token == null)
                {
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    apiResponse.AddErrorMessage("Active token not found");
                    return BadRequest(apiResponse);
                }

                token.IsActive = false;
                await _tokenRepo.UpdateAsync(token);

                apiResponse.Result = token;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                return Ok(apiResponse);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while revoking the token");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("revokeAll")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RevokeAllTokensAsync()
        {
            try
            {
                var tokens = await _tokenRepo.GetAllAsync();
                foreach (var token in tokens)
                {
                    if (token.IsActive)
                    {
                        token.IsActive = false;
                    }
                }

                await _tokenRepo.UpdateRangeAsync(tokens);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while revoking all tokens");
            }
        }
    }
}
