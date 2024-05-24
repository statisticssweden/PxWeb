﻿using System;
using System.Web;

namespace PXWeb.Modules.Security
{
    /// <summary>
    /// HTTP Module to restrict access by IP address
    /// </summary>
    public class SecurityHttpModule : IHttpModule
    {
        /// <summary>
        /// Clean up
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Register the HTTP Module for HttpApplication events
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
        }

        /// <summary>
        /// Handler for the BeginRequest event
        /// </summary>
        /// <param name="source">Source of the event</param>
        /// <param name="e">Parameters</param>
        private void Application_BeginRequest(object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            if (PXWeb.Settings.Current.General.Administration.UseIPFilter)
            {
                // Only protect the Administration part of PX-Web (both UI and REST API)
                var path = context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
                if (path.StartsWith("~/admin/") || path.StartsWith("~/api/admin"))
                {
                    string ipAddress = context.Request.UserHostAddress;
                    if (!IsValidIpAddress(ipAddress))
                    {
                        application.CompleteRequest(); // Abort processing
                        context.Response.StatusCode = 403;  // (Forbidden)
                        context.Response.StatusDescription = "Forbidden";
                    }
                }
            }
        }

        /// <summary>
        /// Function for validating the IP address
        /// </summary>
        /// <param name="ipAddress">The IP address to validate</param>
        /// <returns>True if the IP address is valid, else false</returns>
        private bool IsValidIpAddress(string ipAddress)
        {
            foreach (string ip in PXWeb.Settings.Current.General.Administration.IPAddresses)
            {
                if (ipAddress.StartsWith(ip))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
