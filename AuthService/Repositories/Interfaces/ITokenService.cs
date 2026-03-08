using AuthService.Modal.data;

namespace AuthService.Repositories.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(Auth user);
    }
}
