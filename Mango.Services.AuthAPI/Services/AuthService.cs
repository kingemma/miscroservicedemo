using AutoMapper;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper
            , IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        public async Task<bool> AssignRoleAsync(string email, string roleName)
        {
            var userToReturn = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (userToReturn != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(userToReturn, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var userToReturn = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == loginRequestDto.Email);
            if (userToReturn == null)
            {
                return new LoginResponseDto { IsSuccess = false, Message = $"Invalid user name: {loginRequestDto.Email}." };
            }
            bool isPasswordValid = await _userManager.CheckPasswordAsync(userToReturn, loginRequestDto.Password);
            if (!isPasswordValid)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid password.",
                    User = null,
                    Token = string.Empty
                };
            }
            return new LoginResponseDto
            {
                IsSuccess = true,
                Message = "Success",
                User = _mapper.Map<UserDto>(userToReturn),
                Token = _jwtTokenGenerator.GenerateToken(userToReturn)
            };
        }

        public async Task<string> RegisterAsync(RegisterrationRequestDto userRegisterRequestDto)
        {
            try
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = userRegisterRequestDto.Email,
                    Email = userRegisterRequestDto.Email,
                    NormalizedEmail = userRegisterRequestDto.Email.ToUpper(),
                    Name = userRegisterRequestDto.Name,
                    Firstname = userRegisterRequestDto.Firstname,
                    LastName = userRegisterRequestDto.LastName,
                    PhoneNumber = userRegisterRequestDto.PhoneNumber,
                    PhoneNumberConfirmed = true
                };
                var result = await _userManager.CreateAsync(applicationUser, userRegisterRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == userRegisterRequestDto.Email);
                    return "";
                }
                else
                {
                    return string.Join(", ", result.Errors.Select(x => x.Description));
                }
            }
            catch (Exception ex)
            {
                return $"Error Encountered. {ex.Message}";
            }
        }
    }
}
