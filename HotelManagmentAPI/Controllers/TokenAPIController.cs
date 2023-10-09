using System.Security.Claims;
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
        public async Task<IActionResult> RefreshTokenAsync(TokenDTO tokenDto)
        {
            var accessToken = tokenDto.AccessToken;
            var refreshToken = tokenDto.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity?.Name;
            var userRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var oldToken = await _tokenRepo.GetAsync(t => t.UserName == userName && t.IsActive);
            if (oldToken == null || oldToken.RefreshToken != refreshToken || oldToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Refresh token not found or date expired");
            }

            oldToken.IsActive = false;
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
            return Ok(tokenDto);
        }

        [Authorize]
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Revoke()
        {
            var userName = User.Identity?.Name;
            var token = await _tokenRepo.GetAsync(t => t.UserName == userName && t.IsActive);
            if (token == null)
            {
                return BadRequest("Active token not found");
            }

            token.IsActive = false;
            await _tokenRepo.UpdateAsync(token);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("revokeAll")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RevokeAllAsync()
        {
            var tokens = await _tokenRepo.GetAllAsync();
            foreach (var token in tokens)
            {
                token.IsActive = false;
            }

            await _tokenRepo.UpdateRangeAsync(tokens);
            return NoContent();
        }
    }
}
