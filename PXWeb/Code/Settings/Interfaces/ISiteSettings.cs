namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Site settings
    /// </summary>
    public interface ISiteSettings
    {
        /// <summary>
        /// Application name
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Logo path
        /// </summary>
        string LogoPath { get; }

        /// <summary>
        /// Main header (H1) type for table pages
        /// </summary>
        MainHeaderForTablesType MainHeaderForTables { get; }

        /// <summary>
        /// Show link to external search or not
        /// </summary>
        bool ShowExternalSearchLink { get; }

    }
}
