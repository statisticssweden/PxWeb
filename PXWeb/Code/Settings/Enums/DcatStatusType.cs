namespace PXWeb
{
    /// <summary>
    /// Describes status of dcat file creation
    /// </summary>
    public enum DcatStatusType
    {
        /// <summary>
        /// The dcat file has not been created
        /// </summary>
        NotCreated,
        /// <summary>
        /// The file has been created
        /// </summary>
        Created,
        /// <summary>
        /// The file is being created right now
        /// </summary>
        Creating,
        /// <summary>
        /// The file is waiting to be (re)created
        /// </summary>
        WaitingCreate
    }
}
