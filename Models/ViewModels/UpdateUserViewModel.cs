using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; }

        // if user forgot the password admin can update new password or else can continue with old password
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
