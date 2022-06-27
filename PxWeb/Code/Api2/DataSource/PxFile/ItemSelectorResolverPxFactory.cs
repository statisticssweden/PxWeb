using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class ItemSelectorResolverPxFactory : IItemSelectionResolverFactory
    {
        private readonly IPxFileConfigurationService _pxFileConfigurationService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ItemSelectorResolverPxFactory(IPxFileConfigurationService pxFileConfigurationService, IHostingEnvironment hostingEnvironment)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public Dictionary<string, string> GetMenuLookup(string language)
        {
            var menuLookup = new Dictionary<string, string>();

            string webRootPath = _hostingEnvironment.WebRootPath;
            string xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Database", "Menu.xml");

            XmlDocument xdoc = new XmlDocument();

            if (System.IO.File.Exists(xmlFilePath))
            {
                xdoc.Load(xmlFilePath);
            }

            string xpath = string.Format("//Language [@lang='{0}']//MenuItem", language);

            foreach (XmlElement childEl in xdoc.SelectNodes(xpath))
            {
                string selection = childEl.GetAttribute("selection");
                var menu = Path.GetDirectoryName(selection);
                var sel = Path.GetFileName(selection).ToUpper();
                if (!menuLookup.ContainsKey(sel))
                {
                    menuLookup.Add(sel, menu);
                }
            }

            return menuLookup;
        }
    }
}
