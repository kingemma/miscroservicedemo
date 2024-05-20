using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient(requestDto.Url);
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), encoding: Encoding.UTF8, "application/json");
                }

                message.Method = requestDto.ApiType switch
                {
                    Utility.SD.ApiType.GET => HttpMethod.Get,
                    Utility.SD.ApiType.POST => HttpMethod.Post,
                    Utility.SD.ApiType.PUT => HttpMethod.Put,
                    Utility.SD.ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get,
                };

                HttpResponseMessage apiResponse = await httpClient.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        return new ResponseDto() { Message = await apiResponse.Content.ReadAsStringAsync(), IsSuccess = false };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new ResponseDto() { Message = "Unauthorized", IsSuccess = false };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new ResponseDto() { Message = "Forbidden", IsSuccess = false };
                    case System.Net.HttpStatusCode.NotFound:
                        return new ResponseDto() { Message = "Not Found", IsSuccess = false };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new ResponseDto() { Message = "InternalServerError", IsSuccess = false };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto() { Message = ex.Message, IsSuccess = false };
                return dto;
            }
        }
    }
}
