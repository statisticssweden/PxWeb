using PCAxis.Sql.DbConfig;
using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PXWeb
{
    public interface IRouteExtender
    {
        ICacheService MetaCacheService { get; set; }
        string HomeSitePage { get; set; }
        string SitePathStart { get; }
        string DefaultRedirectPage { get; set; }
        SqlDbConfig Db { get; set; }
        string GetPresentationRedirectUrl(string tableId, string presentationLayout);
        void AddSavedQueryRoute(RouteCollection routes);
        void RegisterCustomRoutes(RouteCollection routes);  
        string GetDatabase();
        bool ShowBreadcrumb();
        LinkManager.LinkMethod CreateLink { get; }
        IPxUrlProvider PxUrlProvider { get; }
        string GetTableListPath(string path);
        string GetLastNodeFromPath(string path);
        void RedirectMenuRoutePath(string language, string tableListName);
        string GetSavedQueryPath(string language, string queryId);
        string GetTableIdByName(string tablename);
        string GetLanguageFromUri(Uri uri);
        bool DoesSelectionPathContainTableName(System.Web.UI.Page page);
        bool HasTableData(string tableId);
        string GetRedirectNoDataPath(string tableId);
    }
}