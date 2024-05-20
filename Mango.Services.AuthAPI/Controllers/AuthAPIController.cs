using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);
            return result;
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegisterrationRequestDto loginRequestDto)
        {
            var assignRoleSuccessful = await _authService.AssignRoleAsync(loginRequestDto.Email,loginRequestDto.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                var result= new ResponseDto() { IsSuccess = false,Message="Error encountered" };
                return BadRequest(result);
            }
            return Ok(new ResponseDto() { IsSuccess = true });
        }
    }
}
