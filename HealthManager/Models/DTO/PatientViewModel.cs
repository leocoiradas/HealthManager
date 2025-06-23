using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class PatientViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(100)]
        [MinLength(1)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(100)]
        [MinLength(1)]
        public string Surname { get; set; } = null!;
        [Required(ErrorMessage = "This field is required.")]
        public DateOnly Birthdate { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "This field is required.")]
        [MinLength(12)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-+]).{12,}$")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "This field is required.")]
        public int Dni { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Compare("Password", ErrorMessage ="Passwords do not match.")]
        [MinLength(12)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*+-]).{12,}$")]
        public string ConfirmPassword { get; set; } = null!;
        public long? PhoneNumber { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(1)]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(1)]
        public string? Sex { get; set; }
    }
}
