using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Menu settings
    /// </summary>
    public interface IMenuSettings
    {
        /// <summary>
        /// Describes how Menu shall be displayed
        /// </summary>
        MenuModeType MenuMode { get; }

        /// <summary>
        /// Number of values taht will be selected when using default view
        /// </summary>
        int NumberOfValuesInDefaultView { get; }

        /// <summary>
        /// If the root node shall be displayed or not
        /// </summary>
        bool ShowRoot { get; }

        /// <summary>
        /// If the tree shall expand automatically when it is in tree mode
        /// </summary>
        bool ExpandAll { get; }

        /// <summary>
        /// If the nodes should be sorted by alias or not
        /// </summary>
        bool SortByAlias { get; }

        /// <summary>
        /// If the link for selecting variables and values shall be displayed or not
        /// </summary>
        bool ShowSelectLink { get; }

        /// <summary>
        /// If the download link shall be displayed or not
        /// </summary>
        PCAxis.Web.Controls.DownloadLinkVisibilityType ShowDownloadLink { get; }

        /// <summary>
        /// Describes action for the "View" link
        /// </summary>
        MenuViewLinkModeType ViewLinkMode { get; }

        /// <summary>
        /// If the modified date shall be displayed or not
        /// </summary>
        bool ShowModifiedDate { get; }

        /// <summary>
        /// If the last updated date shall be displayed or not
        /// </summary>
        bool ShowLastUpdated { get; }

        /// <summary>
        /// If the file size shall be displayed or not
        /// </summary>
        bool ShowFileSize { get; }

        /// <summary>
        /// If table category shall be displayed or not
        /// </summary>
        bool ShowTableCategory { get; }

        /// <summary>
        /// Show if the table has been updated after it was published or not
        /// </summary>
        bool ShowTableUpdatedAfterPublish { get; }

        /// <summary>
        /// If variables and values shall be displayed or not
        /// </summary>
        bool ShowVariablesAndValues { get; }

        /// <summary>
        /// If table metadata shall be displayed as icons or text
        /// </summary>
        bool MetadataAsIcons { get; }

        /// <summary>
        /// If table metadata shall be displayed as text shall the descriptive text for the metadata be displayd or not
        /// </summary>
        bool ShowTextToMetadata { get; }

        /// <summary>
        /// If additional explanation to the tables/menu shall be displayed as text or not visible at all
        /// </summary>
        bool ShowMenuExplanation { get; }

    }
}
