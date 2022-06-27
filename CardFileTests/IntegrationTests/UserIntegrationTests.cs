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
    public class UserIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/users/";

        [Test]
        public async Task UserController_Get_ReturnsAllUsers()
        {
            // Arrange
            var expected = ExpectedUserDTOs.ToList();

            // Act
            var httpResponse = await _client.GetAsync(RequestUri);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringReponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(stringReponse);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        public async Task UserController_GetById_ReturnsSingleValue(string id)
        {
            // Arrange
            var expected = ExpectedUserDTOs.FirstOrDefault(u => u.Id == id);

            // Act
            var httpResponse = await _client.GetAsync(RequestUri + id);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserDTO>(stringResponse);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public async Task UserController_GetById_ReturnsBadRequestIfIdInvalid(string id)
        {
            // Act
            var httpResponse = await _client.GetAsync(RequestUri + id);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCase("1")]
        public async Task UserController_Get_ReturnsTextMaterialsOfTheUser(string userId)
        {
            // Arrange
            var expected = ExpectedUserDTOs.FirstOrDefault(u => u.Id == userId).TextMaterials;

            // Act
            var httpResponse = await _client.GetAsync(RequestUri + userId + "/textMaterials/");

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TextMaterialDTO>>(stringResponse);
            actual.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        private static readonly IEnumerable<User> ExpectedUserEntities =
            new List<User>
            {
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com" },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com" },
                new User { Id = "3", UserName = "Bobby", Email = "bobby@gmail.com" }
            };

        private IEnumerable<UserDTO> ExpectedUserDTOs =>
            new List<UserDTO>
            {
                new UserDTO { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com", TextMaterials = new List<TextMaterialDTO> { GetTextMaterialDTOs.First() } },
                new UserDTO { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com", TextMaterials = new List<TextMaterialDTO> {GetTextMaterialDTOs.FirstOrDefault(tm => tm.Id == 2), GetTextMaterialDTOs.FirstOrDefault(tm => tm.Id == 3) } },
                new UserDTO { Id = "3", UserName = "Bobby", Email = "bobby@gmail.com", TextMaterials = new List<TextMaterialDTO>() }
            };

        private IEnumerable<TextMaterialDTO> GetTextMaterialDTOs =>
            new List<TextMaterialDTO>
            {
                new TextMaterialDTO { Id = 1, AuthorId = "1", UserName = "Tommy", ApprovalStatusId = 0, CategoryTitle = "First one", Content = "firstContent", Title = "firstArticle", DatePublished = new DateTime(2000,3,12) },
                new TextMaterialDTO { Id = 2, AuthorId = "2", UserName = "Johnny", ApprovalStatusId = 1, CategoryTitle = "First one", Content = "secondContent", Title = "secondArticle", DatePublished = new DateTime(2003, 4,23) },
                new TextMaterialDTO { Id = 3, AuthorId = "2", UserName = "Johnny", ApprovalStatusId = 1, CategoryTitle = "Second one", Content = "thirdContent", Title = "thirdArticle", DatePublished = new DateTime(2004,1,1) }
            };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
