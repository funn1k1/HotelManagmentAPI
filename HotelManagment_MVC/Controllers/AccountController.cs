using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelManagment_API.Models.DTO.Account;
using HotelManagment_MVC.Models;
using HotelManagment_MVC.Models.DTO.Account;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_MVC.ViewModels.Account;
using HotelManagment_Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagment_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly ITokenProvider _tokenProvider;

        public AccountController(IAccountService accountService, ITokenService tokenService, ITokenProvider tokenProvider)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _tokenProvider = tokenProvider;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var apiResponse = await _accountService.Login<TokenDTO, UserLoginDTO>(loginVM.Account);
            if (!apiResponse.IsSuccess)
            {
                AddModelErrors(apiResponse.ErrorMessages);
                return View(loginVM);
            }

            var token = new JwtSecurityTokenHandler().ReadJwtToken(apiResponse.Result.AccessToken);
            var userName = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var roleName = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, roleName)
            };
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            _tokenProvider.SetToken(apiResponse.Result);
            TempData["Success"] = "Success";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Register()
        {
            var registerVM = new RegisterViewModel
            {
                Account = new UserRegisterDTO
                {
                    Roles = Constants.Roles.Select(roleName => new SelectListItem
                    {
                        Text = roleName,
                        Value = roleName
                    })
                }
            };
            return View(registerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                registerVM.Account.Roles = Constants.Roles.Select(roleName => new SelectListItem
                {
                    Text = roleName,
                    Value = roleName
                });
                return View(registerVM);
            }

            var apiResponse = await _accountService.Register<UserDTO, UserRegisterDTO>(registerVM.Account);
            if (!apiResponse.IsSuccess)
            {
                AddModelErrors(apiResponse.ErrorMessages);
                return View(registerVM);
            }

            TempData["Success"] = "The account has been successfully registered";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;
            var apiResponse = await _tokenService.RevokeTokenAsync<Token>(userName);
            if (!apiResponse.IsSuccess)
            {
                AddModelErrors(apiResponse.ErrorMessages);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            await HttpContext.SignOutAsync();
            _tokenProvider.DeleteToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AddModelErrors(IEnumerable<string> errorMessages)
        {
            foreach (var errorMessage in errorMessages)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }
        }
    }
}
