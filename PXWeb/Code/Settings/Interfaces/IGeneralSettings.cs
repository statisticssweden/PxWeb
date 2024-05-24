namespace PXWeb
{
    /// <summary>
    /// Interface for the General settings
    /// </summary>
    public interface IGeneralSettings
    {
        /// <summary>
        /// Site settings
        /// </summary>
        ISiteSettings Site { get; }

        /// <summary>
        /// Paths settings
        /// </summary>
        IPathsSettings Paths { get; }

        /// <summary>
        /// Language settings
        /// </summary>
        ILanguagesSettings Language { get; }

        /// <summary>
        /// FileFormats settings
        /// </summary>
        IFileFormatsSettings FileFormats { get; }

        /// <summary>
        /// Modules settings
        /// </summary>
        IModulesSettings Modules { get; }

        /// <summary>
        /// Global settings
        /// </summary>
        IGlobalSettings Global { get; }

        /// <summary>
        /// Databases settings
        /// </summary>
        IDatabasesSettings Databases { get; }

        /// <summary>
        /// Administration settings
        /// </summary>
        IAdministrationSettings Administration { get; }
    }
}
