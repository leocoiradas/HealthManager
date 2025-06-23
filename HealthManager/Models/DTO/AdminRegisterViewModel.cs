using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class AdminRegisterViewModel
    {

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage ="The maximum of characters for this field is 50")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "The maximum of characters for this field is 50")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        [MinLength(12, ErrorMessage = "The minimum characters for this field is 12")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-+]).{12,}$")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        [Compare("Password", ErrorMessage ="The passwords must be the same")]
        public string ConfirmPassword { get; set; } = null!;

        public string Role { get; set; } = "Admin";
    }
}
