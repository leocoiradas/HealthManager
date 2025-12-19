using Docker.DotNet.Models;
using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Appointments;
using HealthManager.Services.Authentication;
using HealthManager.Services.JWTService;
using HealthManagerIntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HealthManagerIntegrationTests.ControllerTests.DoctorTests
{
    public class DoctorTests : IClassFixture<CustomWebApplicationEnv>
    {
        private HttpClient _httpClient;
        private readonly HealthManagerContext _dbContext;
        private readonly IAppointments _appointmentsService;
        private readonly IJWTService _jwtService;

        public DoctorTests(CustomWebApplicationEnv custom)
        {
            _httpClient = custom.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions 
            {
                AllowAutoRedirect = false
            });
            var scope = custom.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
            _appointmentsService = scope.ServiceProvider.GetRequiredService<IAppointments>();
            _jwtService = scope.ServiceProvider.GetRequiredService<IJWTService>();
        }

        [Fact]
        public async Task AppointmentsForADcotorAreSuccessfullyCreated()
        {
            //Arrange
            Doctor doctorTest = _dbContext.Doctors.Where(x => x.DoctorId == 1).Single();
            WorkingDay wd = _dbContext.WorkingDays.Where(x => x.DoctorId == 1).Single();
            DoctorShift ds = _dbContext.DoctorShifts.Where(x => x.DoctorId == 1).Single();

            DoctorDTO dto = new DoctorDTO { Doctor = doctorTest, DoctorShift = ds, WorkingDay = wd};
            List<DoctorDTO> doctorList = new List<DoctorDTO> {dto};


            //Act
            MethodResponse response = await _appointmentsService.CreateDoctorAppointments(doctorList, 1);

            //Assert
            Assert.Equal(true, response.Success);


        }

    }
}
