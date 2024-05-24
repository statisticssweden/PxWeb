namespace PXWeb.Database
{

    /// <summary>


    /// <summary>
    /// Interface to implement so that you can hock into the building menu process 
    /// for file based databases
    /// </summary>
    public interface IDatabaseBuilder
    {
        /// <summary>
        /// The priority of the builder. A builder with a high priority (small number) 
        /// will be invoked before a builder with a smaller priority 
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// This method will be called by the DatabaseSpider to inform it that a new
        /// build process has started
        /// </summary>
        /// <param name="path">The root path to the database</param>
        void BeginBuild(string path, DatabaseLogger logger);

        /// <summary>
        /// This method will be called by the DatabaseSpider to inform it that the current
        /// build process has ended
        /// </summary>
        /// <param name="path">The root path to the database</param>
        void EndBuild(string path);

        /// <summary>
        /// This method will be called by the DatabaseSpider to inform the builder that it
        /// has reached a new folder in the file system
        /// </summary>
        /// <param name="path">The path to the folder</param>
        /// <remarks>Notice that the DatabaseSpider works recursively depth first</remarks>
        void BeginNewLevel(string path);

        /// <summary>
        /// This method will be called by the DatabaseSpider to inform the builder that it
        /// has steped back one level in the tree hierarchy of the file system
        /// </summary>
        /// <param name="path">The path to the folder</param>
        /// <remarks>Notice that the DatabaseSpider works recursively depth first</remarks>
        void EndNewLevel(string path);

        /// <summary>
        /// This method will be called by the DatabaseSpider to inform the builder that a
        /// new item/file has be detected and handeled by a handler
        /// </summary>
        /// <param name="item">The handler object</param>
        /// <param name="path">The path to the file</param>
        void NewItem(object item, string path);
    }
}
