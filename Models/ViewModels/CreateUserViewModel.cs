using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage ="Username is Required")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [StringLength(100, MinimumLength =8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Role is Required")]
        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
