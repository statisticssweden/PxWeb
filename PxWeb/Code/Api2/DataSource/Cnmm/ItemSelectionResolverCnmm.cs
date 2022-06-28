using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class ItemSelectionResolverCnmm : IItemSelectionResolver
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IItemSelectionResolverFactory _itemSelectionResolverFactory;
        public ItemSelectionResolverCnmm(IMemoryCache memoryCache, IItemSelectionResolverFactory itemSelectionResolverFactory)
        {
            _memoryCache = memoryCache;
            _itemSelectionResolverFactory = itemSelectionResolverFactory;
        }

        public ItemSelection Resolve(string language, string selection, out bool selectionExists)
        {

            //if (!_memoryCache.TryGetValue("LookUpTableCache", out Dictionary<string,string> lookupTable))
            //{
            //    lookupTable = _itemSelectionResolverFactory.GetMenuLookup(language);  

            //    // TODO: Get cache time from appsetting
            //    var cacheEntryOptions = new MemoryCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            //    _memoryCache.Set("LookUpTableCache", lookupTable, cacheEntryOptions);
            //}


            ////todo: what todo if selection not present in lookuptable? 
            //var itemSelection = string.IsNullOrEmpty(selection) ? new ItemSelection() : new ItemSelection(lookupTable[selection.ToUpper()], selection);

            //return itemSelection;

            selectionExists = true;
            ItemSelection itemSelection = new ItemSelection();

            string lookupTableName = "LookUpTableCache_" + language;
            if (!_memoryCache.TryGetValue(lookupTableName, out Dictionary<string, string> lookupTable))
            {
                lookupTable = _itemSelectionResolverFactory.GetMenuLookup(language);

                // TODO: Get cache time from appsetting
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(lookupTableName, lookupTable, cacheEntryOptions);
            }

            if (!string.IsNullOrEmpty(selection))
            {
                if (lookupTable.ContainsKey(selection.ToUpper()))
                {
                    string menu = lookupTable[selection.ToUpper()];
                    itemSelection.Menu = menu;
                    itemSelection.Selection = Path.Combine(menu, selection);
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
