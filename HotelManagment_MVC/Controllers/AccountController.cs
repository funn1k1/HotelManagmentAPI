using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelManagment_API.Models.DTO.Account;
using HotelManagment_MVC.Services.Interfaces;
using HotelManagment_MVC.ViewModels.Account;
using HotelManagment_Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagment_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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

            var apiResponse = await _accountService.Login<string, UserLoginDTO>(loginVM.Account);
            if (!apiResponse.IsSuccess)
            {
                AddModelErrors(apiResponse.ErrorMessages);
                return View(loginVM);
            }

            var token = new JwtSecurityTokenHandler().ReadJwtToken(apiResponse.Result);
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

            HttpContext.Session.SetString(Constants.JwtToken, apiResponse.Result);
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
            registerVM.Account.Roles = Constants.Roles.Select(roleName => new SelectListItem
            {
                Text = roleName,
                Value = roleName
            });
            if (!ModelState.IsValid)
            {
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove(Constants.JwtToken);
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
