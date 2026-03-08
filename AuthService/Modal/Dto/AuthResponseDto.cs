namespace AuthService.Modal.Dto
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public List<string> Message { get; set; }
    }
}
