using Core.DTOs;
using Core.Models;
using DAL.Contexts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
    public class TextMaterialIntergrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/textMaterials/";

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task TextMaterialContoller_Get_ReturnsAllValues()
        {
            // Arrange
            var expected = ExpectedTextMaterialDTOs;

            // Act
            var httpResponse = await _client.GetAsync(RequestUri);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TextMaterialDTO>>(stringResponse);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TextMaterialController_GetById_ReturnsSingleValue(int id)
        {
            // Arrange
            var expected = ExpectedTextMaterialDTOs.FirstOrDefault(tm => tm.Id == id);

            // Act
            var httpResponse = await _client.GetAsync(RequestUri + id);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextMaterialDTO>(stringResponse);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task TextMaterialController_GetById_ReturnsNotFoundIfIdInvalid()
        {
            // Arrange
            var id = -1212;

            // Act
            var httpResponse = await _client.GetAsync(RequestUri + id);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task TextMaterialController_Post_AddsTextMaterialToDatabase()
        {
            // Arrange
            var textMaterial = new CreateTextMaterialDTO
            {
                AuthorId = "1",
                CategoryTitle = "First one",
                Content = "New one",
                Title = "Fake title"
            };
            var content = new StringContent(JsonConvert.SerializeObject(textMaterial), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextMaterialDTO>(stringResponse);
            Assert.IsNotNull(actual);
        }

        [TestCase(" ", "content")]
        [TestCase("abcd", "content")]
        [TestCase("Valid title", "   ")]
        public async Task TextMaterialController_Post_ReturnsBadRequestIfModelInvalid(string title, string content)
        {
            // Arrange
            var textMaterial = new CreateTextMaterialDTO
            {
                AuthorId = "1",
                Title = title,
                CategoryTitle = "First one",
                Content = content
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(textMaterial), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri, stringContent);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task TextMaterialController_Approve_ApprovesTextMaterial()
        {
            // Arrange
            var id = 1;

            // Act
            var httpResponse = await _client.PutAsync(RequestUri + id + "/approve", null);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<AppDbContext>();
                Assert.AreEqual(context.TextMaterials.First().ApprovalStatus, ApprovalStatus.Approved);
            }
        }

        [Test]
        public async Task TextMaterialController_Approve_ReturnsBadRequestIfTextMaterialAlreadyApproved()
        {
            // Arrange
            var id = 2;

            // Act
            var httpResponse = await _client.PutAsync(RequestUri + id + "/approve", null);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task TextMaterialController_Reject_RejectsTextMaterial()
        {
            // Arrange
            var id = 1;
            var content = new StringContent(JsonConvert.SerializeObject(new { RejectMessage = "fakeMessage" }), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PutAsync(RequestUri + id + "/reject", content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<AppDbContext>();
                Assert.AreEqual(context.TextMaterials.First().ApprovalStatus, ApprovalStatus.Rejected);
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TextMaterialController_Delete_DeletesTextMaterial(int id)
        {
            // Arrange
            var expectedLength = ExpectedTextMaterialDTOs.Count() - 1;

            // Act
            var httpResponse = await _client.DeleteAsync(RequestUri + id);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<AppDbContext>();
                context.TextMaterialCategory.Should().HaveCount(expectedLength);
            }
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task TextMaterialController_Delete_ReturnsBadRequestIfIdInvalid(int id)
        {
            // Act
            var httpResponse = await _client.DeleteAsync(RequestUri + id);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task TextMaterialController_Reject_ReturnsBadRequestIfTextMaterialAlreadyApproved()
        {
            // Arrange
            var id = 3;
            var content = new StringContent(JsonConvert.SerializeObject(new { RejectMessage = "fakeMessage" }), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PutAsync(RequestUri + id + "/reject", content);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public readonly IEnumerable<TextMaterialDTO> ExpectedTextMaterialDTOs =
            new List<TextMaterialDTO>
            {
                new TextMaterialDTO { Id = 1, AuthorId = "1", UserName = "Tommy", ApprovalStatusId = 0, Content = "firstContent", Title = "firstArticle", CategoryTitle = "First one", DatePublished = new DateTime(2000,3,12) },
                new TextMaterialDTO { Id = 2, AuthorId = "2", UserName = "Johnny", ApprovalStatusId = 1, Content = "secondContent", Title = "secondArticle", CategoryTitle = "First one", DatePublished = new DateTime(2003, 4,23) },
                new TextMaterialDTO { Id = 3, AuthorId = "2", UserName = "Johnny", ApprovalStatusId = 1, Content = "thirdContent", Title = "thirdArticle", CategoryTitle = "Second one", DatePublished = new DateTime(2004,1,1) }
            };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
