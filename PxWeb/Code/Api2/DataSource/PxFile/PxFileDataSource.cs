using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using PCAxis.Paxiom;
using PCAxis.Sql.DbConfig;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class PxFileDataSource : IDataSource
    {
        private readonly IPxFileConfigurationService _pxFileConfigurationService;
        private readonly IItemSelectionResolver _itemSelectionResolver;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PxFileDataSource(IPxFileConfigurationService pxFileConfigurationService, IItemSelectionResolver itemSelectionResolver, IWebHostEnvironment hostingEnvironment)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
            _hostingEnvironment = hostingEnvironment;
        }

        public IPXModelBuilder CreateBuilder(string id, string language)
        {
            var builder = new PCAxis.Paxiom.PXFileBuilder();

            var tablePath = GetTablePath(id);

            string path = Path.Combine(_hostingEnvironment.WebRootPath, tablePath);
            builder.SetPath(path);
            builder.SetPreferredLanguage(language);
            return builder;
        }

        private string GetTablePath(string id)
        {
            //TODO: Get path from selection attribute in Menu.xml
            return @"Database\AggregallowedNo\PR0101B3.px";
        }

        public PxMenuBase CreateMenu(string id, string language, out bool selectionExists)
        {
            string xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Database", "Menu.xml");

            ItemSelection itmSel = _itemSelectionResolver.Resolve(language, id, out selectionExists);

            XmlMenu menu = new XmlMenu(XDocument.Load(xmlFilePath), language,
                    m =>
                    {
                        m.Restriction = item =>
                        {
                            return true;
                        };
                    });
            menu.SetCurrentItemBySelection(itmSel.Menu, itmSel.Selection);
            return menu;
        }
    }
}
