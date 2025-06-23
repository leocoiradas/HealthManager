using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class DoctorRegistrationViewModel
    {
        [Required(ErrorMessage = "This field is required"), MaxLength(100), MinLength(2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required"), MaxLength(100), MinLength(2)]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Specialty { get; set; }
        [Required(ErrorMessage = "This field is required"), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required"), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-+]).{12,}$")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required"), Compare("Password")]
        public string ConfirmPassword {  get; set; }
        [Required(ErrorMessage = "This field is required")]  
        public TimeOnly WorkingHoursStart { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public TimeOnly WorkingHoursEnd { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int ConsultDuration { get; set; }
        [Required]
        public bool Monday { get; set; }

        [Required]
        public bool Tuesday { get; set; }

        [Required]
        public bool Wednesday { get; set; }

        [Required]
        public bool Thursday { get; set; }

        [Required]
        public bool Friday { get; set; }

        [Required]
        public bool Saturday { get; set; }

        [Required]
        public bool Sunday { get; set; }
    }
}
