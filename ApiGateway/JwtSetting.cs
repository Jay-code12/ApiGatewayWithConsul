namespace ApiGateway
{
    public class JwtSetting
    {
        public required string Secret { get; set; }
        public string? Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
