using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class PatientViewModel
    {
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        [MinLength(1)]
        public string Surname { get; set; } = null!;
        [Required]
        public DateOnly Birthdate { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(12), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
        public string Password { get; set; } = null!;
        [Required]
        public int Dni { get; set; }
        [Required, Compare("Password"), MinLength(12), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
