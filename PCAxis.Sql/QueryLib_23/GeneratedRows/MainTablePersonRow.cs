using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_23
{

    /// <summary>
    /// Holds the attributes for MainTablePerson. (This entity is language independent.)
    /// 
    /// The table links the person responsible, i.e. the contact person and person responsible for updating, to the main tables. An unlimited number of persons can be linked to a main table.
    /// </summary>
    public class MainTablePersonRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of main table.
        /// 
        /// See further in the description of the table MainTable.
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mPersonCode;
        /// <summary>
        /// Code for the person responsible, contact person or person responsible for updating for the main table.
        /// 
        /// For description, see the table Person.
        /// </summary>
        public String PersonCode
        {
            get { return mPersonCode; }
        }
        private String mRolePerson;
        /// <summary>
        /// Code that shows the role of responsible person, contact person and/or person responsible for updating. There are the following alternatives:
        /// 
        /// M = Main contact person (one person)
        /// C = Contact person (0 – several persons)
        /// U = Person responsible for updating (1 – several persons)
        /// I = Person responsible for international reporting (0 - 1 person).
        /// V = Person that verifies not yet published data  (0 – several persons)
        /// </summary>
        public String RolePerson
        {
            get { return mRolePerson; }
        }

        public MainTablePersonRow(DataRow myRow, SqlDbConfig_23 dbconf)
        {
            this.mMainTable = myRow[dbconf.MainTablePerson.MainTableCol.Label()].ToString();
            this.mPersonCode = myRow[dbconf.MainTablePerson.PersonCodeCol.Label()].ToString();
            this.mRolePerson = myRow[dbconf.MainTablePerson.RolePersonCol.Label()].ToString();
        }
    }
}
