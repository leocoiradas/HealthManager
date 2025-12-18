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
    public class LoginTesting: IClassFixture<CustomWebApplicationEnv>
    {
        private HttpClient _httpClient;
        private readonly HealthManagerContext _dbContext;
        private readonly IJWTService _tokenService;

        public LoginTesting(CustomWebApplicationEnv custom)
        {
            _httpClient = custom.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var scope = custom.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
            _tokenService = scope.ServiceProvider.GetRequiredService<IJWTService>();
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


    }
}
