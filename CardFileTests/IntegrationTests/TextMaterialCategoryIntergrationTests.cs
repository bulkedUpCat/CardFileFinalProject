using Core.DTOs;
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
    public class TextMaterialCategoryIntergrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/textMaterials/categories/";

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task TextMaterialCategoryController_GetCategories_ReturnsAllCategories()
        {
            // Arrange
            var expected = ExpectedTextMaterialCategoryDTOs;

            // Act
            var httpResponse = await _client.GetAsync(RequestUri);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TextMaterialCategoryDTO>>(stringResponse).ToList();
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextMaterialCategoryController_GetCategory_ReturnsSingleValue(int id)
        {
            // Arrange
            var expected = ExpectedTextMaterialCategoryDTOs.FirstOrDefault(tmc => tmc.Id == id);

            // Act
            var httpResponse = await _client.GetAsync(RequestUri + id);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextMaterialCategoryDTO>(stringResponse);
            actual.Should().BeEquivalentTo(expected);

        }

        [Test]
        public async Task TaskTextMaterialCategoryController_GetCategory_ReturnsNotFound()
        {
            // Arrange
            var id = -19921;

            // Act
            var httpResponse = await _client.GetAsync(RequestUri + id);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task TextMaterialCategoryController_PostCategory_AddsCustomerToDatabase()
        {
            // Arrange
            var textMaterialCategory = new CreateTextMaterialCategoryDTO
            {
                Title = "New category"
            };
            var content = new StringContent(JsonConvert.SerializeObject(textMaterialCategory), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var textMaterialCategoryInResponse = JsonConvert.DeserializeObject<TextMaterialCategoryDTO>(stringResponse);
            Assert.IsNotNull(textMaterialCategoryInResponse);
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public async Task TextMaterialCategoryController_PostCategory_ReturnsBadRequestIfModelInvalid(string title)
        {
            // Arrange
            var textMaterialCategory = new CreateTextMaterialCategoryDTO
            {
                Title = title
            };
            var content = new StringContent(JsonConvert.SerializeObject(textMaterialCategory), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri, content);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextMaterialCategoryController_DeleteCategory_RemovesTextMaterialCategoryFromDatabase(int id)
        {
            // Arrange
            var expectedLength = ExpectedTextMaterialCategoryDTOs.Count() - 1;

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

        [Test]
        public async Task TextMaterialCategoryController_DeleteCategory_ReturnBadRequestIfIdInvalid()
        {
            // Arrange
            var id = -1000;

            // Act
            var httpResponse = await _client.DeleteAsync(RequestUri + id);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public readonly IEnumerable<TextMaterialCategoryDTO> ExpectedTextMaterialCategoryDTOs =
            new List<TextMaterialCategoryDTO>
            {
                new TextMaterialCategoryDTO { Id = 1, Title = "First one" },
                new TextMaterialCategoryDTO { Id = 2, Title = "Second one" }
            };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
