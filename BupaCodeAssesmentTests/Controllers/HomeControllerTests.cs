using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using Moq.Protected;
using System.Threading;

namespace BupaCodeAssesment.Controllers.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private HomeController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _controller = new HomeController(_httpClient);
        }

        [TestMethod]
        public async Task Index_ReturnsCorrectBookCategories_WhenApiReturnsSuccess() // Positive Scenario
        {
            // Arrange
            var responseJson = JsonConvert.SerializeObject(new[]
            {
                new { age = 20, books = new[] { new { name = "Great Expectations" }, new { name = "Hamlet" } } },
                new { age = 10, books = new[] { new { name = "Little Red Riding Hood" }, new { name = "The Hobbit" } } },
                new { age = 22, books = new[] { new { name = "Wuthering Heights" }, new { name = "Jane Eyre" } } },
            });

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _controller.Index() as ViewResult;
            var bookCategories = result.Model as Dictionary<string, List<string>>;

            // Assert
            Assert.IsNotNull(bookCategories);
            Assert.IsTrue(bookCategories.ContainsKey("Adults"));
            Assert.IsTrue(bookCategories.ContainsKey("Children"));
            Assert.AreEqual(4, bookCategories["Adults"].Count); // Expected number of distinct adult books
            Assert.AreEqual(2, bookCategories["Children"].Count); // Expected number of distinct children books
        }

        [TestMethod]
        public async Task Index_ReturnsEmptyCategories_WhenApiReturnsFailure() // Negative Scenario
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act
            var result = await _controller.Index() as ViewResult;
            var bookCategories = result.Model as Dictionary<string, List<string>>;

            // Assert
            Assert.IsNotNull(bookCategories);
            Assert.IsFalse(bookCategories.ContainsKey("Adults"));
            Assert.IsFalse(bookCategories.ContainsKey("Children"));
        }
    }
}
