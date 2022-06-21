using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.DataSource.Cnmm;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class ItemSelectionResolverPxFile : IItemSelectionResolver
    {

        private readonly IMemoryCache _memoryCache;
        private readonly IItemSelectionResolverFactory _itemSelectionResolverFactory;

        public ItemSelectionResolverPxFile(IMemoryCache memoryCache, IItemSelectionResolverFactory itemSelectionResolverFactory)
        {
            _memoryCache = memoryCache;
            _itemSelectionResolverFactory = itemSelectionResolverFactory;
        }
        public ItemSelection Resolve(string selection)
        {
            if (!_memoryCache.TryGetValue("LookUpTableCache", out Dictionary<string, string> lookupTable))
            {
                //lookupTable = _itemSelectionResolverFactory.GetMenuLookup();

                

                    // TODO: Get cache time from appsetting
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set("LookUpTableCache", lookupTable, cacheEntryOptions);
            }

            //todo: what todo if selection not present in lookuptable? 
            var itemSelection = string.IsNullOrEmpty(selection) ? new ItemSelection() : new ItemSelection(lookupTable[selection.ToUpper()], selection);

            return itemSelection;

        }
    }
}
