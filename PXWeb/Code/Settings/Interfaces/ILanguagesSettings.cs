using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Languages settings
    /// </summary>
    public interface ILanguagesSettings
    {
        /// <summary>
        /// All possible languages
        /// </summary>
        IEnumerable<string> AllLanguages { get; }

        /// <summary>
        /// The default language
        /// </summary>
        string DefaultLanguage { get; }

        /// <summary>
        /// Available languages in PX-Web
        /// </summary>
        IEnumerable<ILanguageSettings> SiteLanguages { get; }

        /// <summary>
        /// Resets languages.  
        /// </summary>
        void ResetLanguages();

        /// <summary>
        /// Is the language a site language
        /// </summary>
        /// <param name="lang">Language to check</param>
        /// <returns></returns>
        bool IsSiteLanguage(string language);

    }
}
