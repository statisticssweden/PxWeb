using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using PXWeb.API;
using System.Net;
using log4net;
using Moq;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {

        private Mock<PXWeb.API.Services.ICacheService> _cacheServiceMock;
        private Mock<ILog> _logger;

        [ClassInitialize]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("APIKey", "foo");
        }
        
        private HttpActionContext InitializeHttpActionContext(string apiKey, bool isApiKey = true)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            if (isApiKey)
                request.Headers.Add("APIKey", apiKey);
            HttpControllerContext controllerContext = new HttpControllerContext()
            {
                Request = request
            };
            HttpActionContext context = new HttpActionContext()
            {
                ControllerContext = controllerContext
            };
            return context;
        }

        [TestMethod]
        public void Call_AuthenticationFilterWithApikey_ReturnsNull()
        {
            // Arrange
            var filter = new AuthenticationFilter();

            var context = InitializeHttpActionContext("foo");

            // Act
            filter.OnActionExecuting(context);

            var response = context.Response;

            // Assert
            Assert.IsNull(response);

        }

        [DataTestMethod]
        //[DataRow("tttt", "")]
        [DataRow("", false)]
        [DataRow("")]
        [DataRow("xyz")]
        public void Call_AuthenticationFilterWithoutApikey_ReturnsForbidden(string apiKey, bool isApiKey = true)
        {
            // Arrange
            var filter = new AuthenticationFilter();
            var context = InitializeHttpActionContext(apiKey, isApiKey);

            // Act
            filter.OnActionExecuting(context);

            var response = context.Response;

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
        
        [TestMethod]
        public void Initialize_AuthenticationFilter_WithoutEnv_ThrowsArgumentException()
        {
            // Arrange
            Environment.SetEnvironmentVariable("APIKey", "");

            // Act
            try
            {
                var filter = new AuthenticationFilter();
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            // Assert
        }
    }
}
