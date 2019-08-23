using System;
using System.Collections.Generic;
using System.Text;

namespace PCAxis.Sql.DbConfig
{
    public class Tab
    {
       
        // The actual name of the table in the database
        private string localTableName;
        // The alias of the table 
        private string tableAlias;
        private string metaOwner;
        private string nameAndAlias;
       
        /// <summary>
        /// Local table name
        /// </summary>
        public String TableName
        {
            get { return localTableName; }
        }
        
        /// <summary>
        /// Table alias
        /// </summary>
        public String Alias
        {
            get { return tableAlias; }
        }

        internal Tab(string tableAlias, string localTableName, string metaOwner)
        {
            this.tableAlias = tableAlias;
            this.localTableName = localTableName;
            this.metaOwner = metaOwner;
            this.nameAndAlias = metaOwner + localTableName + " " + tableAlias;
        }

        /// <summary>
        /// metaOwner + localTableName + " " + tableAlias
        /// </summary>
        /// <returns></returns>
          public string GetNameAndAlias()
            {
                return this.nameAndAlias;
            }

          }
}
