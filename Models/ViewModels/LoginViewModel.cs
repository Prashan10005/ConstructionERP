using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is Required")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
