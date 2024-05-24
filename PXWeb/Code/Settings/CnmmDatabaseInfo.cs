using PCAxis.Web.Core.Enums;
using System.Collections.Generic;

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
