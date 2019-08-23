using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PXWeb.Management
{
    public class GotoRouteHandler : IRouteHandler
    {
        private GotoHandler _gotoHandler = new GotoHandler();

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // Store the route data on the requestcontext so we can avoid state class-variables.
            requestContext.HttpContext.Items["RouteData"] = requestContext.RouteData;
            return _gotoHandler;
        }
    }

    public class SavedQueryRouteHandler : IRouteHandler
    {
        private SavedQueryHandler _gotoHandler;
        
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (RouteInstance.RouteExtender == null)
            {
                _gotoHandler = new SavedQueryHandler();
            }
            else
            {
                var language = RouteInstance.RouteExtender.GetLanguageFromUri(requestContext.HttpContext.Request.Url);
                _gotoHandler = new SavedQueryHandler(language);
            }

            // Store the route data on the requestcontext so we can avoid state class-variables.
            requestContext.HttpContext.Items["RouteData"] = requestContext.RouteData;
            return _gotoHandler;
        }
    }
}
