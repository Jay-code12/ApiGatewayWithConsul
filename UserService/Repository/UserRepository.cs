using Microsoft.EntityFrameworkCore;
using UserService.Modal;
using UserService.Modal.Data;
using UserService.Modal.Dto;
using UserService.Repository.Interfaces;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetProfileAsync(string id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<UserResponseDto> CreateProfileAsync(User profile)
        {
            try
            {
                var existing = await _context.Users.FirstOrDefaultAsync(x => x.Id == profile.Id);

                if (existing != null)
                {
                    return new UserResponseDto
                    {
                        Success = false,
                        Message = new List<string> { "Profile already exists." }
                    };
                }

                _context.Users.Add(profile);
                await _context.SaveChangesAsync();

                return new UserResponseDto
                {
                    Success = true,
                    Message = new List<string> { "Profile created successfully." }
                };
            }
            catch (Exception ex)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = new List<string> { ex.Message }
                };
            }
        }

        public async Task<UserResponseDto> UpdateProfileAsync(string id, UpdateDto updateDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new UserResponseDto
                    {
                        Success = false,
                        Message = new List<string> { "User not found." }
                    };
                }
                user.FirstName = updateDto.FirstName;
                user.LastName = updateDto.LastName;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new UserResponseDto
                {
                    Success = true,
                    Message = new List<string> { "updated successfully." }
                };
            }
            catch (Exception ex)
            {
                return new UserResponseDto
                {
                    Success = false,
                    Message = new List<string> { ex.Message }
                };
            }
        }
    }
}
