using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class PatientViewModel: Patient
    {
        [Required, Compare("Password"), MinLength(12), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
        public string ConfirmPassword { get; set; }
    }
}
