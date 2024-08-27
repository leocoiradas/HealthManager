using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthManager.Services.JWTService
{
    public class JWTService: IJWTService
    {
        private readonly HealthManagerContext _dbcontext;
        private IConfiguration _configuration;

        public JWTService(HealthManagerContext context, IConfiguration configuration)
        {
            _dbcontext = context;
            _configuration = configuration;

        }
        public string GenerateToken(string username, string email)
        {
            var secret = _configuration.GetSection("JWT").GetSection("secret-key").ToString();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
            ]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject= claims,
                Expires= DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token =  tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        
    }
}
