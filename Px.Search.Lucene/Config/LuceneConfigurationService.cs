using Microsoft.Extensions.Options;

namespace Px.Search.Lucene.Config
{
    public class LuceneConfigurationService : ILuceneConfigurationService
    {
        private readonly LuceneConfigurationOptions _configOptions;

        public LuceneConfigurationService(IOptions<LuceneConfigurationOptions> configOptions)
        {
            _configOptions = configOptions.Value;
        }
        public LuceneConfigurationOptions GetConfiguration()
        {
            return _configOptions;
        }
    }
}
