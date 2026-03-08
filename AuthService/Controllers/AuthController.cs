using AuthService.Modal.data;
using AuthService.Modal.Dto;
using AuthService.Repositories.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepo.RegisterAsync(request);

            if (result == null)
                return StatusCode(500, "Auth service returned null");

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepo.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result.Token);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            return Ok();
        }
    }
}
