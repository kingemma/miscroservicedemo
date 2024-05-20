using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);
            if (result != null && result.IsSuccess)
            {
                TempData["success"] = "Login Successful";
                return RedirectToPage("/Coupon/CouponIndex");
            }
            return View();
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

        public IActionResult Logout()
        {
            return View();
        }
    }
}
