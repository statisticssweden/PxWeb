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
    public static class PCAxisRepository
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PCAxisRepository));

        /// <summary>
        /// Gets the menu object for the specified database
        /// </summary>
        /// <remarks></remarks>
        /// <returns>A menu object</returns>
        public static Item GetMenu(string db, string language, string[] nodePath)
        {
            if (ExposedDatabases.DatabaseConfigurations[language].ContainsKey(db) == false)
                return null;

            var database = ExposedDatabases.DatabaseConfigurations[language][db];
            var dbtype = database.Type;
            try
            {
                if (dbtype == "PX")
                {
                    return GetPxMenu(db, language, string.Join("\\", nodePath));
                }
                else if (dbtype == "CNMM")
                {
                    string tid = nodePath.Last();
                    string menu = nodePath.Length > 1 ? nodePath[nodePath.Length-2]:"START";

                    if (tid.Contains("'") || menu.Contains("'")) throw new ArgumentException("Possible SQL injection");
                    return GetCnmmMenu(db, language, tid, menu);
                }
                return null;
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

                                    if (string.Compare(tbl.ID.Selection, nodeId, false) == 0)
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

                selections.Add(selection);
               
            }
            return selections;
        }

    }
}