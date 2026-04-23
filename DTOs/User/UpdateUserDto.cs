namespace ConstructionERP.DTOs.User
{
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; } // optional
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
