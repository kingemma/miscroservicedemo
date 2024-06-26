﻿namespace Mango.Services.AuthAPI.Models.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string? Name { get; set; }
        public string? Firstname { get; set; }
        public string? LastName { get; set; }
    }
}
