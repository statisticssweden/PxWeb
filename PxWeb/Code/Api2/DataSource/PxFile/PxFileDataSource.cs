using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using PCAxis.Paxiom;
using PCAxis.Sql.DbConfig;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;
using Px.Abstractions;
using PxWeb.Mappers;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class PxFileDataSource : IDataSource
    {
        private readonly IPxFileConfigurationService _pxFileConfigurationService;
        private readonly IItemSelectionResolver _itemSelectionResolver;
        private readonly ITablePathResolver _tablePathResolver;
        private readonly IPxHost _hostingEnvironment;
        private readonly ICodelistMapper _codelistMapper;

        public PxFileDataSource(IPxFileConfigurationService pxFileConfigurationService, IItemSelectionResolver itemSelectionResolver, ITablePathResolver tablePathResolver, IPxHost hostingEnvironment, ICodelistMapper codelistMapper)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
            _tablePathResolver = tablePathResolver;
            _hostingEnvironment = hostingEnvironment;
            _codelistMapper = codelistMapper;
        }

        /// <summary>
        /// Create a PxFileBuilder
        /// </summary>
        /// <param name="id">Table id</param>
        /// <param name="language">Language</param>
        /// <returns>Builder object, null if builder could not be created</returns>
        public IPXModelBuilder? CreateBuilder(string id, string language)
        {
            var builder = new PCAxis.Paxiom.PXFileBuilder();

            var path = _tablePathResolver.Resolve(language, id, out bool selectionExists);
       
            if (selectionExists)
            {
                builder.SetPath(path);
                builder.SetPreferredLanguage(language);
                return builder;
            }
            else
            { 
                return null; 
            }
        }

        public Item? CreateMenu(string id, string language, out bool selectionExists)
        {
            string xmlFilePath = Path.Combine(_hostingEnvironment.RootPath, "Database", "Menu.xml");

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

            // Fix selection for subitems - we only want the last part...
            if (menu.CurrentItem is PxMenuItem) 
            {
                foreach (var item in ((PxMenuItem)(menu.CurrentItem)).SubItems)
                {
                    if ((item is PxMenuItem) || (item is TableLink))
                    {
                        item.ID.Selection = GetIdentifierWithoutPath(item.ID.Selection);
                    }
                }
            }

            return menu.CurrentItem;
        }

        public Codelist? GetCodelist(string id, string language)
        {
            Codelist? codelist = null;
            
            if (string.IsNullOrEmpty(id))
            {
                return codelist;
            }

            id = id.Replace("agg_", "");

            Grouping grouping = PCAxis.Paxiom.GroupRegistry.GetRegistry().GetGrouping(id);

            if (grouping != null)
            {
                codelist = _codelistMapper.Map(grouping);
            }

            return codelist;
        }

        public bool TableExists(string tableId, string language)
        {
            bool selectionExists;

            _itemSelectionResolver.Resolve(language, tableId, out selectionExists);
            return selectionExists;

        }

        private string GetIdentifierWithoutPath(string id)
        {
            if (id.Contains('\\'))
            {
                return Path.GetFileName(id);
            }
            else
            {
                return id;
            }
        }

    }
}
