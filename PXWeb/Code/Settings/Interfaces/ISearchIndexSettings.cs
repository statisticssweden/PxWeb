using PCAxis.Search;

namespace PXWeb
{
    /// <summary>
    /// Interface for seacrh index settings
    /// </summary>
    public interface ISearchIndexSettings
    {
        /// <summary>
        /// Status of the search index
        /// </summary>
        SearchIndexStatusType Status { get; }

        /// <summary>
        /// Time when the search index was updated
        /// </summary>
        string IndexUpdated { get; }

        /// <summary>
        /// Implementation for updating the table
        /// </summary>
        ISearchIndex UpdateMethod { get; }
    }
}
