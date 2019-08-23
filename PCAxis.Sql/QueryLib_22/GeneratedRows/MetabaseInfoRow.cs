using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_22
{

    /// <summary>
    /// Holds the attributes for MetabaseInfo. (This entity is language independent.) 
    /// 
    /// The table contains information on the relevant data model, macro/micro, version and the database role (see further description of respective columns).\nUsed by the indata program, which via its .ini file carries out a check at the beginning that the program is run against the right version of the metadata model. (By right version, it is meant the version that the respective program is adapted for and that it can manage with the right result). If the program is run against the wrong version of the data model, an error message is received and the program is interrupted.
    /// </summary>
    public class MetabaseInfoRow
    {
        private String mModel;
        /// <summary>
        /// Macro or micro.
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
        /// Role of database. Can be:\n- Production\n- Operation\n- Text
        /// </summary>
        public String DatabaseRole
        {
            get { return mDatabaseRole; }
        }

        public MetabaseInfoRow(DataRow myRow, SqlDbConfig_22 dbconf)
        {
            this.mModel = myRow[dbconf.MetabaseInfo.ModelCol.Label()].ToString();
            this.mModelVersion = myRow[dbconf.MetabaseInfo.ModelVersionCol.Label()].ToString();
            this.mDatabaseRole = myRow[dbconf.MetabaseInfo.DatabaseRoleCol.Label()].ToString();
        }
    }
}
