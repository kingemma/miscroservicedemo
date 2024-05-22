namespace Mango.Web.Services.IService
{
    public interface ITokenProvider
    {
        void ClearToken();
        string? GetToken();
        void SetToken(string token);
    }
}