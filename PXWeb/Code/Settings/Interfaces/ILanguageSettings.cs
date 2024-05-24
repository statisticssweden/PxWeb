namespace PXWeb
{
    /// <summary>
    /// Interface for General.Languages.Language settings
    /// </summary>
    public interface ILanguageSettings
    {
        /// <summary>
        /// Name (code) for the language
        /// </summary>
        string Name { get; }

        /// <summary>
        /// If it is the default language
        /// </summary>
        bool DefaultLanguage { get; }

        /// <summary>
        /// String used as decimal separator
        /// </summary>
        string DecimalSeparator { get; }

        /// <summary>
        /// String used as thousand separator
        /// </summary>
        string ThousandSeparator { get; }

        /// <summary>
        /// Date format
        /// </summary>
        string DateFormat { get; }
    }
}
