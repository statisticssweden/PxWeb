using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class ItemSelectionResolverCnmm : IItemSelectionResolver
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IItemSelectionResolverFactory _pcAxisFactory;
        public ItemSelectionResolverCnmm(IMemoryCache memoryCache, IItemSelectionResolverFactory pcAxisFactory)
        {
            _memoryCache = memoryCache;
            _pcAxisFactory = pcAxisFactory;
        }

        public ItemSelection Resolve(string selection)
        {
            //todo : Get ItemSelection from pcaxis.sql from Norway
            
            if (!_memoryCache.TryGetValue("LookUpTableCache", out Dictionary<string,string> lookupTable))
            {
                lookupTable = _pcAxisFactory.GetMenuLookup();  
                //lookupTable = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases["ssd"].GetMenuLookup();  

                // TODO: Get cache time from appsetting
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                _memoryCache.Set("LookUpTableCache", lookupTable, cacheEntryOptions);
            }

            //todo: what todo if selection not present in lookuptable? 
            var itemSelection = string.IsNullOrEmpty(selection) ? new ItemSelection() : new ItemSelection(lookupTable[selection.ToUpper()], selection);
            
            return itemSelection;
        }
    }
}
