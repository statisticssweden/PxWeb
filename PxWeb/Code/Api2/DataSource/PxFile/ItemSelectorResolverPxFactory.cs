using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Config.Api2;
using PxWeb.Controllers.Api2;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class ItemSelectorResolverPxFactory : IItemSelectionResolverFactory
    {
        private readonly IPxFileConfigurationService _pxFileConfigurationService;
        private readonly IPxHost _hostingEnvironment;
        private readonly ILogger _logger;

        public ItemSelectorResolverPxFactory(IPxFileConfigurationService pxFileConfigurationService, IPxHost hostingEnvironment, ILogger<ItemSelectorResolverPxFactory> logger)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        
        public Dictionary<string, ItemSelection> GetMenuLookup(string language)
        {
            var menuLookup = new Dictionary<string, ItemSelection>();

            try
            {
                string webRootPath = _hostingEnvironment.RootPath;
                string xmlFilePath = Path.Combine(_hostingEnvironment.RootPath, "Database", "Menu.xml");

                XmlDocument xdoc = new XmlDocument();

                if (System.IO.File.Exists(xmlFilePath))
                {
                    xdoc.Load(xmlFilePath);
                }

                // Add Menu levels to lookup table
                string xpath = string.Format("//Language [@lang='{0}']//MenuItem", language);
                AddMenuItemsToMenuLookup(xdoc, menuLookup, xpath);

                // Add Tables to lookup table
                xpath = string.Format("//Language [@lang='{0}']//Link", language);
                AddTablesToMenuLookup(xdoc, menuLookup, xpath);
            }

            catch (Exception e)
            {
                _logger.LogError($"Error loading MenuLookup table for language {language}", e);
            }

            return menuLookup;
        }

        private void AddMenuItemsToMenuLookup(XmlDocument xdoc, Dictionary<string, ItemSelection> menuLookup, string xpath)
        {
            var nodeList = xdoc.SelectNodes(xpath);

            if (nodeList != null)
            {
                foreach (XmlElement childEl in nodeList)
                {
                    string selection = childEl.GetAttribute("selection");
                    var menu = Path.GetDirectoryName(selection);
                    var sel = Path.GetFileName(selection).ToUpper();
                    if (!menuLookup.ContainsKey(sel))
                    {
                        ItemSelection itemSelection = new ItemSelection(menu, selection);
                        //menuLookup.Add(sel, menu);
                        menuLookup.Add(sel, itemSelection);
                    }
                }
            }
        }

        private void AddTablesToMenuLookup(XmlDocument xdoc, Dictionary<string, ItemSelection> menuLookup, string xpath)
        {
            var nodeList = xdoc.SelectNodes(xpath);

            if (nodeList != null)
            {
                foreach (XmlElement childEl in nodeList)
                {
                    string selection = childEl.GetAttribute("selection");
                    string tableId = childEl.GetAttribute("tableId");
                    var menu = Path.GetDirectoryName(selection);
                    //var sel = Path.GetFileName(selection).ToUpper();
                    if (!menuLookup.ContainsKey(tableId))
                    {
                        ItemSelection itemSelection = new ItemSelection(menu, selection);
                        //menuLookup.Add(sel, menu);
                        menuLookup.Add(tableId, itemSelection);
                    }
                }
            }
        }

    }
}
