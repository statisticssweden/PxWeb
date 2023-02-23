using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.Cache;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class ItemSelectionResolverCnmm : IItemSelectionResolver
    {
        private readonly IPxCache _pxCache;
        private readonly IItemSelectionResolverFactory _itemSelectionResolverFactory;
        private readonly IPxApiConfigurationService _pxApiConfigurationService;

        public ItemSelectionResolverCnmm(IPxCache pxCache, IItemSelectionResolverFactory itemSelectionResolverFactory, IPxApiConfigurationService pxApiConfigurationService)
        {
            _pxCache = pxCache;
            _itemSelectionResolverFactory = itemSelectionResolverFactory;
            _pxApiConfigurationService = pxApiConfigurationService;
        }

        public ItemSelection Resolve(string language, string selection, out bool selectionExists)
        {
            selectionExists = true;
            ItemSelection itemSelection = new ItemSelection();

            string lookupTableName = "LookUpTableCache_" + language;
            var lookupTable = _pxCache.Get<Dictionary<string, ItemSelection>>(lookupTableName);
            if (lookupTable is null)
            {
                lookupTable = _itemSelectionResolverFactory.GetMenuLookup(language);
                _pxCache.Set(lookupTableName, lookupTable);
            }

            if (!string.IsNullOrEmpty(selection))
            {
                if (lookupTable.ContainsKey(selection.ToUpper()))
                {
                    var itmSel = lookupTable[selection.ToUpper()];
                    itemSelection.Menu = itmSel.Menu;
                    itemSelection.Selection = itmSel.Selection;
                }
                else
                {
                    selectionExists = false;
                }
            }
            return itemSelection;
        }
    }
}
