using DocumentFormat.OpenXml.Office2010.Excel;
using PCAxis.Menu;
using PCAxis.Paxiom;
using PXWeb.Code.API.Interfaces;
using PXWeb.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXWeb.Code.API.Services
{
    public class TableService : ITableService
    {

        private readonly log4net.ILog _logger;

        public TableService(log4net.ILog logger) 
        {
            _logger = logger;        
        }
        /// <summary>
        /// Get all tables from the specified database and language.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        /// <param name="language">The language.</param>
        /// <returns>A list of table links.</returns>
        public List<TableLink> GetAllTables(string database, string language)
        {
            var root = PxContext.GetMenu(database, "", language, 10);
            var tables = new List<TableLink>();
            AddTables(root.CurrentItem, tables);

            return tables;
        }

        /// <summary>
        /// Retrieves the table model for the specified database, selection, and language.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        /// <param name="selection">The selection criteria.</param>
        /// <param name="language">The language.</param>
        /// <returns>The table model.</returns>
        public PXModel GetTableModel(string database, string selection, string language)
        {
            try
            {
                var builder = PxContext.CreatePaxiomBuilder(database, selection);
                builder.SetPreferredLanguage(language);
                builder.BuildForSelection();
                builder.BuildForPresentation(PCAxis.Paxiom.Selection.SelectAll(builder.Model.Meta));
                return builder.Model;
            } catch (Exception ex)
            {
                _logger.Warn($"Failed to get table model for database {database}, selection {selection}, language {language}", ex);
            }
            return null;
        }


        /// <summary>
        /// Recursively adds tables from the specified menu item to the list of table links.
        /// </summary>
        /// <param name="item">The menu item to process.</param>
        /// <param name="tables">The list of table links to add to.</param>
        private void AddTables(PCAxis.Menu.Item item, List<TableLink> tables)
        {
            if (item is PxMenuItem)
            {
                foreach (var child in ((PxMenuItem)item).SubItems)
                {
                    AddTables(child, tables);
                }
            }
            else if (item is TableLink)
            {
                tables.Add((TableLink)item);
            }
        }
    }
}