using Moq;
using NUnit.Framework;
using PXWeb.API;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Controllers;
using NUnit.Framework.Legacy;

namespace PxWeb.Test
{
    public class AuthenticationFilterTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("APIKey", "foo");
        }

        private HttpActionContext InitializeHttpActionContext(string apiKey, bool isApiKey = true)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            if(isApiKey)
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

        [Test]
        public void Call_AuthenticationFilterWithApikey_ReturnsNull()
        {
            // Arrange
            var filter = new AuthenticationFilter();

            var context = InitializeHttpActionContext("foo");

            // Act
            filter.OnActionExecuting(context);

            var response = context.Response;

            // Assert
            ClassicAssert.IsNull(response);

        }

        [TestCase("", false)]
        [TestCase("")]
        [TestCase("xyz")]
        public void Call_AuthenticationFilterWithoutApikey_ReturnsForbidden(string apiKey, bool isApiKey = true)
        {
            // Arrange
            var filter = new AuthenticationFilter();       
            var context = InitializeHttpActionContext(apiKey, isApiKey);

            // Act
            filter.OnActionExecuting(context);

            var response = context.Response;

            // Assert
            ClassicAssert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
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