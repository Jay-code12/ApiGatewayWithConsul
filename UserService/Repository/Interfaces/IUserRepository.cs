using UserService.Modal.Data;
using UserService.Modal.Dto;

namespace UserService.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetProfileAsync(string id);

        Task<UserResponseDto> CreateProfileAsync(User profile);

        Task<UserResponseDto> UpdateProfileAsync(string userId, UpdateDto updateDto);
    }
}
