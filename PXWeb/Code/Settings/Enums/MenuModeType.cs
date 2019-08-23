
namespace PXWeb
{
    /// <summary>
    /// Describes how Menu will be displayed
    /// </summary>
    public enum MenuModeType
    {
        /// <summary>
        /// List view (only TableOfContents)
        /// </summary>
        List,
        /// <summary>
        /// Tree view without files (First TableOfContents then TableList)
        /// </summary>
        TreeViewWithoutFiles,
        /// <summary>
        /// Tree view with files only (TableOfContents)
        /// </summary>
        TreeViewWithFiles,
        /// <summary>
        /// Tree view and files (TableOfContents togheter with TableList)
        /// </summary>
        TreeViewAndFiles
    }
}
