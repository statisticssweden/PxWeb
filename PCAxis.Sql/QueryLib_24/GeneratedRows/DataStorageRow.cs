using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_24
{

    /// <summary>
    /// Holds the attributes for DataStorage. (This entity is language independent.)
    /// 
    /// The table contains information on which database the data tables for the statistical product are stored in.
    /// </summary>
    public class DataStorageRow
    {
        private String mProductCode;
        /// <summary>
        /// Unique identifier for at product group.
        /// </summary>
        public String ProductCode
        {
            get { return mProductCode; }
        }
        private String mServerName;
        /// <summary>
        /// Name of the server where the database is situated.
        /// </summary>
        public String ServerName
        {
            get { return mServerName; }
        }
        private String mDatabaseName;
        /// <summary>
        /// Name of the database where the product's data tables are stored.
        /// </summary>
        public String DatabaseName
        {
            get { return mDatabaseName; }
        }

        public DataStorageRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mProductCode = myRow[dbconf.DataStorage.ProductCodeCol.Label()].ToString();
            this.mServerName = myRow[dbconf.DataStorage.ServerNameCol.Label()].ToString();
            this.mDatabaseName = myRow[dbconf.DataStorage.DatabaseNameCol.Label()].ToString();
        }
    }
}
