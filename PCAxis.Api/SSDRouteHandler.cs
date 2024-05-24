using System.Web;
using System.Web.Routing;

namespace PCAxis.Api
{
    /// <summary>
    /// The route handler for the API
    /// </summary>
    public class SSDRouteHandler : IRouteHandler
    {
        private SSDHandler _ssdHandler;

        public SSDRouteHandler() : this(null)
        {

        }

        public SSDRouteHandler(string defaultResponseFormat)
        {
            _ssdHandler = new SSDHandler(defaultResponseFormat);
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // Store the route data on the requestcontext so we can avoid state class-variables.
            requestContext.HttpContext.Items["RouteData"] = requestContext.RouteData;
            return _ssdHandler;
        }
    }
}