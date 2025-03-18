using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class AuthorizeRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(12) ,RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-+]).{12,}$")]
        public string Password { get; set; } = null!;
    }
}
