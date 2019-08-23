using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace PXWeb.Security
{
    public class DefaultAuthorization : PX.Security.IAuthorization 
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DefaultAuthorization));

        public bool IsAuthorized(string dbid, string menu, string selection)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Default authurization authorized " + HttpContext.Current.User.Identity.Name);
            }
            return true;
        }


        public bool IsAuthorized(string dbid)
        {
            return true;
        }
    }
}