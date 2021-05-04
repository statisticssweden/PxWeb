using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PXWeb.API
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        const string KEYNAME = "APIKey";
        string _key;


        public AuthenticationFilter()
        {
            _key = Environment.GetEnvironmentVariable(KEYNAME);
            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentException("APIKey is not set to environment variables.");
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            bool isAuthenticated = actionContext.Request.Headers.Contains(KEYNAME) && actionContext.Request.Headers.GetValues(KEYNAME).First().Equals(_key);
            if (string.IsNullOrEmpty(_key))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
            if (!isAuthenticated)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }

    }
}