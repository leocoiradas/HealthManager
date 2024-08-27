using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Authentication;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace HealthManager.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly HealthManagerContext _dbcontext;
        private readonly IJWTService _jwtservice;
        public AuthorizeController(HealthManagerContext context, IJWTService jwtservice) {
            _dbcontext = context;
            _jwtservice = jwtservice;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var existingCookie = ControllerContext.HttpContext.Request.Cookies["Token"];
            if (existingCookie != null)
            {
                return RedirectToAction("Appointment", "Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AuthorizeRequest request)
        {
            if (ModelState.IsValid)
            {
                Patient patientSearch = _dbcontext.Patients.FirstOrDefault(p => p.Email == request.email);
                if (patientSearch == null || !BCrypt.Net.BCrypt.Verify(request.password, patientSearch.Password))
                {
                    ModelState.AddModelError("Email", "Cannot find an account with the provided email.");
                    return View(request);
                }
                string createdToken = _jwtservice.GenerateToken(patientSearch.Name, patientSearch.Email);
                HttpContext.Response.Cookies.Append("Token", createdToken,
                    new CookieOptions
                    {
                        Path = "/",
                        Expires = DateTime.UtcNow.AddDays(1),
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                    });
                return RedirectToAction("Appointments", "Index");
            }
                return View(request);
                

            
            
        }

        [HttpGet]
        public IActionResult Register() 
        {
            var existingCookie = ControllerContext.HttpContext.Request.Cookies["Token"];
            if (existingCookie != null)
            {
                return RedirectToAction("Appointment", "Index");
            }
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(Patient patientData)
        {
            if (ModelState.IsValid) 
            {
                Patient checkPatientExists = await _dbcontext.Patients.FindAsync(patientData);
                if (checkPatientExists == null)
                {
                    patientData.Password = BCrypt.Net.BCrypt.HashPassword(patientData.Password);
                    await _dbcontext.Patients.AddAsync(patientData);
                    await _dbcontext.SaveChangesAsync();
                    return RedirectToAction("Login");

                }
                else 
                {
                    ModelState.AddModelError("Email", "A patient with this email already exists.");
                    return View(patientData);
                }

            }
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
            return View(patientData);
        }
    }
}
