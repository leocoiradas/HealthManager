using DotNet.Testcontainers.Builders;
using HealthManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using HealthManager.Services.Authentication;
using HealthManager.Services.JWTService;
using HealthManager.Models.DTO;
using HealthManager.Services.Appointments;

namespace HealthManagerIntegrationTests
{
    public class CustomWebApplicationEnv : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer;

        public CustomWebApplicationEnv() 
        {
            _dbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("YourStrong(!)Password")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("MSSQL_PID", "Express")
                .WithPortBinding(1433, true)
                .Build();
        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var scope = Services.CreateScope();

           
                var db = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
                var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointments>();
                db.Database.EnsureCreated();


                if (!db.Specialties.Any())
                {
                    var neu = new Specialty { SpecialtyName = "Neurología" };
                    await db.Specialties.AddAsync(neu);
                    await db.SaveChangesAsync();
                }
                if (!db.Doctors.Any())
                {
                    var newDoctor = new Doctor
                    {
                        Name = "Mariano",
                        Surname = "Benitez",
                        Specialty = 1,
                        Email = "marianobenitez@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("DefaultPass0213++"),
                        Role ="Doctor"
                    };
                    await db.Doctors.AddAsync(newDoctor);
                    await db.SaveChangesAsync();

                    var wd = new WorkingDay
                    {
                        DoctorId = 1,
                        Monday = true,
                        Tuesday = true,
                        Wednesday = true,
                        Thursday = true,
                        Friday = true,
                        Saturday = true,
                        Sunday = true
                    };
                    await db.WorkingDays.AddAsync(wd);
                    await db.SaveChangesAsync();

                    var doctorShift = new DoctorShift
                    {
                        DoctorId = 1,
                        ShiftStart = new TimeOnly(10, 00),
                        ShiftEnd = new TimeOnly(18, 30),
                        ConsultDuration = new TimeOnly(00, 20)
                    };
                    await db.DoctorShifts.AddAsync(doctorShift);

                    await db.SaveChangesAsync();


                    var response = await appointmentService.CreateAppointmentsForAllDoctors();



                }

                if (!db.Patients.Any())
                {
                    Patient newPatient = new Patient
                    {
                        Name = "Alicia",
                        Surname = "Rodriguez",
                        Dni = 23456789,
                        BirthDate = new DateOnly(1980, 11, 05),
                        Gender = "F",
                        Sex = "F",
                        Email = "aliciarodriguez@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("Rodriguez02!!"),
                        Role = "Patient"
                    };
                    await db.Patients.AddAsync(newPatient);
                    await db.SaveChangesAsync();
                }
            
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            
            builder.ConfigureServices(services => 
            {
                services.AddScoped<IJWTService, JWTService>();
                services.AddScoped<IAppointments, AppointmentsService>();
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HealthManagerContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HealthManagerContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));

                

            });

        }
    }
}
