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
                return RedirectToAction("MyAppointments", "PatientDashboard");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(AuthorizeRequest request)
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
                Patient patientAccount = _dbcontext.Patients.FirstOrDefault(x => x.Email == request.email);

                if (patientAccount == null)
                {
                    ViewData["AuthorizeResult"] = "* There's no account associated to the provided email.";
                    return View(request);
                }
                if (!BCrypt.Net.BCrypt.Verify(request.password, patientAccount.Password))
                {
                    ViewData["AuthorizeResult"] = "* Invalid credentials.";
                    return View(request);
                }
                List <Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, patientAccount.PatientId.ToString()),
                    new Claim(ClaimTypes.Name, patientAccount.Name),
                    new Claim(ClaimTypes.Surname, patientAccount.Surname),
                    new Claim(ClaimTypes.Email, patientAccount.Email),
                    new Claim(ClaimTypes.Role, patientAccount.Role)
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,

                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
                return RedirectToAction("MyAppointments", "PatientDashboard");
            }
            else
            {
                ViewData["AuthorizeResult"] = "* There was an error during the process of authentication. We suggest you try again later.";
                return View(request);
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
                return RedirectToAction("MyAppointments", "PatientDashboard");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Register(PatientViewModel patientData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Patient checkPatientExists = _dbcontext.Patients.FirstOrDefault(x => x.Email == patientData.Email);
                    if (checkPatientExists == null)
                    {
                        Patient newPatient = new Patient
                        {
                            Name = patientData.Name,
                            Surname = patientData.Surname,
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
                        ModelState.AddModelError("Email", "* The provided email is already in use.");
                        return View(patientData);
                    }

                }
                
                return View(patientData);
            }
            catch (Exception error)
            {

                ModelState.AddModelError(string.Empty, "* An error occurred while processing your request. Please try again.");
                Console.WriteLine(error);
                return View(patientData);
            }
        }
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AuthorizeRequest request)
        {
            var employee = _dbcontext.Doctors.Where(d => d.Email == request.email).FirstOrDefault(); 
            var admin = _dbcontext.Doctors.Where(d => d.Email == "hola123").FirstOrDefault();
            
           if (employee == null && admin != null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.password, admin.Password))
                {
                    string adminToken = _jwtservice.GenerateToken(employee.Name, admin.Email);
                    HttpContext.Response.Cookies.Append("Token", adminToken,
                        new CookieOptions
                        {
                            Path = "/",
                            Expires = DateTime.UtcNow.AddDays(1),
                            HttpOnly = true,
                            Secure = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.None,
                        });
                    return RedirectToAction("Admin", "AppointmentsManager");
                    
                }
            } else if (employee != null && admin == null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.password, employee.Password))
                {
                    string employeeToken = _jwtservice.GenerateToken(employee.Name, admin.Email);
                    HttpContext.Response.Cookies.Append("Token", employeeToken,
                        new CookieOptions
                        {
                            Path = "/",
                            Expires = DateTime.UtcNow.AddDays(1),
                            HttpOnly = true,
                            Secure = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.None,
                        });
                    return RedirectToAction("Doctor", "Index");
                }
            }
            
            ModelState.AddModelError("Credentials", "The credentials are invalid. Please try again.");
            return View(request);
            
           
           
            
        }
    }
}
