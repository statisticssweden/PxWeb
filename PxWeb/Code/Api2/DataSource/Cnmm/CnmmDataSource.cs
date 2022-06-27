using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class CnmmDataSource : IDataSource
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;
        private readonly IItemSelectionResolver _itemSelectionResolver;
        private readonly IItemSelectionResolverFactory _pcAxisFactory;

        public CnmmDataSource(ICnmmConfigurationService cnmmConfigurationService, IItemSelectionResolver itemSelectionResolver,IItemSelectionResolverFactory pcAxisFactory)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
            _pcAxisFactory = pcAxisFactory;
        }

        public PxMenuBase CreateMenu(string id, string language)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();

            ItemSelection itmSel = _itemSelectionResolver.Resolve(language, id);
            
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
                                                    Url url = (Url)item;
                                                }

                                                if (item is TableLink)
                                                {
                                                    TableLink tbl = (TableLink)item;
                                                    string tblId = tbl.ID.Selection;
                                                    //if (!string.IsNullOrEmpty(dbid))
                                                    //{
                                                    //    tbl.ID = new ItemSelection(item.ID.Menu, dbid + ":" + tbl.ID.Selection); // Hantering av flera databaser!
                                                    //}



                                                    if (tbl.Published.HasValue)
                                                    {
                                                        // Store date in the PC-Axis date format
                                                        tbl.SetAttribute("modified", tbl.Published.Value.ToString(PCAxis.Paxiom.PXConstant.PXDATEFORMAT));
                                                    }

                                                }

                                                if (string.IsNullOrEmpty(item.SortCode))
                                                {
                                                    item.SortCode = item.Text;
                                                }
                                            };
                                            m.Restriction = item =>
                                            {
                                                return true;
                                            };
                                        });
            retMenu.RootItem.Sort();

            return retMenu;
        }
    }
}
