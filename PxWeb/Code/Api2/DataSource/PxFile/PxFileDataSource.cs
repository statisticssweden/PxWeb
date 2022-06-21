using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class PxFileDataSource : IDataSource
    {
        private readonly IPxFileConfigurationService _pxFileConfigurationService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PxFileDataSource(IPxFileConfigurationService pxFileConfigurationService, IHostingEnvironment hostingEnvironment)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public PxMenuBase CreateMenu(string id, string language)
        {
            var pxOptions = _pxFileConfigurationService.GetConfiguration();

            string webRootPath = _hostingEnvironment.WebRootPath;
            string xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Database", "Menu.xml");

            XmlMenu menu = new XmlMenu(XDocument.Load(xmlFilePath), language,
                    m =>
                    {
                        m.Restriction = item =>
                        {
                            return true;
                        };
                    });

            //ItemSelection cid = PathHandlerFactory.Create(PCAxis.Web.Core.Enums.DatabaseType.PX).GetSelection(nodeId);
            //menu.SetCurrentItemBySelection(cid.Menu, cid.Selection);
            //currentItem = menu.CurrentItem;
            return menu;
            //}
            //currentItem = null;
            //return null;
        }
    }
}
