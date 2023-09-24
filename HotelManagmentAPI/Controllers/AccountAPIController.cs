using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Account;
using HotelManagment_API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountAPIController(IUserRepository userRepo, IMapper mapper, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDto)
        {
            var apiResponse = new APIResponse<User>();
            var user = await _userRepo.GetAsync(u => u.UserName.ToLower() == registerDto.UserName.ToLower());
            if (user != null)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Username already exists");
                return BadRequest(apiResponse);
            }

            user = _mapper.Map<User>(registerDto);
            await _userRepo.AddAsync(user);

            apiResponse.Result = user;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            return Ok(apiResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDto)
        {
            var apiResponse = new APIResponse<string>();
            var user = await _userRepo.GetAsync(u => u.UserName == loginDto.UserName && u.Password == u.Password);
            if (user == null)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("Username or password is incorrect");
                return BadRequest(apiResponse);
            }

            apiResponse.Result = GenerateToken(user);
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            return Ok(apiResponse);
        }

        private string GenerateToken(User user)
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