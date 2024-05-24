using System;
using System.Text;
using System.Web;

namespace PXWeb.Modules
{
    /// <summary>
    /// HTTP Module to handle application events in PX-Web
    /// </summary>
    public class PXWebHttpModule : IHttpModule
    {
        private static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(PXWebHttpModule));

        /// <summary>
        /// Register the HTTP Module for HttpApplication events
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.Error += new System.EventHandler(OnError);
        }

        /// <summary>
        /// Handle errors
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        public void OnError(object obj, EventArgs args)
        {
            // At this point we have information about the error
            HttpContext ctx = HttpContext.Current;

            Exception exception = ctx.Server.GetLastError();

            StringBuilder errorInfo = new StringBuilder();
            errorInfo.Append("PX-Web Application Error --->");
            errorInfo.Append(" Offending URL: " + ctx.Request.Url.ToString());
            errorInfo.Append(" -- Source: " + exception.Source);
            errorInfo.Append(" -- Message: " + exception.Message);
            if (exception.InnerException != null)
            {
                errorInfo.Append(" -- Inner exception message: " + exception.InnerException.Message);
            }
            errorInfo.Append(" -- Stack trace: " + exception.StackTrace);

            try
            {
                Logger.ErrorFormat(errorInfo.ToString());
            }
            catch
            {
                // Do nothing
            }

            //ctx.Response.Write (errorInfo);

            // --------------------------------------------------
            // To let the page finish running we clear the error
            // --------------------------------------------------
            //ctx.Server.ClearError ();
        }

        /// <summary>
        /// Clean up
        /// </summary>
        public void Dispose()
        {
        }

    }
}
