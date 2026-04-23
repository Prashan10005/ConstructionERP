namespace ConstructionERP.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
