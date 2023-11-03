using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Chart;

namespace PXWeb
{
    /// <summary>
    /// Interface for the dcat settings
    /// </summary>
    public interface IDcatSettings
    {
        /// <summary>
        /// Base uri for referencing in the xml file
        /// </summary>
        string BaseURI { get; }

        /// <summary>
        /// Base url for the api
        /// </summary>
        string BaseApiUrl { get; }

        /// <summary>
        /// Base url for the landing page
        /// </summary>
        string LandingPageUrl { get; }

        /// <summary>
        /// Settings that are different for each language
        /// </summary>
        IEnumerable<IDcatLanguageSpecificSettings> LanguageSpecificSettings { get; }

        /// <summary>
        /// Database to be generated
        /// </summary>
        string Database { get; }


        /// <summary>
        /// Type of database (cnmm or px)
        /// </summary>
        string DatabaseType { get; }


        /// <summary>
        /// License for referencing in each dataset
        /// </summary>
        string License { get; }

        string FileUpdated { get; }

        DcatStatusType FileStatus { get; }
    }
}
