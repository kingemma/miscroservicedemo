using Mango.Web.Models.Dto;

namespace Mango.Web.Services.IService
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto);
    }
}
