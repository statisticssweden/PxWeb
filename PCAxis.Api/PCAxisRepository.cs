using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using PCAxis.Query;
using System.Text;
using System.Configuration;
using System.Xml.Linq;

namespace PCAxis.Api
{

    public class MenuObject
    {
        public ItemSelection ID { get; set; }

        public bool HasSubItems { get; set; }

        public Type MenuType { get; set; }

        public IEnumerable<MetaList> MetaList { get; set; }

        public static MenuObject Create(Item item)
        {

            if (item is null) return null;

            MenuObject menu = new MenuObject();

            menu.ID = item.ID;
            menu.MenuType = item.GetType();

            if (item is PxMenuItem)
            {
                PxMenuItem mi = (PxMenuItem)item;
                menu.HasSubItems = mi.HasSubItems;

                if (menu.HasSubItems)
                {
                    menu.MetaList = GetMetaList(mi);
                }

            }
            else
            {
                menu.HasSubItems = false;
            }

            return menu;
        }

        /// <summary>
        /// Returns a list of metadata for the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IEnumerable<MetaList> GetMetaList(PxMenuItem item)
        {
            // Logs usage
           // _usageLogger.Info(String.Format("url={0}, type=metadata, caller={1}, cached=false", context.Request.RawUrl, context.Request.UserHostAddress));

            return item.SubItems.Select(i => new MetaList
            {
                Id = i.ID.Selection.Replace('\\', '/'),
                Text = i.Text,
                Type = GetMetaListType(i),
                Updated = i is TableLink ? (((TableLink)i).Published) : null
            });
        }



        /// <summary>
        /// Returns the coded type indicating that the item  t(able), h(eadline) or l(ink)/folder 
        /// </summary>
        /// <param name="menuItem">the item </param>
        /// <returns>the coded type</returns>
        private static string GetMetaListType(Item menuItem)
        {
            if (menuItem is TableLink)
            {
                return "t";
            }
            else if (menuItem is Headline)
            {
                return "h";
            }
            else
            {
                return "l";
            }
        }

    }


public static class PCAxisRepository
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PCAxisRepository));

        /// <summary>
        /// Gets the menu object for the specified database
        /// </summary>
        /// <remarks></remarks>
        /// <returns>A menu object</returns>
        public static MenuObject GetMenu(string db, string language, string[] nodePath)
        {
            if (ExposedDatabases.DatabaseConfigurations[language].ContainsKey(db) == false)
                return null;

            var database = ExposedDatabases.DatabaseConfigurations[language][db];
            var dbtype = database.Type;
            string cacheKey = ApiCache.CreateKey($"{db}_{language}_{string.Join("_", nodePath)}");
            MenuObject menuObj = null;
            
            ResponseBucket cacheResponse = ApiCache.Current.Fetch(cacheKey);

            if (cacheResponse != null)
            {
                return cacheResponse.Menu;
            }

            try
            {
                if (dbtype == "PX")
                {
                    menuObj = MenuObject.Create(GetPxMenu(db, language, string.Join("\\", nodePath)));
                }
                else if (dbtype == "CNMM")
                {
                    string tid = nodePath.Last();
                    string menu = nodePath.Length > 1 ? nodePath[nodePath.Length-2]:"START";

                    if (tid.Contains("'") || menu.Contains("'")) throw new ArgumentException("Possible SQL injection");
                    menuObj = MenuObject.Create(GetCnmmMenu(db, language, tid, menu));
                }

                // Request object to be stored in cache
                cacheResponse = new ResponseBucket();
                cacheResponse.Key = cacheKey;
                cacheResponse.Menu = menuObj;
                ApiCache.Current.Store(cacheResponse, new TimeSpan(24,0,0));

                return menuObj;
            }
            catch (Exception e)
            {
                _logger.Error("Error retrieving menu", e);
                return null;
            }
        }



        /// <summary>
        /// Gets the PC-Axis file based menu object
        /// </summary>
        /// <returns>A PX file Menu object</returns>
        private static Item GetPxMenu(string db, string language, string nodeId)
        {
            string menuXMLPath = System.IO.Path.Combine(ExposedDatabases.DatabaseConfigurations[language][db].RootPath, "menu.xml");
            XDocument doc =  XDocument.Load(menuXMLPath);
            XmlMenu menu = new XmlMenu(doc, language,
            m => { m.AlterItemBeforeStorage = item => { item.ID.Selection = System.IO.Path.GetFileName(item.ID.Selection); }; });

            ItemSelection cid = string.IsNullOrEmpty(nodeId) ? new ItemSelection() : new ItemSelection(System.IO.Path.GetDirectoryName(db + "\\" + nodeId), System.IO.Path.GetFileName(nodeId));
            menu.SetCurrentItemBySelection(cid.Menu, cid.Selection);

            // Check if menu level has been found or not
            if (cid.Menu != "START" && cid.Selection != "START")
            {
                if (menu.CurrentItem.ID.Menu == "" && menu.CurrentItem.ID.Selection == db)
                {
                    //Menu level has not been found in database
                    return new PxMenuItem(null);
                }
                else if ( ((nodeId.ToLower().Contains(".px")) && (!nodeId.Contains(@"\"))) && 
                          ((menu.CurrentItem.ID.Menu.Contains(@"\") && menu.CurrentItem.ID.Selection == nodeId)) )
                {
                    // 1. We have a .PX-file with no path (no \) specified in nodeID
                    // 2. SetCurrentItemBySelection however has found the path to the table and put this in CurrentItem.ID.Selection

                    // We need to return an empty PxMenuItem in this case and instead find the table via the search functionality.
                    // If we do not do this we will get an error in BuildForPresentation later on because of we do not have the cotrrect path to the table...
                    return new PxMenuItem(null);
                }
            }
            
            return menu.CurrentItem;
        }

        /// <summary>
        /// Gets the CNMM menu object
        /// </summary>
        /// <returns>A Common Nordic Metamodel Menu object</returns>
        private static Item GetCnmmMenu(string dbid, string language, string nodeId, string menuid)
        {
            string dbLang = language;
            TableLink tblFix = null;
            //Create database objecst to return
            DatamodelMenu menu = ConfigDatamodelMenu.Create(
                        dbLang,
                        PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[dbid],
                        m =>
                        {
                            m.RootSelection = nodeId == "" ? new ItemSelection() : new ItemSelection(menuid, nodeId);
                            m.AlterItemBeforeStorage = item =>
                            {
                                if (item is TableLink)
                                {
                                    TableLink tbl = (TableLink)item;

                                    if (string.Compare(tbl.ID.Selection, nodeId, true) == 0)
                                    {
                                        tblFix = tbl;
                                    }
                                    if (tbl.StartTime == tbl.EndTime)
                                    {
                                        tbl.Text = tbl.Text + " " + tbl.StartTime;
                                    }
                                    else
                                    {
                                        tbl.Text = tbl.Text + " " + tbl.StartTime + " - " + tbl.EndTime;
                                    }

                                    if (tbl.Published.HasValue)
                                    {
                                        tbl.SetAttribute("modified", tbl.Published.Value.ToShortDateString());
                                    }
                                }
                                if (String.IsNullOrEmpty(item.SortCode))
                                {
                                    item.SortCode = item.Text;
                                }
                            };
                        });

            
            return tblFix != null ? tblFix : menu.CurrentItem;
        }

        /// <summary>
        /// Builds a list of Selection-objects for each selection in the JSON request
        /// </summary>
        /// <param name="builder">The bulder of the table</param>
        /// <param name="tableQuery">The table query from the client</param>
        /// <returns>A list of selection objects</returns>
        public static List<PCAxis.Paxiom.Selection> BuildSelections(PCAxis.Paxiom.IPXModelBuilder builder, TableQuery tableQuery)
        {
            //Check to see that the variable exists
            int c = builder.Model.Meta.Variables.Where(var => tableQuery.Query.Select(q => q.Code).Contains(var.Code)).ToArray().Length;
            if (tableQuery.Query.Length > c) throw new ArgumentException("Variable is not defined");

            var selections = new List<PCAxis.Paxiom.Selection>();
            foreach (var variable in builder.Model.Meta.Variables)
            {
                
                PCAxis.Paxiom.Selection selection = new PCAxis.Paxiom.Selection(variable.Code);
                var query = tableQuery.Query.SingleOrDefault(q => q.Code == variable.Code);
                if (query != null)
                {
                    // Process filters
                    if (query.Selection.Filter.ToLower() == "all") // All
                    {
                        selection = QueryHelper.SelectAll(variable, query);
                    }
                    else if (query.Selection.Filter.ToLower() == "top") // Top
                    {
                        selection = QueryHelper.SelectTop(variable, query);
                    }
                    else if (PCAxis.Query.QueryHelper.IsAggregation(query.Selection.Filter))
                    {
                        selection = QueryHelper.SelectAggregation(variable, query, builder);
                    }
                    else if (query.Selection.Filter.StartsWith("vs:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        selection = QueryHelper.SelectValueSet(variable, query, builder);
                    }
                    else
                    {
                        // Assume item
                        selection = QueryHelper.SelectItem(variable, query);
                    }
                }
                else
                {
                    // Variable not specified in query
                    if (!variable.Elimination)
                        selection = PCAxis.Paxiom.Selection.SelectAll(variable);
                }

                if (selection != null)
                {
                    selections.Add(selection);
                }
                else
                {
                    //The user has requested an non valid selection
                    throw new HttpException(400,
                        $"The request for variable '{variable.Code}' has an error. Please check your query.");
                }
            }

            if (!ValidateSelection(builder, selections)) return null;

            return selections;
        }

        public static bool ValidateSelection(PCAxis.Paxiom.IPXModelBuilder builder, List<PCAxis.Paxiom.Selection> selections)
        {
            //Check that all mandatory variables has at least one value selected

            var mandatoryVariables = builder.Model.Meta.Variables.Where(v => !v.Elimination);

            foreach (var v in mandatoryVariables)
            {
                var selection = selections.FirstOrDefault(s => s.VariableCode == v.Code);
                if (selection == null) return false;
                if (selection.ValueCodes.Count == 0) return false;
            }

            return true;

        }
    }
}