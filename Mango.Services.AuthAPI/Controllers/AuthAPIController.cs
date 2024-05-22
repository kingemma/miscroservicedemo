using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("Register")]
        public async Task<ResponseDto> Register(RegisterrationRequestDto registerrationRequestDto)
        {
            var result = await _authService.RegisterAsync(registerrationRequestDto);
            if (string.IsNullOrEmpty(result))
            {
                return new ResponseDto() { IsSuccess = true };
            }
            else
            {
                return new ResponseDto() { IsSuccess = false, Message = result };
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);
            if (result == null)
            {
                var response= new ResponseDto() { IsSuccess = false, Message="Username or password is incorrect." };
                return BadRequest(response);
            }
            var responseSuccess = new ResponseDto() { IsSuccess = true, Result= result };
            return Ok(responseSuccess);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegisterrationRequestDto loginRequestDto)
        {
            var assignRoleSuccessful = await _authService.AssignRoleAsync(loginRequestDto.Email, loginRequestDto.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                var result = new ResponseDto() { IsSuccess = false, Message = "Error encountered" };
                return BadRequest(result);
            }
            return Ok(new ResponseDto() { IsSuccess = true });
        }
    }
}
