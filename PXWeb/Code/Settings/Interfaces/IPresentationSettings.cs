namespace PXWeb
{
    /// <summary>
    /// Interface for the Presentation settings
    /// </summary>
    public interface IPresentationSettings
    {
        /// <summary>
        /// If all mandatory footnotes shall be displayed the first time the table is shown or not
        /// </summary>
        bool PromptMandatoryFootnotes { get; }

        /// <summary>
        /// If the table Layout is new or old
        /// </summary>
        bool NewTitleLayout { get; }

        /// <summary>
        /// Table settings
        /// </summary>
        ITableSettings Table { get; }

        /// <summary>
        /// CommandBar settings
        /// </summary>
        ICommandBarSettings CommandBar { get; }
    }
}
