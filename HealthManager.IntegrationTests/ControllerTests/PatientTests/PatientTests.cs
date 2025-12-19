using Docker.DotNet.Models;
using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Appointments;
using HealthManager.Services.Authentication;
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

namespace HealthManagerIntegrationTests.ControllerTests.PatientTests

{
    public class PatientTests : IClassFixture<CustomWebApplicationEnv>
    {
        private HttpClient _httpClient;
        private readonly HealthManagerContext _dbContext;
        private readonly IAppointments _appointmentsService;
        private readonly IJWTService _jwtService;

        public PatientTests(CustomWebApplicationEnv custom)
        {
            _httpClient = custom.CreateClient();
            var scope = custom.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
            _appointmentsService = scope.ServiceProvider.GetRequiredService<IAppointments>();
            _jwtService = scope.ServiceProvider.GetRequiredService<IJWTService>();
        }

        [Fact]
        public async Task PatientCannotAccessDoctorMethods()
        {
            //Arrange

            Patient patientTest = _dbContext.Patients.Where(x => x.PatientId == 1).FirstOrDefault();
            string patientToken = _jwtService.GenerateToken(patientTest.Name, patientTest.Email, patientTest.Role, patientTest.PatientId);
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", patientToken);

            //Act

            var request = await _httpClient.GetAsync("/Doctor/PatientTodayList");

            //Assert

            Assert.Equal(HttpStatusCode.Forbidden, request.StatusCode);
        }

    }
}
