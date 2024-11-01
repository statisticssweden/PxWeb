using Newtonsoft.Json;
using PCAxis.Paxiom.Localization;
using PCAxis.Web.Core.Management;
using PXWeb.Code.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace PXWeb.Code.API.Services
{
    /// <summary>
    /// Represents a bulk registry for managing table bulk files.
    /// </summary>
    public class BulkRegistry : IBulkRegistry
    {
        private string _context;
        private List<FileInfo> _history;
        private String _language;
        
        /// <summary>
        /// Saves the history of table bulk files and creates an index file.
        /// </summary>
        public void Save()
        {
            SaveHistory();
            CreateIndexFile();
        }

        /// <summary>
        /// Registers the updated date of a table bulk file.
        /// </summary>
        /// <param name="tableId">The ID of the table.</param>
        /// <param name="generationDate">The generation date of the file.</param>
        public void RegisterTableBulkFileUpdated(string tableId, string tableText, DateTime generationDate)
        {
            var fileInfo = _history.FirstOrDefault(x => x.TableId == tableId);

            if (fileInfo != null)
            {
                fileInfo.TableText = tableText;
                fileInfo.GenerationDate = generationDate;
            }
            else
            {
                _history.Add(new FileInfo { TableId = tableId, TableText = tableText, GenerationDate = generationDate });
            }

        }

        /// <summary>
        /// Sets the context for the bulk registry.
        /// </summary>
        /// <param name="context">The context path.</param>
        public void SetContext(string context, string language)
        {
            _context = context;
            _language = language;
            _history = LoadHistory();
            
        }

        /// <summary>
        /// Determines whether a table should be updated based on its ID and last updated date.
        /// </summary>
        /// <param name="tableId">The ID of the table.</param>
        /// <param name="lastUpdated">The last updated date of the table.</param>
        /// <returns><c>true</c> if the table should be updated; otherwise, <c>false</c>.</returns>
        public bool ShouldTableBeUpdated(string tableId, DateTime lastUpdated)
        {
            var fileInfo = _history.FirstOrDefault(x => x.TableId == tableId);

            if (fileInfo != null)
            {
                var zipPath = System.IO.Path.Combine(_context, $"{tableId}_{_language}.zip");

                if (lastUpdated != null &&
                    fileInfo.GenerationDate > lastUpdated &&
                    File.Exists(zipPath))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Loads the history of table bulk files from the content.json file.
        /// </summary>
        /// <returns>The list of FileInfo objects representing the history of table bulk files.</returns>
        private List<FileInfo> LoadHistory()
        {
            var history = new List<FileInfo>();
            var historyPath = System.IO.Path.Combine(_context, "content.json");

            if (File.Exists(historyPath))
            {
                history = JsonConvert.DeserializeObject<List<FileInfo>>(File.ReadAllText(historyPath));
            }

            return history;
        }

        /// <summary>
        /// Saves the history of table bulk files to the content.json and creates an index.html file.
        /// </summary>
        private void SaveHistory()
        {
            var historyPath = System.IO.Path.Combine(_context, "content.json");

            string json = JsonConvert.SerializeObject(_history, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(historyPath, json);
        }

        /// <summary>
        /// Creates an index.html file that contains a link to each bulk file.
        /// </summary>
        private void CreateIndexFile()
        {
            string informationBox = GetLocalizedString("PxWebBulkInformation", _language);
            string titleOnBrowserTab = GetLocalizedString("PxWebBulkTitleOnTab", _language);
            string tableTitle = GetLocalizedString("PxWebBulkTableTitle", _language);
            string tableColumnText = GetLocalizedString("PxWebBulkTableColumnText", _language);
            string tableColumnTabelId = GetLocalizedString("PxWebBulkTableColumnTableId", _language);
            string tableColumnFileName = GetLocalizedString("PxWebBulkTableColumnFileName", _language );
            string tableColumCreatedTime = GetLocalizedString("PxWebBulkTableColumCreatedTime", _language);

            const string style = "<style>table {border-collapse: collapse;}table, th, td {border: 1px solid black;}th, td {padding: 5px;text-align: left;}th {background-color: #bdbebd;}</style>";
            bool isGray = false;
            var content = new StringBuilder($"<!DOCTYPE html><html lang=\"en\">\r\n\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n  <title>" + titleOnBrowserTab + "</title>\r\n</head>\r\n\r\n<body>\r\n  <h1>" + tableTitle +"</h1>\r\n");
            content.Append(informationBox);
            content.Append(style);
            content.Append("<table border><tr style=\"background-color:#bdbebd;\">\r\n"); 
            content.Append($"<td><h3>" + tableColumnText + "</h3></td><td><h3>" + tableColumnTabelId + "</h3></td><td><h3>" + tableColumnFileName +"</h3></td><td><h3>"+ tableColumCreatedTime +"</h3></td></tr>\r\n");
            foreach (var file in _history)
            {
                string rowColor = isGray ? " style=\"background-color:#e9e9e9;\"" : "";
                content.Append($"<tr {rowColor}>");
                content.Append($"<td><a href=\"{file.TableId}_{_language}.zip\">{file.TableText}</a></td>\r\n");
                content.Append($"<td>{file.TableId}</td>\r\n");
                content.Append($"<td><a href=\"{file.TableId}_{_language}.zip\">{file.TableId}_{_language}.zip</a></td>\r\n");
                content.Append($"<td>{file.GenerationDate.ToShortDateString()}</td>\r\n");
                content.Append("</tr>");
                isGray = !isGray;
            }
            content.Append("</table>");
            content.Append("</body>\r\n\r\n</html>");

            //write content to file
            File.WriteAllText(Path.Combine(_context, "index.html"), content.ToString());
        }

       
        /// <summary>
        /// Get text in the currently selected language
        /// </summary>
        /// <param name="key">Key identifying the string in the language file</param>
        /// <returns>Localized string</returns>
        public string GetLocalizedString(string key, string lang)
        {
            if(string.IsNullOrWhiteSpace(lang))
            {
                lang = LocalizationManager.CurrentCulture.Name;
            }            
            return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key, new CultureInfo(lang));
        }
       
        internal class FileInfo
        {
            public string TableId { get; set; }
            public string TableText { get; set; }
            public DateTime GenerationDate { get; set; }
        }
    }
}