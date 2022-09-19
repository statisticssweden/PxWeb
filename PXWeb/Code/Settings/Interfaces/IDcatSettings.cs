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
        /// Title of the catalog
        /// </summary>
        string CatalogTitle { get; }

        /// <summary>
        /// Description of the catalog
        /// </summary>
        string CatalogDescription { get; }

        /// <summary>
        /// Publisher of the catalog
        /// </summary>
        string Publisher { get; }

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
    }
}
