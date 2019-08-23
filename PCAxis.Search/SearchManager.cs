using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using PCAxis.Menu;
using System.Xml;
using PCAxis.Paxiom.Extensions;

namespace PCAxis.Search
{
    /// <summary>
    /// Describes status of a search attempt
    /// </summary>
    public enum SearchStatusType
    {
        // A successful search has been made
        Successful,
        // No search index existed for the database/language
        NotIndexed
    }
    
    /// <summary>
    /// Delegate function for getting the Menu
    /// </summary>
    /// <param name="database">Database id</param>
    /// <param name="nodeId">Node id</param>
    /// <param name="language">Language</param>
    /// <returns></returns>
    public delegate PCAxis.Menu.PxMenuBase GetMenuDelegate(string database, ItemSelection node, string language, out PCAxis.Menu.Item currentItem);

    /// <summary>
    /// Class for managing search indexes
    /// </summary>
    public class SearchManager
    {
        #region "Private fields"
        
        private static SearchManager _current = new SearchManager();
        private DirectoryInfo _databaseBaseDirectory;
        private GetMenuDelegate _menuMethod;
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(SearchManager));
        private FileSystemWatcher _dbConfigWatcher;
        private int _cacheTime;
        
        #endregion


        #region "Public properties"
        
        /// <summary>
        /// Get the (Singleton) SearchManager object
        /// </summary>
        public static SearchManager Current
        {
            get
            {
                return _current;
            }
        }

        /// <summary>
        /// Time in minutes that search index will be cached
        /// </summary>
        public int CacheTime 
        {
            get
            {
                return _cacheTime;
            }
            set
            {
                _cacheTime = value;
            }
        }

        #endregion

        /// <summary>
        /// Private constructor
        /// </summary>
        private SearchManager()
        {
        }

        #region "Public methods"

        /// <summary>
        /// Initialize the SearchManager
        /// </summary>
        /// <param name="databaseBaseDirectory">Base directory for PX databases</param>
        /// <param name="menuMethod">Delegate method to get the Menu</param>
        /// <param name="cacheTime">Time in minutes that searchers will be cached</param>
        public void Initialize(string databaseBaseDirectory, GetMenuDelegate menuMethod, int cacheTime=60, DefaultOperator defaultOperator=DefaultOperator.OR)
        {
            SetDatabaseBaseDirectory(databaseBaseDirectory);
            SetDbConfigWatcher();
            _menuMethod = menuMethod;
            _cacheTime = cacheTime;
            PxModelManager.Current.Initialize(databaseBaseDirectory);
            SetDefaultOperator(defaultOperator);
        }


        /// <summary>
        /// Create index for the specified database and language
        /// </summary>
        /// <param name="database">Database id</param>
        /// <param name="language">language</param>
        public bool CreateIndex(string database, string language)
        {
            Indexer indexer = new Indexer(GetIndexDirectoryPath(database, language), _menuMethod, database, language);

            if (!indexer.CreateIndex()) {
                return false;
            }

            RemoveSearcher(database, language);
            return true;
        }

        /// <summary>
        /// Update index for the specified database and language
        /// </summary>
        /// <param name="database">Database id</param>
        /// <param name="language">language</param>
        public bool UpdateIndex(string database, string language, List<TableUpdate> tableList)
        {
            Indexer indexer = new Indexer(GetIndexDirectoryPath(database, language), _menuMethod, database, language);

            if (!indexer.UpdateIndex(tableList)) {
                return false;
            }

            RemoveSearcher(database, language);
            return true;
        }


        /// <summary>
        /// Search for text in the specified index 
        /// </summary>
        /// <param name="database">Database id</param>
        /// <param name="language">Language</param>
        /// <param name="text">Text to search for</param>
        /// <returns></returns>
        public List<SearchResultItem> Search(string database, string language, string text, out SearchStatusType status, string filter = "", int resultListLength = 250)
        {
            Searcher searcher = GetSearcher(database, language);
            
            if (searcher == null)
            {
                // Return empty list
                status = SearchStatusType.NotIndexed;
                return new List<SearchResultItem>();
            }

            status = SearchStatusType.Successful;
            return searcher.Search(text, filter, resultListLength);
        }

        /// <summary>
        /// Set which operator AND/OR will be used by default when more than one word is specified for a search query
        /// </summary>
        /// <param name="defaultOPerator"></param>
        public void SetDefaultOperator(DefaultOperator defaultOperator)
        {
            if (defaultOperator == DefaultOperator.OR)
            {
                Searcher.DefaultOperator = Lucene.Net.QueryParsers.QueryParser.Operator.OR;
            }
            else
            {
                Searcher.DefaultOperator = Lucene.Net.QueryParsers.QueryParser.Operator.AND;
            }
        }

        #endregion

        #region "Private methods"

        /// <summary>
        /// Set the index base directory
        /// </summary>
        /// <param name="indexDirectory">Base directory for all search indexes</param>
        private void SetDatabaseBaseDirectory(string databaseBaseDirectory)
        {
            if (!System.IO.Path.IsPathRooted(databaseBaseDirectory))
            {
                databaseBaseDirectory = HttpContext.Current.Server.MapPath(databaseBaseDirectory);
            }

            if (System.IO.Directory.Exists(databaseBaseDirectory))
            {
                _databaseBaseDirectory = new DirectoryInfo(databaseBaseDirectory);
                _logger.Info("Search index base directory successfully set to '" + databaseBaseDirectory + "'");
            }
            else
            {
                _logger.Error("Failed to set search index base directory. Directory '" + databaseBaseDirectory + "' does not exist");
            }
        }

        /// <summary>
        /// Get path to the specified index directory 
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="language">language</param>
        /// <returns></returns>
        private string GetIndexDirectoryPath(string database, string language)
        {
            StringBuilder dir = new StringBuilder(_databaseBaseDirectory.FullName);

            dir.Append(database);
            dir.Append(@"\_INDEX\");
            dir.Append(language);

            return dir.ToString();
        }

        /// <summary>
        /// Add file system watcher for the database.config files
        /// </summary>
        private void SetDbConfigWatcher()
        {
            _dbConfigWatcher = new FileSystemWatcher(_databaseBaseDirectory.FullName);
            _dbConfigWatcher.EnableRaisingEvents = true;
            _dbConfigWatcher.IncludeSubdirectories = true;
            _dbConfigWatcher.Filter = "database.config";
            _dbConfigWatcher.Changed += new FileSystemEventHandler(DatabaseConfigChanged);
        }

        // Event handler for when a database.config file has been changed  
        private void DatabaseConfigChanged(object source, FileSystemEventArgs e)
        {
            FileInfo dbConf = new FileInfo(e.FullPath);
            DirectoryInfo dbDir = dbConf.Directory;
            string indexPath = Path.Combine(dbDir.FullName, "_INDEX"); 

            if (!Directory.Exists(indexPath))
            {
                return;
            }

            DirectoryInfo indexDir = new DirectoryInfo(indexPath);

            // Check if search index has been updated
            DateTime IndexUpdated;
            if (GetIndexUpdated(e.FullPath, out IndexUpdated))
            {
                foreach (DirectoryInfo langDir in indexDir.GetDirectories())
                {
                    string key = CreateSearcherKey(dbDir.Name, langDir.Name);
                    Searcher searcher = (Searcher)System.Web.Hosting.HostingEnvironment.Cache[key];

                    if (searcher != null)
                    {
                        if (searcher.CreationTime < IndexUpdated)
                        {
                            // Search index has been updated and our searcher is out of date - remove it!
                            RemoveSearcher(dbDir.Name, langDir.Name);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Read the time when the search index was last updated from the specified database.config file
        /// </summary>
        /// <param name="path">Path to database.config file</param>
        /// <param name="indexUpdated">Out parameter containing the last updated date</param>
        /// <returns>True if last updated could be read from the file, else false</returns>
        private bool GetIndexUpdated(string path, out DateTime indexUpdated)
        {
            indexUpdated = DateTime.MinValue;

            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(path);
                XmlNode node = xdoc.SelectSingleNode("/settings/searchIndex/indexUpdated");

                if (node.InnerText.IsPxDate())
                {
                    indexUpdated = node.InnerText.PxDateStringToDateTime();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get Searcher from cache
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="language">language</param>
        /// <returns></returns>
        public Searcher GetSearcher(string database, string language)
        {
            string dir = GetIndexDirectoryPath(database, language);

            if (!System.IO.Directory.Exists(dir))
            {
                return null;
            }

            string key = CreateSearcherKey(database, language);

            if (System.Web.Hosting.HostingEnvironment.Cache[key] == null)
            {
                // Create new Searcher and add to cache
                Searcher searcher = new Searcher(dir);

                // Add searcher to cache for 5 minutes
                System.Web.Hosting.HostingEnvironment.Cache.Insert(key, searcher, null, DateTime.Now.AddMinutes(_cacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            // Get from cache
            return (Searcher)System.Web.Hosting.HostingEnvironment.Cache[key];
        }

        /// <summary>
        /// Remove the specified searcher from cache
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="language">language</param>
        private void RemoveSearcher(string database, string language)
        {
            string key = CreateSearcherKey(database, language);

            if (System.Web.Hosting.HostingEnvironment.Cache[key] != null)
            {
                System.Web.Hosting.HostingEnvironment.Cache.Remove(key);
            }
        }

        /// <summary>
        /// Create cache Searcher key
        /// </summary>
        /// <param name="database"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private string CreateSearcherKey(string database, string language)
        {
            StringBuilder key = new StringBuilder();

            key.Append("px-search-");
            key.Append(database);
            key.Append("|");
            key.Append(language);

            return key.ToString();
        }

        #endregion
    }
}
