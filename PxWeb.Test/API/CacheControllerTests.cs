using Moq;
using NUnit.Framework;
using PXWeb.API;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PxWeb.Test
{
    public class CacheControllerTests
    {

        private Mock<PXWeb.API.Services.ICacheService> _cacheServiceMock;

        [SetUp]
        public void Setup()
        {
            _cacheServiceMock = new Mock<PXWeb.API.Services.ICacheService>();
        }

        private CacheController InitializeCacheController()
        {
            return new CacheController(_cacheServiceMock.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
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
    }
}