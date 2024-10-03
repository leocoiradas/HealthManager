using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class AuthorizeRequest
    {
        [Required, EmailAddress]
        public string email {  get; set; }
        [Required, MinLength(12) ,RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
        public string password { get; set; }
    }
}
