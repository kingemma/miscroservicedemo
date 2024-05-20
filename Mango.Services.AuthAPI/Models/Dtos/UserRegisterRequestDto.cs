namespace Mango.Services.AuthAPI.Models.Dtos
{
    public class RegisterrationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmPassword { get; set; }

        public string? Name { get; set; }
        public string? Firstname { get; set; }
        public string? LastName { get; set; }

        public string? Role { get; set; }

    }
}
