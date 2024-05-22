using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);
            if (result != null && result.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(result.Result.ToString());
                _tokenProvider.SetToken(loginResponseDto.Token);
                SignInUser(loginResponseDto);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", result.Message);
                return View(loginRequestDto);
            }
        }


        public IActionResult Register()
        {
            AssignRoleDropdownItems();
            return View();
        }

        private void AssignRoleDropdownItems()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text=SD.RoleAdmin},
                new SelectListItem(){ Text=SD.RoleCustom}
            };
            ViewBag.RoleList = roleList;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto registerrationRequestDto)
        {
            var registerSuccessful = await _authService.RegisterAsync(registerrationRequestDto);
            if (registerSuccessful != null && registerSuccessful.IsSuccess)
            {
                if (string.IsNullOrEmpty(registerrationRequestDto.Role))
                {
                    registerrationRequestDto.Role = SD.RoleCustom;
                }
                var assignRoleSuccessful = await _authService.AssignRoleAsync(registerrationRequestDto);
                if (assignRoleSuccessful != null && assignRoleSuccessful.IsSuccess)
                {
                    TempData["success"] = "Registeration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            AssignRoleDropdownItems();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponseDto.Token);
            var identity = new ClaimsIdentity(jwt.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
