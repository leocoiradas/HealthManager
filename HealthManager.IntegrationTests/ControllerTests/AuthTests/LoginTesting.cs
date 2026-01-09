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

namespace HealthManagerIntegrationTests.ControllerTests.AuthTests
{
    [Collection("IntegrationTests")]
    public class LoginTesting: IClassFixture<CustomWebApplicationEnv>
    {
        private HttpClient _httpClient;
        private readonly HealthManagerContext _dbContext;
        private readonly IJWTService _tokenService;
        private readonly IAppointments _appointmentService;

        public LoginTesting(CustomWebApplicationEnv custom)
        {
            _httpClient = custom.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var scope = custom.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
            _tokenService = scope.ServiceProvider.GetRequiredService<IJWTService>();
            _appointmentService = scope.ServiceProvider.GetRequiredService<IAppointments>();
        }


        [Fact]
        public async Task LoginRequestIsSuccessful()
        {
            //Arrange

            AuthorizeRequest request = new AuthorizeRequest
            {
                Email = "aliciarodriguez@gmail.com",
                Password = "Rodriguez02!!"
            };

            //Act

            var requestFormData = AuxMethods.ConvertClassObjectToFormUrlEncoded(request);

            var response = await _httpClient.PostAsync("/Authorize/Login", requestFormData);

            //Assert

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        }


        [Fact]
        public async Task LoginFailWhenAParameterIsMissing()
        {
            //Arrange
            Patient testPatient = new Patient
            {
                Name = "Rosalia",
                Surname = "Benitez",
                BirthDate = new DateOnly(1999, 03, 30),
                Dni = 1234567,
                Email = "Rosalia@gmail.com",
                Password = "Benitez1234!!",
                Gender = "F",
                Sex = "F",
            };

            await _dbContext.Patients.AddAsync(testPatient);
            await _dbContext.SaveChangesAsync();

            AuthorizeRequest request = new AuthorizeRequest
            {
                Email = "Rosalia@gmail.com",
                Password = null
            };
            //Act

            var requestFormData = AuxMethods.ConvertClassObjectToFormUrlEncoded(request);

            var response = await _httpClient.PostAsync("/Authorize/Login", requestFormData);

            string responseString = await response.Content.ReadAsStringAsync();

            //Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("There was an error during the process of authentication. We suggest you try again later", responseString);
        }
    }
}
