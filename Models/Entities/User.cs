using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage ="Username is Required")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }
    }
}
