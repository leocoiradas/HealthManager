using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class DoctorRegistrationViewModel
    {
        [Required, MaxLength(100), MinLength(2)]
        public string Name { get; set; }
        [Required, MaxLength(100), MinLength(2)]
        public string? Surname { get; set; }
        [Required, MaxLength(100), MinLength(2)]
        public string Specialty { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword {  get; set; }
        [Required]  
        public TimeOnly WorkingHoursStart { get; set; }
        [Required]
        public TimeOnly WorkingHoursEnd { get; set; }
        [Required]
        public TimeOnly ConsultationDuration { get; set; }
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
