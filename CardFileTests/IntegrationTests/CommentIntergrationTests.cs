using Core.DTOs;
using DAL.Contexts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.IntegrationTests
{
    public class CommentIntergrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/comments/";

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TestCase(1)]
        [TestCase(3)]
        public async Task CommentController_GetByTextMaterialId_ReturnsCommentsByTextMaterialId(int textMaterialId)
        {
            // Arrange
            var expected = ExpectedCommentDTOs.Where(c => c.TextMaterialId == textMaterialId).ToList();
            
            // Act
            var httpResponse = await _client.GetAsync(RequestUri + "textMaterials/" + textMaterialId);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CommentDTO>>(stringResponse).ToList();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CommentController_Post_AddsCommentToDatabase()
        {
            // Arrange
            var comment = new CreateCommentDTO
            {
                ParentCommentId = null,
                UserId = "1",
                TextMaterialId = 1,
                Content = "new comment"
            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var commentInResponse = JsonConvert.DeserializeObject<CommentDTO>(stringResponse);
            Assert.IsNotNull(commentInResponse);
        }

        [Test]
        public async Task CommentController_Put_UpdatesCommentInDatabase()
        {
            // Arrange
            var comment = new UpdateCommentDTO
            {
                Id = 1,
                Content = "updated content"
            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PutAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<AppDbContext>();
                context.Comments.First().Should().BeEquivalentTo(comment);
            }
        }

        [Test]
        public async Task CommentController_Delete_RemovesCommentFromDatabase()
        {
            // Arrange
            var commentId = 1;
            var expectedLength = ExpectedCommentDTOs.Count() - 1;

            // Act
            var httpResponse = await _client.DeleteAsync(RequestUri + commentId);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<AppDbContext>();
                context.Comments.Should().HaveCount(expectedLength);
            }
        }

        private static readonly IEnumerable<CommentDTO> ExpectedCommentDTOs =
            new List<CommentDTO>
            {
                /*new CommentDTO { Id = 1, Content = "first comment", ParentCommentId = null, UserId = "1", UserName = "Tommy", TextMaterialId = 1, CreatedAt = new DateTime(2000,1,1) },
                new CommentDTO { Id = 2, Content = "second comment", ParentCommentId = null, UserId = "1", UserName = "Tommy", TextMaterialId = 1, CreatedAt = new DateTime(2000,1,2) },*/
                new CommentDTO { Id = 1, Content = "first comment", UserName = "Tommy", UserId = "1", TextMaterialId = 1, CreatedAt = new DateTime(2007, 11, 12), ParentCommentId = null},
                new CommentDTO { Id = 2, Content = "second comment", UserName = "Tommy", UserId = "1", TextMaterialId = 1, CreatedAt = new DateTime(2001,1,23), ParentCommentId = null},
                new CommentDTO { Id = 3, Content = "third comment", UserName = "Johnny", UserId = "2", TextMaterialId = 3, CreatedAt = new DateTime(2000,12,1), ParentCommentId = null } 
            };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
