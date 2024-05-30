using Newtonsoft.Json;
using PXWeb.Code.API.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PXWeb.Code.API.Services
{
    public class BulkRegistry : IBulkRegistry
    {
        private string _context;
        private List<FileInfo> _history;

        public void Save()
        {
            SaveHistory();
            CreateIndexFile();
        }

        public void RegisterTableBulkFileUpdated(string tableId, DateTime generationDate)
        {
            var fileInfo = _history.FirstOrDefault(x => x.TableId == tableId);

            if (fileInfo != null)
            {
                fileInfo.GenerationDate = generationDate;
            }
            else
            {
                _history.Add(new FileInfo { TableId = tableId, GenerationDate = generationDate });
            }

        }

        public void SetContext(string context)
        {
            _context = context;
            _history = LoadHistory();
        }

        public bool ShouldTableBeUpdated(string tableId, DateTime lastUpdated)
        {
            var fileInfo = _history.FirstOrDefault(x => x.TableId == tableId);
            
            if (fileInfo != null)
            {
                var zipPath = System.IO.Path.Combine(_context, $"{tableId}.zip");

                if (lastUpdated != null &&
                    fileInfo.GenerationDate > lastUpdated &&
                    File.Exists(zipPath))
                {
                    return false;
                }
            }
            return true;
        }


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

        private void SaveHistory()
        {
            var historyPath = System.IO.Path.Combine(_context, "content.json");

            string json = JsonConvert.SerializeObject(_history, Formatting.Indented);
            File.WriteAllText(historyPath, json);
        }

        private void CreateIndexFile()
        {
            var content = new StringBuilder("<!DOCTYPE html><html lang=\"en\">\r\n\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n  <title>Bulk files</title>\r\n</head>\r\n\r\n<body>\r\n  <h1>Bulk files</h1>\r\n");

            foreach (var file in _history)
            {
                content.Append($"<a href=\"{file.TableId}.zip\">{file.TableId}.zip</a><br>\r\n");
            }

            content.Append("</body>\r\n\r\n</html>");

            //write content to file
            File.WriteAllText(Path.Combine(_context, "index.html"), content.ToString());
        }

        internal class FileInfo
        {
            public string TableId { get; set; }
            public DateTime GenerationDate { get; set; }
        }
    }
}