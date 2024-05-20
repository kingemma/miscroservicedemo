using static Mango.Web.Utility.SD;

namespace Mango.Web.Models.Dto
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public required string Url { get; set; }
        public string? AccessToken { get; set; }
        public object? Data { get; set; }
    }
}
