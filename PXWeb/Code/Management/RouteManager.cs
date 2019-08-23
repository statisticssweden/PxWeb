using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Configuration;
using PCAxis.Api;
using PXWeb.Management;
using System.Text;

namespace PXWeb
{
    
    /// <summary>
    /// Class for handling routes in PX-Web
    /// </summary>
    public class RouteManager
    {
        private const string API_PATTERN = "{language}/{*path}";
        
        /// <summary>
        /// Add Route for API calls
        /// </summary>
        public static void AddApiRoute()
        {
            //string routePrefix = ConfigurationManager.AppSettings["routePrefix"];
            string routePrefix = PXWeb.Settings.Current.Features.Api.RoutePrefix;
            if (routePrefix == null)
                throw new Exception("No route prefix set up in app config");

            string url = routePrefix + API_PATTERN;

            if (RouteExists(url))
            {
                return;
            }

            using (RouteTable.Routes.GetWriteLock())
            {
                RouteTable.Routes.Add(new Route
                (
                        url, new SSDRouteHandler(PXWeb.Settings.Current.Features.Api.DefaultExampleResponseFormat)
                ));
            }

        }

        /// <summary>
        /// Adds a default route for goto (shorter links) calls
        /// </summary>
        public static void AddDefaultGotoRoute()
        {
            string url = PxUrl.PX_GOTO + "/" +
                         "{" + PxUrl.LANGUAGE_KEY + "}/" +
                         "{" + PxUrl.DB_KEY + "}/" +
                         "{" + PxUrl.TABLE_KEY + "}/";

            if (RouteExists(url))
            {
                return;
            }

            using (RouteTable.Routes.GetWriteLock())
            {
                RouteTable.Routes.Add(new Route
                (
                        url, new GotoRouteHandler()
                ));
            }

        }

        public interface IGotoManager
        {
            IEnumerable<string> GetOrderedGotoRouteParts();
            IRouteHandler GetGotoHttpdHandler();
        }

        /// <summary>
        /// Adds a default route for goto (shorter links) calls
        /// </summary>
        public static void AddCustomizedGotoRoute(IGotoManager gotoManager)
        {
            StringBuilder urlBuilder = new StringBuilder();

            urlBuilder.Append(PxUrl.PX_GOTO + "/");

            foreach (string gotoRoutePart in gotoManager.GetOrderedGotoRouteParts())
            {
                urlBuilder.Append("{" + gotoRoutePart + "}/");
            }

            string url = urlBuilder.ToString();

            if (RouteExists(url))
            {
                return;
            }
            
            using (RouteTable.Routes.GetWriteLock())
            {
                RouteTable.Routes.Add(new Route(url, new GotoRouteHandler()));
            }

        }

        /// <summary>
        /// Add route for Saved Queries
        /// </summary>
        public static void AddSavedQueryRoute()
        {
            string urlFormat = "SQ/{QueryName}.{Format}/";
            string url = "SQ/{QueryName}/";

            if (RouteExists(url))
            {
                return;
            }

            using (RouteTable.Routes.GetWriteLock())
            {
                RouteTable.Routes.Add(new Route
                (
                        urlFormat, new SavedQueryRouteHandler()
                ));
                RouteTable.Routes.Add(new Route
                (
                        url, new SavedQueryRouteHandler()
                ));
                RouteTable.Routes.Add(new Route
                (
                     "SQ", new SavedQueryRouteHandler()
                ));
            }

        }

        /// <summary>
        /// Check if route is already added
        /// </summary>
        /// <param name="url">Route to check</param>
        /// <returns></returns>
        private static bool RouteExists(string url)
        {
            foreach (Route r in RouteTable.Routes)
            {
                if (r.Url == url)
                {
                    //Route already added
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove route for API calls
        /// </summary>
        public static void RemoveApiRoute()
        {
            string routePrefix = ConfigurationManager.AppSettings["routePrefix"];
            if (routePrefix == null)
                throw new Exception("No route prefix set up in app config");

            string url = routePrefix + API_PATTERN;

            foreach (Route r in RouteTable.Routes)
            {
                if (r.Url == url)
                {
                    using (RouteTable.Routes.GetWriteLock())
                    {
                        RouteTable.Routes.Remove(r);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Change route for API calls
        /// </summary>
        public static void ChangeApiRoute(string newRoute)
        {
        }

    }
}
