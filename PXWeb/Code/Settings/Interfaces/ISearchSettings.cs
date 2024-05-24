namespace PXWeb
{
    /// <summary>
    /// Iterface for search settings
    /// </summary>
    public interface ISearchSettings
    {
        /// <summary>
        /// Time in minutes that the searcher objects will be cached
        /// </summary>
        int CacheTime { get; }

        /// <summary>
        /// Maximum number of tables that will be displayed in the search result list
        /// </summary>
        int ResultListLength { get; }

        /// <summary>
        /// The operator AND/OR that will be used by default if more than one word is specified in a search query
        /// </summary>
        PCAxis.Search.DefaultOperator DefaultOperator { get; }
    }
}
