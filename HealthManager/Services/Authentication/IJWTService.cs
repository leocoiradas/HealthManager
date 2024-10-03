using HealthManager.Models;
using HealthManager.Models.DTO;

namespace HealthManager.Services.Authentication
{
    public interface IJWTService
    {
        public string GenerateToken(string username, string email);
    }
}
