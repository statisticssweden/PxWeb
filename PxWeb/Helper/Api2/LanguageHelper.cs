using PxWeb.Config.Api2;

namespace PxWeb.Helper.Api2
{
    public class LanguageHelper : ILanguageHelper
    {
        private readonly IPxApiConfigurationService _pxApiConfigurationService;

        public LanguageHelper(IPxApiConfigurationService pxApiConfigurationService)
        {
            _pxApiConfigurationService = pxApiConfigurationService;
        }
        public string HandleLanguage(string? lang)
        {
            if (lang == null)
            {
                var op = _pxApiConfigurationService.GetConfiguration();
                lang = op.DefaultLanguage;
            }
            return lang.ToLower();
        }
    }
}
