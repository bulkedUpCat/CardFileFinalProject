using Core.DTOs;
using Core.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.IntegrationTests
{
    public class AuthIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/auth/";

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task AuthController_LogIn_ReturnsBadRequestIfUserNotFound()
        {
            // Arrange
            var user = new UserLoginDTO
            {
                Email = "tommy@gmail.com",
                Password = "Tommyspass18@"
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri + "login", content);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task AuthController_SignUp_ReturnsNewUser()
        {
            // Arrange
            var user = new UserRegisterDTO
            {
                Email = "newUser@gmail.com",
                Name = "newUser",
                Password = "newUser18@",
                ConfirmPassword = "newUser18@"
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri + "signup", content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userInResponse = JsonConvert.DeserializeObject<User>(stringResponse);
            Assert.IsNotNull(userInResponse);
        }

        [Test]
        public async Task AuthController_SignUp_ReturnsBadRequestIfPasswordsDontMatch()
        {
            // Arrange
            var user = new UserRegisterDTO
            {
                Email = "newUser@gmail.com",
                Name = "newUser",
                Password = "newUser18@",
                ConfirmPassword = "newUser"
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri + "signup", content);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        public IEnumerable<User> ExpectedUserEntities =
            new List<User>
            {
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com" },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com" },
                new User { Id = "3", UserName = "Bobby", Email = "bobby@gmail.com" }
            };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
