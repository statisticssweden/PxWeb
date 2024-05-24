namespace PXWeb
{
    //Interface for settings concerning a specific database
    public interface IDatabaseSettings
    {
        /// <summary>
        /// Home pages settings
        /// </summary>
        IHomepagesSettings Homepages { get; }

        /// <summary>
        /// Search index settings
        /// </summary>
        ISearchIndexSettings SearchIndex { get; }

        /// <summary>
        /// Dcat settings
        /// </summary>
        IDcatSettings Dcat { get; }

        /// <summary>
        /// The database protection
        /// </summary>
        IProtection Protection { get; }

        /// <summary>
        /// Metadata settings
        /// </summary>
        IMetadataSettings Metadata { get; }
    }
}
