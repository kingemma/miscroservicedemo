using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Models.Dtos
{
    public class LoginResponseDto : ResponseDto
    {
        public UserDto? User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
