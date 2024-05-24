using System;

namespace PXWeb.Database
{
    /// <summary>
    /// Item handler for Menu.sort files in PX file databases
    /// </summary>
    public class MenuSortFileHandler : IItemHandler
    {
        private log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MenuSortFileHandler));

        #region IItemHandler Members

        /// <summary>
        /// Return the priority of the handler which is set to 4
        /// </summary>
        public int Priority
        {
            get { return 4; }
        }

        /// <summary>
        /// Checks if it is a Menu.sort file
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>
        /// Returns true if the filename starts with Menu. and ends with .sort 
        /// oterwise it will return false
        /// </returns>
        /// <remarks>the check is case insensitive</remarks>
        public bool CanHandle(string path)
        {
            if (path.EndsWith(".sort", StringComparison.InvariantCultureIgnoreCase))
            {
                return System.IO.Path.GetFileName(path).StartsWith("Menu", StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// Reads the Menu.sort file and creates a MenuSortItem containg the information read
        /// </summary>
        /// <param name="path">Path to the Menu.sort file</param>
        /// <returns>A MenuSOrtItem containg the information from the Menu.sort file</returns>
        /// <remarks>
        /// If the alias file do not have a language specified in the name then the 
        /// default language specified in the settings will be used
        /// </remarks>
        public object Handle(string path, DatabaseLogger logger)
        {
            string fileName = System.IO.Path.GetFileName(path);
            int splittIndex = fileName.LastIndexOf('.');

            MenuSortItem sort = new MenuSortItem();

            if (splittIndex == 4)
            {
                sort.Language = Settings.Current.General.Language.DefaultLanguage;
            }
            else
            {
                sort.Language = fileName.Substring(5, splittIndex - 5);
            }
            try
            {
                sort.SortString = ReadAll(path);
            }
            catch (System.IO.IOException ex)
            {
                logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Error, Message = "Could not read file " + path });
                _logger.Warn(ex.ToString());
                return null;
            }

            return sort;
        }

        /// <summary>
        /// Reads all text from the specified file
        /// </summary>
        /// <param name="path">Path to the file to read</param>
        /// <returns>the contents of the file</returns>
        private string ReadAll(string path)
        {
            System.Text.Encoding encoding = PCAxis.Paxiom.Parsers.PXFileParser.GetEncoding(path);
            string data;

            using (System.IO.StreamReader reader =
                    new System.IO.StreamReader(
                        new System.IO.FileStream(path,
                                                 System.IO.FileMode.Open,
                                                 System.IO.FileAccess.Read,
                                                 System.IO.FileShare.Read,
                                                 2048),
                                                encoding))
            {
                data = reader.ReadToEnd();
            }
            return data;
        }

        #endregion
    }
}