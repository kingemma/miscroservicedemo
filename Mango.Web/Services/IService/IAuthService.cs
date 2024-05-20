using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Services.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registerrationRequestDto);
        Task<ResponseDto?> LoginAsync(LoginRequestDto registerrationRequestDto);
        Task<ResponseDto?> RegisterAsync(RegisterationRequestDto registerrationRequestDto);
    }
}