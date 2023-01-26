namespace Px.Search.Lucene.Config
{
    public interface ILuceneConfigurationService
    {
        LuceneConfigurationOptions GetConfiguration();
        string GetIndexDirectoryPath();
    }
}
