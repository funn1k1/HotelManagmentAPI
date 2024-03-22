using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using HotelManagment_API.Models;
using HotelManagment_API.Models.DTO.Account;
using HotelManagment_API.Repository.Interfaces;
using HotelManagment_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersionNeutral]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepo;
        private readonly IMapper _mapper;

        public AccountAPIController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, ITokenRepository tokenRepo, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _tokenRepo = tokenRepo;
            _mapper = mapper;
        }

        [HttpGet("fileError")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FileError()
        {
            throw new FileNotFoundException();
        }

        [HttpGet("imageError")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImageError()
        {
            throw new BadImageFormatException();
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

            var roleExists = await _roleManager.RoleExistsAsync(registerDto.Role);
            if (!roleExists)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.AddErrorMessage("This role does not exist");
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync(UserLoginDTO loginDto)
        {
            try
            {
                var apiResponse = new APIResponse<TokenDTO>();
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

                var oldToken = await _tokenRepo.GetAsync(t => t.UserName == loginDto.UserName && t.IsActive);
                if (oldToken != null)
                {
                    oldToken.IsActive = false;
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                var newToken = new Token
                {
                    UserName = user.UserName,
                    AccessToken = _tokenService.GenerateAccessToken(claims),
                    RefreshToken = _tokenService.GenerateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
                };
                await _tokenRepo.AddAsync(newToken);

                apiResponse.Result = new TokenDTO
                {
                    AccessToken = newToken.AccessToken,
                    RefreshToken = newToken.RefreshToken
                };
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                return Ok(apiResponse);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while adding the token");
            }
        }
    }
}