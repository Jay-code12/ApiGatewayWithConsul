using AuthService.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using AuthService.Modal.Dto;
using AuthService.Modal.data;
using Mapster;

namespace AuthService.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<Auth> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly UserServiceClient _userClient;

        public AuthRepository(
            UserManager<Auth> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            UserServiceClient userClient)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userClient = userClient;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            var user = request.Adapt<Auth>();
            // Generate ID before calling microservice
            user.Id = Guid.NewGuid().ToString();

            var userProfileRequest = request.Adapt<UserServiceDto>();
            userProfileRequest.Id = user.Id;

            var userServiceResponse =
                await _userClient.CreateUserProfileAsync(userProfileRequest);

            if (userServiceResponse == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = new List<string>
                    {
                        "User service returned null"
                    }
                };
            }

            if (!userServiceResponse.Success)
                return userServiceResponse;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = result.Errors
                        .Select(e => e.Description)
                        .ToList()
                };
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _tokenService.CreateToken(user);

            userServiceResponse.Token = token;

            return userServiceResponse;
        }
    
        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = new List<string>
                    {
                        "Invalid Login Details."
                    }
                };
            }

            var token = await _tokenService.CreateToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Token = $"{token} //: {user.Id}"
            };
        }
    }
}