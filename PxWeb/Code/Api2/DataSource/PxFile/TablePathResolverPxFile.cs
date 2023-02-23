using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PCAxis.Menu;
using PCAxis.Paxiom;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.Cache;
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
        private readonly IPxCache _pxCache;
        private readonly IPxHost _hostingEnvironment;
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly ILogger _logger;

        public TablePathResolverPxFile(IPxCache pxCache, IPxHost hostingEnvironment, IPxApiConfigurationService pxApiConfigurationService, ILogger<TablePathResolverPxFile> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _pxCache = pxCache;
            _pxApiConfigurationService = pxApiConfigurationService;
        }

        public string Resolve(string language, string id, out bool selectionExists)
        {
            string tablePath = "";
            selectionExists = true;

            string lookupTableName = "LookUpTablePathCache_" + language;
            var lookupTable = _pxCache.Get<Dictionary<string, string>>(lookupTableName);
            if (lookupTable is null)
            {
                lookupTable = GetPxTableLookup(language);
                _pxCache.Set(lookupTableName, lookupTable);
            }

            if (!string.IsNullOrEmpty(id))
            {
                if (lookupTable.ContainsKey(id.ToUpper()))
                {
                    tablePath = Path.Combine(_hostingEnvironment.RootPath, lookupTable[id.ToUpper()]);
                }
                else
                {
                    selectionExists = false;
                }
            }

            return tablePath;
        }

        private Dictionary<string, string> GetPxTableLookup(string language)
        {
            var tableLookup = new Dictionary<string, string>();

            try
            {
                string webRootPath = _hostingEnvironment.RootPath;
                string xmlFilePath = Path.Combine(_hostingEnvironment.RootPath, "Database", "Menu.xml");

                XmlDocument xdoc = new XmlDocument();

                if (System.IO.File.Exists(xmlFilePath))
                {
                    xdoc.Load(xmlFilePath);
                }

                string xpath = string.Format("//Language [@lang='{0}']//Link", language);
                var nodeList = xdoc.SelectNodes(xpath); 

                if (nodeList != null)
                {
                    foreach (XmlElement childEl in nodeList)
                    {
                        string selection = childEl.GetAttribute("selection");
                        string tableId = childEl.GetAttribute("tableId");
                        if (!tableLookup.ContainsKey(tableId))
                        {
                            tableLookup.Add(tableId, selection);
                        }
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
