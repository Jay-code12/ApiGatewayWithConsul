using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Modal;
using UserService.Modal.Data;
using UserService.Modal.Dto;
using UserService.Repository.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ApplicationDbContext _context;

        public UserController(IUserRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var profile = await _repo.GetProfileAsync(id);

            if (profile == null)
                return NotFound("user not found");

            return Ok(profile);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var profile = userDto.Adapt<User>();

            var result = await _repo.CreateProfileAsync(profile);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateProfileAsync(userId, updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
