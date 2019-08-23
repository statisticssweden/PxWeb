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
    /// Holds the attributes for MetabaseInfo. (This entity is language independent.)
    /// 
    /// The table contains information on the relevant data model,version and the database role.
    /// </summary>
    public class MetabaseInfoRow
    {
        private String mModel;
        /// <summary>
        /// This field can be used to give information about general characteristics of the database.
        /// E.g. if the data is on macro or micro level.
        /// </summary>
        public String Model
        {
            get { return mModel; }
        }
        private String mModelVersion;
        /// <summary>
        /// Version number for the metadata model that the metadata database uses.
        /// </summary>
        public String ModelVersion
        {
            get { return mModelVersion; }
        }
        private String mDatabaseRole;
        /// <summary>
        /// Role of database. Can be:
        /// 
        /// - Production
        /// - Operation
        /// - Test
        /// </summary>
        public String DatabaseRole
        {
            get { return mDatabaseRole; }
        }

        public MetabaseInfoRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mModel = myRow[dbconf.MetabaseInfo.ModelCol.Label()].ToString();
            this.mModelVersion = myRow[dbconf.MetabaseInfo.ModelVersionCol.Label()].ToString();
            this.mDatabaseRole = myRow[dbconf.MetabaseInfo.DatabaseRoleCol.Label()].ToString();
        }
    }
}
