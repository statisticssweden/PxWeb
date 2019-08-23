using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using PCAxis.Web.Core.Enums;

namespace PXWeb
{
    /// <summary>
    /// Class for holding information about a database
    /// </summary>
    public class CnmmDatabaseInfo : DatabaseInfo
    {
        private List<string> _otherLanguages;

        /// <summary>
        /// Constructor
        /// </summary>
        public CnmmDatabaseInfo()
        {
            this.Type = DatabaseType.CNMM;
            _otherLanguages = new List<string>();
        }

        /// <summary>
        /// Default language of the CNMM database
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Languages of the CNMM database execpt the default language
        /// </summary>
        public List<string> OtherLanguages
        {
            get
            {
                return _otherLanguages;
            }
        }

    }
}
