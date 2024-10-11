using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

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
            /*var existingCookie = ControllerContext.HttpContext.Request.Cookies["Token"];
            if (existingCookie != null)
            {
                return RedirectToAction("ReserveAppointment", "Appointment");
            }*/
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ReserveAppointment", "Appointment");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async IActionResult Login(AuthorizeRequest request)
        {
           /* if (ModelState.IsValid)
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
                return View(request);*/
                
            if (ModelState.IsValid)
            {
                Patient patientAccount = await  _dbcontext.FindAsync<Patient>(request.email);

                if (patientAccount == null)
                {

                }
                if (!BCrypt.Net.BCrypt.Verify(request.password, patientAccount.Password))
                {

                }
                List <Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, patientAccount.PatientId.ToString()),
                    new Claim(ClaimTypes.Name, patientAccount.Name),
                    new Claim(ClaimTypes.Surname, patientAccount.Surname),
                    new Claim(ClaimTypes.Email, patientAccount.Email)
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,

                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
                return RedirectToAction("MyAppointments", "Appointment");
            }

            
            
        }

        [HttpGet]
        public IActionResult Register() 
        {
            /*var existingCookie = ControllerContext.HttpContext.Request.Cookies["Token"];
            if (existingCookie != null)
            {
                return RedirectToAction("Appointment", "Index");
            }*/
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ReserveAppointment", "Appointment");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Register(PatientViewModel patientData)
        {
            if (ModelState.IsValid) 
            {
                Patient checkPatientExists = await _dbcontext.Patients.FindAsync(patientData.Email);
                if (checkPatientExists == null)
                {
                    Patient newPatient = new Patient
                    {
                        Name= patientData.Name,
                        Surname= patientData.Surname,
                        Birthdate = patientData.Birthdate,
                        Email = patientData.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(patientData.Password),
                        Dni = patientData.Dni,
                    };
                    await _dbcontext.Patients.AddAsync(newPatient);
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
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
