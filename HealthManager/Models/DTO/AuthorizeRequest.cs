using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class AuthorizeRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
