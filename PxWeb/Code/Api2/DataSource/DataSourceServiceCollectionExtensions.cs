using log4net.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Code.Api2.DataSource.PxFile;
using PxWeb.Config.Api2;
using System;

namespace PxWeb.Code.Api2.DataSource
{
    public static class DataSourceServiceCollectionExtensions
    {
        public static void AddPxDataSource(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var dataSource = builder.Configuration.GetSection("DataSource:DataSourceType");

            if (dataSource.Value.ToUpper() == "PX")
            {
                // Px-file data source
                builder.Services.AddTransient<IDataSource, PxFileDataSource>();
                builder.Services.AddTransient<IItemSelectionResolver, ItemSelectionResolverPxFile>();
                builder.Services.AddTransient<IItemSelectionResolverFactory, ItemSelectorResolverPxFactory>();
                builder.Services.AddTransient<ITablePathResolver, TablePathResolverPxFile>();

                // Add configuration
                builder.Services.Configure<PxFileConfigurationOptions>(builder.Configuration.GetSection("DataSource:PX"));
                builder.Services.AddTransient<IPxFileConfigurationService, PxFileConfigurationService>();

                //Set if strict check of groupings shall be performed or not
                var strictAggregations = builder.Configuration.GetSection("DataSource:PX:StrictAggregations");
                bool strict = true;
                if (strictAggregations != null && strictAggregations.Value != null)
                {
                    Boolean.TryParse(strictAggregations.Value, out strict);
                }
                PCAxis.Paxiom.GroupRegistry.GetRegistry().Strict = strict;

                //Load aggregations
                PCAxis.Paxiom.GroupRegistry.GetRegistry().LoadGroupingsAsync();
            }
            else
            {
                // CNMM data source
                builder.Services.AddTransient<IDataSource, CnmmDataSource>();
                builder.Services.AddTransient<IItemSelectionResolver, ItemSelectionResolverCnmm>();
                builder.Services.AddTransient<IItemSelectionResolverFactory, ItemSelectionResolverCnmmFactory>();
                builder.Services.AddTransient<ITablePathResolver, TablePathResolverCnmm>();

                // Add configuration
                builder.Services.Configure<CnmmConfigurationOptions>(builder.Configuration.GetSection("DataSource:CNMM"));
                builder.Services.AddTransient<ICnmmConfigurationService, CnmmConfigurationService>();
            }

        }

    }
}
