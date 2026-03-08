using AuthService.Modal.Dto;
using Mapster;
using System.Text.Json;

namespace AuthService.Repositories
{
    public class UserServiceClient
    {
        private readonly ConsulServiceDiscovery _discovery;
        private readonly HttpClient _httpClient;

        public UserServiceClient(ConsulServiceDiscovery discovery, HttpClient httpClient)
        {
            _discovery = discovery;
            _httpClient = httpClient;
        }

        public async Task<AuthResponseDto> CreateUserProfileAsync(UserServiceDto dto)
        {
            var service = await _discovery.GetServiceAsync("UserService");

            if (service == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = new List<string> { "UserService not available" }
                };
            }

            var url = $"http://{service.Service.Address}:{service.Service.Port}/api/User";

            var response = await _httpClient.PostAsJsonAsync(url, dto);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = new List<string>
                    {
                        $"UserService error: {response.StatusCode}"
                    }
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = new List<string> { "UserService returned empty response" }
                };
            }

            var userResponse =
                JsonSerializer.Deserialize<UserServiceResponseDto>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (userResponse == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = new List<string> { "Failed to deserialize UserService response" }
                };
            }

            return userResponse.Adapt<AuthResponseDto>();
        }
    }
}
