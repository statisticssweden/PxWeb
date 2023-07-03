using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXWeb
{
    public interface IDcatLanguageSpecificSettings
    {
        /// <summary>
        /// Language for these settings
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Title of the catalog
        /// </summary>
        string CatalogTitle { get; }

        /// <summary>
        /// Description of the catalog
        /// </summary>
        string CatalogDescription { get; }

    }
}
