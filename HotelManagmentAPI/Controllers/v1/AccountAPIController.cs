using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagment_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountAPIController(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(UserRegisterDTO registerDto)
        {
            var apiResponse = new APIResponse<UserDTO>();
            var user = await _userManager.FindByNameAsync(registerDto.UserName);
            if (user != null)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Username already exists");
                return BadRequest(apiResponse);
            }

            user = _mapper.Map<ApplicationUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Failed to create a user");
                return BadRequest(apiResponse);
            }

            result = await _userManager.AddToRoleAsync(user, registerDto.Role);
            if (!result.Succeeded)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Failed to add a role");
                return BadRequest(apiResponse);
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Role = registerDto.Role
            };

            apiResponse.Result = userDto;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            return Ok(apiResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync(UserLoginDTO loginDto)
        {
            var apiResponse = new APIResponse<string>();
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Username does not exist");
                return BadRequest(apiResponse);
            }

            var isValidPass = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidPass)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Password is not correct");
                return BadRequest(apiResponse);
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };
            apiResponse.Result = GenerateToken(userDto);
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            return Ok(apiResponse);
        }

        private string GenerateToken(UserDTO user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(_configuration["Jwt:SecurityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}