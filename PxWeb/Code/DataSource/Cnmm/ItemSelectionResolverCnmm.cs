using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;

namespace PxWeb.Code.DataSource.Cnmm
{
    public class ItemSelectionResolverCnmm : IItemSelectionResolver
    {
        private readonly IMemoryCache _memoryCache;

        private const string CacheKey = "lookupTableCacheKey";


        public ItemSelectionResolverCnmm(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public ItemSelection Resolve(string selection)
        {
            //todo : Get ItemSelection from pcaxis.sql from Norway
            
            if (!_memoryCache.TryGetValue("LookUpTableCache", out Dictionary<string,string> lookupTable))
            {
                lookupTable = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases["ssd"].GetMenuLookup();  

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                _memoryCache.Set("LookUpTableCache", lookupTable, cacheEntryOptions);
            }
            
            //todo: what todo if selection not present in lookuptable? 
            var itemSelection = string.IsNullOrEmpty(selection) ? new ItemSelection() : new ItemSelection(lookupTable[selection.ToUpper()], selection.ToUpper());
            
            return itemSelection;
        }
    }
}
