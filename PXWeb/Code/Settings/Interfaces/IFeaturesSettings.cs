namespace PXWeb
{
    /// <summary>
    /// Interface for the Features settings
    /// </summary>
    public interface IFeaturesSettings
    {
        /// <summary>
        /// General feature settings
        /// </summary>
        IFeaturesGeneralSettings General { get; }

        /// <summary>
        /// Charts settings
        /// </summary>
        IChartSettings Charts { get; }

        /// <summary>
        /// API settings
        /// </summary>
        IApiSettings Api { get; }

        /// <summary>
        /// Search settings
        /// </summary>
        ISearchSettings Search { get; }

        /// <summary>
        /// Background worker process settings
        /// </summary>
        IBackgroundWorkerSettings BackgroundWorker { get; }

        /// <summary>
        /// Saved query settings
        /// </summary>
        ISavedQuerySettings SavedQuery { get; }
    }
}
