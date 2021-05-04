using Moq;
using NUnit.Framework;
using PXWeb.API;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace PxWeb.Test
{
    public class CacheControllerTests
    {

        private Mock<PXWeb.API.Services.ICacheService> _cacheServiceMock;
        private Mock<log4net.ILog> _logger;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("APIKey", "foo");
        }

        [SetUp]
        public void Setup()
        {
            _cacheServiceMock = new Mock<PXWeb.API.Services.ICacheService>();
            _logger = new Mock<log4net.ILog>();
        }

        private CacheController InitializeCacheController()
        {
            var controller =  new CacheController(_cacheServiceMock.Object, _logger.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.ControllerContext.Request.Headers.Add("APIKey", "foo");

            return controller;
        }

        [TestCase(CacheController.CacheType.ApiCache)]
        [TestCase(CacheController.CacheType.InMemoryCache)]
        [TestCase(CacheController.CacheType.SavedQueryPaxiomCache)]
        public void Delete_WithAllowedCacheType_ReturnsHttpStatusCode204(CacheController.CacheType cacheType)
        {
            // Arrange
            var controller = InitializeCacheController();

            // Act
            var response = controller.Delete(cacheType);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public void Delete_DependenciesFail_ReturnsHttpStatusCode500()
        {
            // Arrange
            _cacheServiceMock.Setup(c => c.ClearCache(It.IsAny<Type>())).Throws<Exception>();
            CacheController controller = InitializeCacheController();

            // Act
            var response = controller.Delete(CacheController.CacheType.ApiCache);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);

        }

        [Test]
        public void Delete_WithAllowedCacheType_CallsCacheService()
        {
            // Arrange
            CacheController controller = InitializeCacheController();

            // Act
            var response = controller.Delete(CacheController.CacheType.ApiCache);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            _cacheServiceMock.Verify(mock => mock.ClearCache(It.IsAny<Type>()), Times.Once());

        }

        // TODO: Test unauthorized
        // 
        
    }
}