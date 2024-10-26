using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;
using BupaCodeAssesment.Controllers;
using BupaCodeAssesment.Services;
using BupaCodeAssesment.Models;

namespace BupaCodeAssesment.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<HttpClientService> _httpClientServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _httpClientServiceMock = new Mock<HttpClientService>();
            _controller = new HomeController();
            var fieldInfo = typeof(HomeController).GetField("_httpClientService", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(_controller, _httpClientServiceMock.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsCorrectCategories_WhenValidResponse()
        {
            // Arrange
            var owners = new List<BookOwner>
            {
                new BookOwner { Age = 13, Books = new List<Book> { new Book { Name = "Great Expectations" } } },
                new BookOwner { Age = 17, Books = new List<Book> { new Book { Name = "Little Red Riding Hood, The Hobbit" } } },
                new BookOwner { Age = 25, Books = new List<Book> { new Book { Name = "React: The Ultimate Guide,Gulliver's Travels, Jane Eyre,Great Expectations" } } },
            };
            _httpClientServiceMock.Setup(s => s.GetBookOwnersAsync(It.IsAny<string>())).ReturnsAsync(owners);

            // Act
            var result = await _controller.IndexAsync();

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Dictionary<string, List<string>>;
            Assert.IsNotNull(model);
            Assert.IsTrue(model.ContainsKey("Adults"));
            Assert.IsTrue(model.ContainsKey("Children"));
        }

        [TestMethod]
        public async Task Index_ReturnsEmpty_WhenNullResponse()
        {
            // Arrange
            _httpClientServiceMock.Setup(s => s.GetBookOwnersAsync(It.IsAny<string>())).ReturnsAsync((List<BookOwner>)null);

            // Act
            var result = await _controller.IndexAsync();

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Dictionary<string, List<string>>;
            Assert.AreEqual(0, model.Count);
        }
    }
}

