using Microsoft.Extensions.Logging;
using PxWeb.Config.Api2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb.Database
{
    /// <summary>
    /// Item handler for .link files in PX file databases
    /// </summary>
    public class LinkFileHandler : IItemHandler
    {
        private ILogger _logger;
        private readonly PxApiConfigurationOptions _configOptions;

        public LinkFileHandler(PxApiConfigurationOptions configOptions, ILogger logger)
        {
            _configOptions = configOptions;
            _logger = logger;   
        }

        #region IItemHandler Members

        /// <summary>
        /// Return the priority of the handler which is set to 4
        /// </summary>
        public int Priority
        {
            get 
            { 
                return 4; 
            }
        }

        /// <summary>
        /// Chackes if it is a link file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>Returns true if the file ends with .link otherwise false</returns>
        public bool CanHandle(string path)
        {
            return path.EndsWith(".link", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Reads the link file and creates a LinkItem object representing the link
        /// </summary>
        /// <param name="path">The path to the link file</param>
        /// <returns>A LinkItem</returns>
        /// <remarks>
        /// If the link file do not have a language specified in the name then the 
        /// default language specified in the settings will be used
        /// </remarks>
        public object Handle(string path, DatabaseLogger logger)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            int splittIndex = fileName.IndexOf('_');

            LinkItem link;
            string text;
            string location;
            string language;

            if (splittIndex < 0)
            {
                //No underscore in the file name use the default language
                language = _configOptions.DefaultLanguage;
            }
            else
            {
                language = fileName.Substring(splittIndex + 1);
            }

            string linkData;
            try
            {
                linkData = ReadAll(path);
            }
            catch (System.IO.IOException ex)
            {
                logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Error, Message = "Could not read file " + path });
                _logger.LogWarning(ex.ToString());
                return null;
            }

            //Find the second "
            splittIndex = linkData.IndexOf('"', 1);

            //No second " corrupted link file
            if (splittIndex < 0)
            {
                logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Warning, Message = "Corrupt link file " + path });
                return null;
            }

            //Find the real splittIndex 
            splittIndex = linkData.IndexOf(',', splittIndex);

            //No , after the second " corrupted link file
            if (splittIndex < 0) 
            {
                logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Warning, Message = "Corrupt link file " + path });
                return null;
            }

            text = TidyString(linkData.Substring(0, splittIndex)).Trim('"');
            location = TidyString(linkData.Substring(splittIndex + 1)).Trim('"');

            link = new LinkItem(text,location,language);
            return link;
        }

        /// <summary>
        /// Cleans up the string by removing "
        /// </summary>
        /// <param name="str">The string to clean</param>
        /// <returns>a clean string</returns>
        private string TidyString(string str)
        {
            //TODO remove "
            return str;
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
