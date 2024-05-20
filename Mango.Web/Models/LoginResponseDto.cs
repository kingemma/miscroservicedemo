using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Models
{
    public class LoginResponseDto : ResponseDto
    {
        public UserDto? User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
