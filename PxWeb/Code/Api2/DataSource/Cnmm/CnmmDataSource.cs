using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using PCAxis.Paxiom;
using Px.Abstractions;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;
using PxWeb.Mappers;
using System.Text;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class CnmmDataSource : IDataSource
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;
        private readonly IItemSelectionResolver _itemSelectionResolver;
        private readonly ITablePathResolver _tablePathResolver;
        private readonly ICodelistMapper _codelistMapper;

        public CnmmDataSource(ICnmmConfigurationService cnmmConfigurationService, IItemSelectionResolver itemSelectionResolver, ITablePathResolver tablePathResolver, ICodelistMapper codelistMapper)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
            _tablePathResolver = tablePathResolver;
            _codelistMapper = codelistMapper;
        }

        public IPXModelBuilder? CreateBuilder(string id, string language)
        {
            var builder = new PCAxis.PlugIn.Sql.PXSQLBuilder();
            var path = _tablePathResolver.Resolve(language, id, out bool selctionExists);

            if (selctionExists)
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

                                tbl.Text = CreateTableTitleWithInterval(tbl);

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

        public Codelist? GetCodelist(string id, string language)
        {
            Codelist? codelist = null;

            if (string.IsNullOrEmpty(id))
            {
                return codelist;
            }

            if (id.StartsWith("agg_", System.StringComparison.InvariantCultureIgnoreCase))
            {
                codelist = GetGrouping(id, language);
            }
            else if (id.StartsWith("vs_", System.StringComparison.InvariantCultureIgnoreCase))
            {
                codelist = GetValueset(id, language);   
            }

            return codelist;
        }

        public bool TableExists(string tableId, string language)
        {
            bool selectionExists;
            
            _itemSelectionResolver.Resolve(language, tableId, out selectionExists);
            return selectionExists;
        }

        private string CreateTableTitleWithInterval(TableLink child)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(child.Text);

            if (IsInteger(child.Text[child.Text.Length - 1].ToString())) //Title ends with a number, add nothing
            {
                return sb.ToString();
            }
            if (string.IsNullOrEmpty(child.StartTime) || string.IsNullOrEmpty(child.EndTime)) //No starttime or endtime, add nothing
            {
                return sb.ToString();
            }
            if (child.Text.EndsWith("-"))//Title ends with a dash, only endtime should be added
            {
                sb.Append(child.EndTime);
                return sb.ToString();
            }
            if (child.StartTime == child.EndTime) //Starttime and Endtime are the same, only starttime should be added
            {
                sb.Append(" ");
                sb.Append(child.StartTime);
                return sb.ToString();
            }

            if (child.StartTime.Contains("-"))
            {
                sb.Append(" (");
                sb.Append(child.StartTime);
                sb.Append(")-(");
                sb.Append(child.EndTime);
                sb.Append(")");
            }
            else
            {
                sb.Append(" ");
                sb.Append(child.StartTime);
                sb.Append("-");
                sb.Append(child.EndTime);
            }

            return sb.ToString();
        }
       
        private static bool IsInteger(string value)
        {
            int outValue;

            return int.TryParse(value, out outValue);
        }

        private Codelist? GetGrouping(string id, string language)
        {
            Codelist? codelist = null;
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();

            if (id.StartsWith("agg_", System.StringComparison.InvariantCultureIgnoreCase))
            {
                // Remove leading "agg_" from id
                id = id.Substring(4);
            }

            PCAxis.Sql.Repositories.GroupingRepository repo = new PCAxis.Sql.Repositories.GroupingRepository(cnmmOptions.DatabaseID);
            PCAxis.Sql.Models.Grouping grouping = repo.GetGrouping(id, language);

            if (grouping != null)
            {
                codelist = _codelistMapper.Map(grouping);
            }

            return codelist;
        }

        private Codelist? GetValueset(string id, string language)
        {
            Codelist? codelist = null;
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();

            if (id.StartsWith("vs_", System.StringComparison.InvariantCultureIgnoreCase))
            {
                // Remove leading "vs_" from id
               id = id.Substring(3);
            }

            PCAxis.Sql.Repositories.ValueSetRepository repo = new PCAxis.Sql.Repositories.ValueSetRepository(cnmmOptions.DatabaseID);
            PCAxis.Sql.Models.ValueSet valueset = repo.GetValueSet(id, language);

            if (valueset != null)
            {
                codelist = _codelistMapper.Map(valueset);
            }

            return codelist;
        }
    }
}
