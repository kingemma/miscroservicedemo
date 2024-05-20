namespace Mango.Services.AuthAPI.Models
{
    public class JwtOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Issue { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public int ExpiryInMinutes { get; set; }
    }
}
