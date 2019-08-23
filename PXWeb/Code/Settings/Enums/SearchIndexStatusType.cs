namespace PXWeb
{
    /// <summary>
    /// Describes status of the search index
    /// </summary>
    public enum SearchIndexStatusType
    {
        /// <summary>
        /// The search index has not been indexed
        /// </summary>
        NotIndexed,
        /// <summary>
        /// The search index is indexed
        /// </summary>
        Indexed,
        /// <summary>
        /// The search index is being indexed right now
        /// </summary>
        Indexing,
        /// <summary>
        /// The search index is waiting to be (re)created
        /// </summary>
        WaitingCreate,
        /// <summary>
        /// The search index is waiting to be partially updated
        /// </summary>
        WaitingUpdate
    }
}
