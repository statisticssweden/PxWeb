using System;
using System.Configuration;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PXWeb.API
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(AuthenticationFilter));
        const string KEYNAME = "APIKey";
        string _key;


        public AuthenticationFilter()
        {
            _key = ConfigurationManager.AppSettings.Get(KEYNAME);
            if (string.IsNullOrWhiteSpace(_key)) 
            {
                _key = Environment.GetEnvironmentVariable(KEYNAME); 
            }
            if (string.IsNullOrEmpty(_key))
            {
                _logger.Error($"Could not retrieve environmental variable with key: '{KEYNAME}'");
                throw new ArgumentException("APIKey is not set to environment variables.");
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            bool isAuthenticated = actionContext.Request.Headers.Contains(KEYNAME) && actionContext.Request.Headers.GetValues(KEYNAME).First().Equals(_key);
            if (string.IsNullOrEmpty(_key))
            {
                _logger.Error($"Environmental variable with key: '{KEYNAME}' not set properly.");
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
            if (!isAuthenticated)
            {
                _logger.Error("Authentication failed. Check environmental variables and make sure that the client appends APIKey in headers.");
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }

    }
}