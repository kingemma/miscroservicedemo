using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService ?? throw new ArgumentNullException(nameof(baseService));
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterationRequestDto registerrationRequestDto)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = registerrationRequestDto,
                Url = $"{Utility.SD.AuthApiBase}/api/AuthAPI/Register"
            });

            return result;
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto registerrationRequestDto)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = registerrationRequestDto,
                Url = $"{Utility.SD.AuthApiBase}/api/AuthAPI/Login"
            });

            return result;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registerrationRequestDto)
        {
            var result = await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = registerrationRequestDto,
                Url = $"{Utility.SD.AuthApiBase}/api/AuthAPI/AssignRole"
            });

            return result;
        }
    }
}
