using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PCAxis.Menu;
using PCAxis.Paxiom;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Config.Api2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class TablePathResolverPxFile : ITablePathResolver
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly ILogger _logger;

        public TablePathResolverPxFile(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment, IPxApiConfigurationService pxApiConfigurationService, ILogger<TablePathResolverPxFile> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _memoryCache = memoryCache;
            _pxApiConfigurationService = pxApiConfigurationService;
        }

        public string Resolve(string language, string id, out bool selectionExists)
        {
            string tablePath = "";
            selectionExists = true;
            var op = _pxApiConfigurationService.GetConfiguration();

            string lookupTableName = "LookUpTablePathCache_" + language;
            if (!_memoryCache.TryGetValue(lookupTableName, out Dictionary<string, string> lookupTable))
            {
                lookupTable = GetPxTableLookup(language);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(op.CacheTime));

                _memoryCache.Set(lookupTableName, lookupTable, cacheEntryOptions);
            }

            if (!string.IsNullOrEmpty(id))
            {
                if (lookupTable.ContainsKey(id.ToUpper()))
                {
                    tablePath = lookupTable[id.ToUpper()];
                }
                else
                {
                    selectionExists = false;
                }
            }

            string path = Path.Combine(_hostingEnvironment.WebRootPath, tablePath);

            return path;
        }

        private Dictionary<string, string> GetPxTableLookup(string language)
        {
            var tableLookup = new Dictionary<string, string>();

            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                string xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Database", "Menu.xml");

                XmlDocument xdoc = new XmlDocument();

                if (System.IO.File.Exists(xmlFilePath))
                {
                    xdoc.Load(xmlFilePath);
                }

                string xpath = string.Format("//Language [@lang='{0}']//Link", language);

                foreach (XmlElement childEl in xdoc.SelectNodes(xpath))
                {
                    string selection = childEl.GetAttribute("selection");
                    var pxFile = Path.GetFileName(selection).ToUpper();
                    if (!tableLookup.ContainsKey(pxFile))
                    {
                        tableLookup.Add(pxFile, selection);
                    }
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error loading TablePathLookup table for language {language}", e);
            }

            return tableLookup;
        }

    }
}
