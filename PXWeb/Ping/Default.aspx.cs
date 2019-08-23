using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace PxApi.Console.Ping
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public static string AssemblyVersion
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                var displayVersion = version.Major + "." + version.Minor + "." + version.Build;

                if (version.Revision > 0)
                {
                    displayVersion += "." + version.Revision;
                }

                return displayVersion;
            }
        }
    }
}