using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using PCAxis.Paxiom;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class CnmmDataSource : IDataSource
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;
        private readonly IItemSelectionResolver _itemSelectionResolver;
        private readonly ITablePathResolver _tablePathResolver;

        public CnmmDataSource(ICnmmConfigurationService cnmmConfigurationService, IItemSelectionResolver itemSelectionResolver, ITablePathResolver tablePathResolver)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
            _tablePathResolver = tablePathResolver;
        }

        public IPXModelBuilder CreateBuilder(string id, string language)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();

            var builder = new PCAxis.PlugIn.Sql.PXSQLBuilder();
            var path = _tablePathResolver.Resolve(language, id, out bool selctionExists);
            builder.SetPath(path);
            builder.SetPreferredLanguage(language);
            return builder;
        }

        public Item CreateMenu(string id, string language, out bool selectionExists)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();

            ItemSelection itmSel = _itemSelectionResolver.Resolve(language, id, out selectionExists);
            TableLink tblFix = null;

            if (selectionExists)
            {
                //Create database object to return
                DatamodelMenu retMenu = ConfigDatamodelMenu.Create(
                    language,
                    PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[cnmmOptions.DatabaseID],
                    m =>
                    {
                        //m.RootSelection = string.IsNullOrEmpty(nodeId) ? new ItemSelection() : PathHandlerFactory.Create(PCAxis.Web.Core.Enums.DatabaseType.CNMM).GetSelection(nodeId);
                        m.RootSelection = itmSel;
                        m.AlterItemBeforeStorage = item =>
                        {
                            if (item is Url)
                            {
                                Url url = (Url) item;
                            }

                            if (item is TableLink)
                            {
                                TableLink tbl = (TableLink) item;
                                //string tblId = tbl.ID.Selection;
                                //if (!string.IsNullOrEmpty(dbid))
                                //{
                                //    tbl.ID = new ItemSelection(item.ID.Menu, dbid + ":" + tbl.ID.Selection); // Hantering av flera databaser!
                                //}
                                if (string.Compare(tbl.ID.Selection, id, true) == 0)
                                {
                                    tblFix = tbl;
                                }

                                if (tbl.Published.HasValue)
                                {
                                    // Store date in the PC-Axis date format
                                    tbl.SetAttribute("modified",
                                        tbl.Published.Value.ToString(PCAxis.Paxiom.PXConstant.PXDATEFORMAT));
                                }
                            }

                            if (string.IsNullOrEmpty(item.SortCode))
                            {
                                item.SortCode = item.Text;
                            }
                        };
                        m.Restriction = item => { return true; }; // TODO: Will show all tables! Even though they are not published...
                    });
                retMenu.RootItem.Sort();
                return tblFix != null ? tblFix : retMenu.CurrentItem;
            }
            return null;
        }
    }
}
