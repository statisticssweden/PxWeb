using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
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
        private readonly ITablePathResolver _tablePathResolver;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PxFileDataSource(IPxFileConfigurationService pxFileConfigurationService, IItemSelectionResolver itemSelectionResolver, ITablePathResolver tablePathResolver, IWebHostEnvironment hostingEnvironment)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
            _tablePathResolver = tablePathResolver;
            _hostingEnvironment = hostingEnvironment;
        }

        public IPXModelBuilder CreateBuilder(string id, string language)
        {
            var builder = new PCAxis.Paxiom.PXFileBuilder();

            var path = _tablePathResolver.Resolve(language, id, out bool selectionExists);
       
            //TODO: check values
            builder.SetPath(path);
            builder.SetPreferredLanguage(language);
            return builder;
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
