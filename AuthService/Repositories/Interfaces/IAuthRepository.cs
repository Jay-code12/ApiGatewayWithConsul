using AuthService.Modal.data;
using AuthService.Modal.Dto;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthService.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto request);
        Task<AuthResponseDto> LoginAsync(LoginDto request);
    }
}
