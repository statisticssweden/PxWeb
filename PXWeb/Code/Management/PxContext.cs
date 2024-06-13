using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text;
using PCAxis.Web.Core.Management;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using System.Diagnostics;
using PCAxis.Web.Controls;
using log4net;
using PX.Web.Interfaces.Cache;
using PXWeb.Code.Management;

namespace PXWeb.Management
{
    /// <summary>
    /// Class for managing the Paxiom model in PX-Web
    /// </summary>
    public class PxContext
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PxContext));

        public static ICacheService CacheService { get; set; }
        
        //Controller for all of the PxWeb caches
        public static IPxCacheController CacheController { get; set; }

        /// <summary>
        /// Get the Paxiom model built for selection (only metadata)
        /// </summary>
        /// <param name="db">Database id</param>
        /// <param name="path">Path to PX-file or datatable</param>
        /// <param name="table">Table name (PX-file or CNMM datatable)</param>
        /// <param name="prefLang">Preferred language</param>
        /// <returns>The Paxiom model built for selection</returns>
        public static PCAxis.Paxiom.PXModel GetPaxiomForSelection(string db, string path, string table, string prefLang, bool clearModel)
        {
            //Verify that user is authorized to view the table
            if (!AuthorizationUtil.IsAuthorized(db, path, table)) //TODO: Should be dbid, menu and selection. Only works for SCB right now... (2018-11-14)
            {
                HttpContext.Current.Response.Redirect(LinkManager.CreateLink("~/Menu.aspx", new LinkManager.LinkItem() { Key = "msg", Value = "UnauthorizedTable" }));
            }

            string tablePath;
            //path = ItemSelectionHelper.CreateFromString(path).Selection;
            
            PCAxis.Paxiom.IPXModelBuilder builder;
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);

            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM && !CnmmDatabaseRootHelper.Check(path))
            {
                HttpContext.Current.Response.Redirect(LinkManager.CreateLink("~/Menu.aspx", new LinkManager.LinkItem() { Key = "msg", Value = "UnauthorizedTable" }));
            }

            PCAxis.Web.Controls.PathHandler pHandler;

            pHandler = PCAxis.Web.Controls.PathHandlerFactory.Create(dbi.Type);
            tablePath = pHandler.CombineTable(db, path, table);

            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder == null)
            {
                builder = CreatePaxiomBuilder(db, tablePath);
                PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = builder;
            }
            else
            {
                string newpath;

                if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.PX)
                {
                    // PX
                    newpath = System.IO.Path.Combine(
                        System.Web.HttpContext.Current.Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath),
                        tablePath);
                }
                else
                {
                    // CNMM
                    newpath = tablePath;
                }

                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder.Path.Equals(newpath) && (clearModel == false)) 
                {
                    // Existing builder is built against the wanted table
                    builder = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder;
                }
                else
                {
                    // Existing builder is built against a differnt builder - Create new builder!!!
                    builder = CreatePaxiomBuilder(db, tablePath);
                    PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = builder;
                }
            }

            if (builder.BuilderState < PCAxis.Paxiom.ModelBuilderStateType.BuildForSelection)
            {
                string lang;

                if (!string.IsNullOrEmpty(prefLang))
                {
                    lang = prefLang;
                }
                else if (!string.IsNullOrEmpty(PXWeb.Settings.Current.General.Language.DefaultLanguage))
                {
                    lang = PXWeb.Settings.Current.General.Language.DefaultLanguage;
                }
                else
                {
                    lang = "en";
                }

                if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
                {
                    // Check if the language exist in the CNMM database, if not use the default language of the database
                    string dbid = PxContext.GetDbId(tablePath);
                    lang = PxContext.GetCnmmDbLanguage(dbid, lang);
                }

                builder.SetPreferredLanguage(lang);
                builder.BuildForSelection();
            }
            
            return builder.Model;
        }

        /// <summary>
        /// Creates a builder of the desired type
        /// </summary>
        /// <param name="db">Database id</param>
        /// <param name="path">Path to PX-file or datatable</param>
        /// <returns>The created Paxiom model builder</returns>
        protected static internal PCAxis.Paxiom.IPXModelBuilder CreatePaxiomBuilder(string db, string path)
        {
            PCAxis.Paxiom.IPXModelBuilder builder = null;
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);

            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.PX)
            {
                string fullPath = System.IO.Path.Combine(
                    System.Web.HttpContext.Current.Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath),
                    path);
                builder = new PCAxis.Paxiom.PXFileBuilder();
                builder.SetPath(fullPath);
            }
            else
            {
                //it seems the incomming path here already is db:tableid . (In the paralell method in SSDHandler.cs it is not) 
                builder = new PCAxis.PlugIn.Sql.PXSQLBuilder();
                if (path.Contains(":"))
                {
                    builder.SetPath(path);
                }
                else
                {
                    builder.SetPath(db + ":" + path);
                }
            }

            return builder;
       }

        public static PxMenuBase GetMenu(string db, string nodeId)
        {
            return GetMenu(db, nodeId, PCAxis.Web.Core.Management.LocalizationManager.GetTwoLetterLanguageCode());
        }


        /// <summary>
        /// Gets the menu object for the specified database
        /// </summary>
        /// <remarks></remarks>
        /// <returns>A menu object</returns>
        public static PxMenuBase GetMenu(string db, string nodeId, string lang, int noOfLevels = 1)
        {
            //Verifies that the user is authorized to view the database
            var dbcfg = Settings.Current.Database[db];
            if (dbcfg != null)
            {
                if (dbcfg.Protection.IsProtected)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.Debug("Check if authenticated");
                    }
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        //TODO Auth
                        //PXWeb.Security.IAuthentication auth = GetAuthentication(dbcfg.Protection.AuthenticationMethod);
                        //auth.Autenticate();
                    }
                    else
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("Loged on user: " + HttpContext.Current.User.Identity.Name);
                        }
                    }

                    if (!dbcfg.Protection.AuthorizationMethod.IsAuthorized(db))
                    {
                        HttpContext.Current.Response.Redirect(LinkManager.CreateLink("~/Default.aspx", new LinkManager.LinkItem() { Key = "msg", Value = "UnauthorizedDatabase" }));
                    }
                }
            }

            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);

            if (dbi == null)
            {
                return null;
            }

            try
            {
                if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.PX)
                {
                    return GetPxMenu(db, nodeId, lang);
                }
                else if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
                {
                    return GetCnmmMenu(db, nodeId, lang, noOfLevels);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Error("Something went wrong when trying to getMenu", e);
                return null;
            }
        }


        /// <summary>
        /// Gets the PC-Axis file based menu object
        /// </summary>
        /// <returns>A PX file Menu object</returns>
        private static PxMenuBase GetPxMenu(string dbid, string nodeId, string lang)
        {
            PCAxis.Menu.Item menuItem = null;
            return GetPxMenuAndItem(dbid, nodeId, lang, out menuItem);
        }

        /// <summary>
        /// Gets the CNMM menu object
        /// </summary>
        /// <returns>A Common Nordic Metamodel Menu object</returns>
        private static PxMenuBase GetCnmmMenu(string dbid, string nodeId, string lang, int noOfLevels = 1)
        {
            PCAxis.Menu.Item menuItem = null;
            return GetCnmmMenuAndItem(dbid, nodeId, lang, out menuItem, noOfLevels);
        }

        public static PCAxis.Menu.Item GetMenuItem(string db, string nodeId)
        {
            return GetMenuItem(db, nodeId, PCAxis.Web.Core.Management.LocalizationManager.GetTwoLetterLanguageCode());
        }

        /// <summary>
        /// Get the current menu item
        /// </summary>
        /// <param name="db">Database</param>
        /// <param name="nodeId">Node id</param>
        /// <returns></returns>
        public static PCAxis.Menu.Item GetMenuItem(string db, string nodeId, string lang)
        {
            Item item = null;
            if (PxContext.CacheService != null)
            {
                string key = $"pxc_menu_{db}_{lang}_{nodeId}";
                item = PxContext.CacheService.Get<Item>(key);
                if (item != null)
                {
                    return item;
                }
            }
            
           
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);

            if (dbi == null)
            {
                return null;
            }

            

            try
            {
                if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.PX)
                {
                    item = GetPxMenuItem(db, nodeId, lang);
                }
                else if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
                {
                    item = GetCnmmMenuItem(db, nodeId, lang);
                }
                if (PxContext.CacheService != null)
                {
                    string key = $"pxc_menu_{db}_{lang}_{nodeId}";
                    
                    if (item != null)
                    {
                        PxContext.CacheService.Set(key, item);
                    }
                }
                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Menu + current item
        /// </summary>
        /// <param name="dbid">Database id</param>
        /// <param name="node">Database node</param>
        /// <param name="lang">Language</param>
        /// <param name="currentItem">Current item (out parameter)</param>
        /// <returns></returns>
        public static PxMenuBase GetMenuAndItem(string dbid, ItemSelection node, string lang, out PCAxis.Menu.Item currentItem)
        {
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(dbid);
            PCAxis.Menu.Item menuItem = null;
            PxMenuBase menu = null;

            try
            {
                if (dbi != null)
                {
                    string nodeId = PathHandlerFactory.Create(dbi.Type).GetPathString(node);

                    if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.PX)
                    {
                        menu = GetPxMenuAndItem(dbid, nodeId, lang, out menuItem);
                    }
                    else if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
                    {
                        menu = GetCnmmMenuAndItem(dbid, nodeId, lang, out menuItem);
                    }
                }
                currentItem = menuItem;
                return menu;
            }
            catch (Exception)
            {
                currentItem = null;
                return null;
            }
        }

        private static PCAxis.Menu.Item GetPxMenuItem(string dbid, string nodeId, string lang)
        {
            PCAxis.Menu.Item menuItem = null;
            GetPxMenuAndItem(dbid, nodeId, lang, out menuItem);

            return menuItem;
        }

        private static PCAxis.Menu.Item GetCnmmMenuItem(string dbid, string nodeId, string lang)
        {
            PCAxis.Menu.Item menuItem = null;
            GetCnmmMenuAndItem(dbid, nodeId, lang, out menuItem);

            return menuItem;
        }

        private static PxMenuBase GetPxMenuAndItem(string dbid, string nodeId, string lang, out PCAxis.Menu.Item currentItem)
        {
            //Get menu-file
            DatabaseInfo currentdb = PXWeb.Settings.Current.General.Databases.GetPxDatabase(dbid);
            if (currentdb != null)
            {
                StringBuilder sb = new StringBuilder(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);
                sb.Append("/");
                sb.Append(currentdb.Id);
                sb.Append("/");
                sb.Append(PXWeb.Settings.Current.General.Databases.PxDatabaseFilename);
                //string _xmlFile = HttpContext.Current.Server.MapPath(sb.ToString());
                string _xmlFile = System.Web.Hosting.HostingEnvironment.MapPath(sb.ToString());
                XmlMenu menu = new XmlMenu(XDocument.Load(_xmlFile), lang,
                    m =>
                    {
                        m.Restriction = item =>
                        {
                            return true;
                        };
                    });

                ItemSelection cid = PathHandlerFactory.Create(PCAxis.Web.Core.Enums.DatabaseType.PX).GetSelection(nodeId);
                menu.SetCurrentItemBySelection(cid.Menu, cid.Selection);
                currentItem = menu.CurrentItem;
                return menu;
            }
            currentItem = null;
            return null;
        }

        private static PxMenuBase GetCnmmMenuAndItem(string dbid, string nodeId, string lang, out PCAxis.Menu.Item currentItem, int noOfLevels = 1)
        {
            nodeId = CnmmDatabaseRootHelper.GetId(nodeId);

            //Get selected language in PX-Web
            //string pxLang = PCAxis.Web.Core.Management.LocalizationManager.GetTwoLetterLanguageCode();
            //string dbLang = PxContext.GetCnmmDbLanguage(dbid, pxLang);
            string dbLang = PxContext.GetCnmmDbLanguage(dbid, lang);
            TableLink tblFix = null;

            //Create database object to return
            DatamodelMenu menu = ConfigDatamodelMenu.Create(
                        dbLang,
                        PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[dbid],
                        m =>
                        {
                            m.NumberOfLevels = noOfLevels;
                            m.RootSelection = string.IsNullOrEmpty(nodeId) ? new ItemSelection() : PathHandlerFactory.Create(PCAxis.Web.Core.Enums.DatabaseType.CNMM).GetSelection(nodeId);
                            m.AlterItemBeforeStorage = item =>
                            {
                                if (item is PCAxis.Menu.Url)
                                {
                                    PCAxis.Menu.Url url = (PCAxis.Menu.Url)item;
                                }

                                if (item is TableLink)
                                {
                                    TableLink tbl = (TableLink)item;
                                    string tblId = tbl.ID.Selection;
                                    if (!string.IsNullOrEmpty(dbid))
                                    {
                                        tbl.ID = new ItemSelection(item.ID.Menu, dbid + ":" + tbl.ID.Selection); // Hantering av flera databaser!
                                    }

                                    CustomizeTableTitle(tbl, false);

                                    if (tbl.Published.HasValue)
                                    {
                                        // Store date in the PC-Axis date format
                                        tbl.SetAttribute("modified", tbl.Published.Value.ToString(PCAxis.Paxiom.PXConstant.PXDATEFORMAT));
                                    }
                                    if (string.Compare(tblId + item.ID.Menu, PathHandlerFactory.Create(PCAxis.Web.Core.Enums.DatabaseType.CNMM).GetSelection(nodeId).Selection + PathHandlerFactory.Create(PCAxis.Web.Core.Enums.DatabaseType.CNMM).GetSelection(nodeId).Menu, true) == 0)
                                    {
                                        tblFix = tbl;
                                    }
                                }

                                if (String.IsNullOrEmpty(item.SortCode))
                                {
                                    item.SortCode = item.Text;
                                }
                            };
                            m.Restriction = item =>
                            {
                                return true;
                            };
                        });

            if (tblFix != null)
            {
                currentItem = tblFix;
            }
            else
            {
                currentItem = menu.CurrentItem;
            }

            menu.RootItem.Sort();

            return menu;
        }



        /// <summary>
        /// Customize the table title
        /// </summary>
        /// <param name="tbl">TableLink object</param>
        /// <param name="showPublished">If published date shall be displayed or not</param>
        private static void CustomizeTableTitle(TableLink tbl, bool showPublished)
        {
            string published = "";

            if (showPublished)
            {
                published = tbl.Published.HasValue ? string.Format("<em> [{0}]</em>", tbl.Published.Value.ToShortDateString()) : "";
            }

            string tableText = tbl.Text;

            try
            {
                if (IsInteger(tbl.Text[tbl.Text.Length - 1].ToString()))
                {
                    tbl.Text = string.Format("{0}{1}", tbl.Text, published);
                }
                else if (tbl.Text[tbl.Text.Length - 1].ToString() == "-") //Specialfall när titeln slutar med streck. Då ska bara slutår läggas till.
                {
                    tbl.Text = string.Format("{0} {1}{2}", tbl.Text, tbl.EndTime, published);
                }
                else if (tbl.StartTime == tbl.EndTime)
                {
                    tbl.Text = string.Format("{0} {1}{2}", tbl.Text, tbl.StartTime, published);
                }
                else
                {
                    tbl.Text = string.Format("{0} {1} - {2}{3}", tbl.Text, tbl.StartTime, tbl.EndTime, published);
                }
            }
            catch (Exception)
            {
                tbl.Text = tableText;
            }

            //TODO: Mark table with icon if it is updated after it was published

            //if (SSDDatabase.IsTableUpdated(tbl.Selection.Split(':')[1], tbl.Selection.Split(':')[0]))
            //{
            //    var tableIsUpdatedText = Translate("/templates/ssd/treeview/tableupdatedafterpublishing");
            //    tbl.Text += "&nbsp;<img src=\"/Pages/Images/refresh.png\" alt=\"" + tableIsUpdatedText + "\" title=\"" + tableIsUpdatedText + "\"></img>";
            //}

        }

        /// <summary>
        /// Check if string is integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if integer, else false</returns>
        private static bool IsInteger(string value)
        {
            int outValue;

            return int.TryParse(value, out outValue);
        }

        /// <summary>
        /// Get language for CNMM database
        /// </summary>
        /// <param name="dbid">Database id</param>
        /// <param name="preferredLanguage">The preferred language</param>
        /// <returns>The language in which the CNMM database will be displayed</returns>
        private static string GetCnmmDbLanguage(string dbid, string preferredLanguage)
        {
            //Get database info object
            CnmmDatabaseInfo dbi = (CnmmDatabaseInfo)PXWeb.Settings.Current.General.Databases.GetCnmmDatabase(dbid);

            // Set database language
            if ((preferredLanguage == dbi.DefaultLanguage) || (dbi.OtherLanguages.Contains(preferredLanguage)))
            {
                return preferredLanguage;
            }
            else
            {
                return dbi.DefaultLanguage;
            }
        }

        /// <summary>
        /// Get id of database from table path
        /// </summary>
        /// <param name="path">Path to a table</param>
        /// <returns>Id of the database</returns>
        private static string GetDbId(string path)
        {
            if (path.IndexOf(":") == -1)
            {
                return path;
            }
            else
            {
                return path.Substring(0, path.IndexOf(":"));
            }
        }

        private static void AlterMenuItem(Item item)
        { 
            
        }
    }
}
