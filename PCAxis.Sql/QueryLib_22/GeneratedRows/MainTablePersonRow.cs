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
    /// Holds the attributes for MainTablePerson. (This entity is language independent.) 
    /// 
    /// The table links the person responsible, i.e. the contact person and person responsible for updating, to the main tables. An unlimited number of persons can be linked to a main table.
    /// </summary>
    public class MainTablePersonRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of main table.\nSee further in the description of the table MainTable. 
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mPersonCode;
        /// <summary>
        /// Code for the person responsible, contact person or person responsible for updating for the main table.\nFor description, see the table Person. 
        /// </summary>
        public String PersonCode
        {
            get { return mPersonCode; }
        }
        private String mRolePerson;
        /// <summary>
        /// Code that shows the role of responsible person, contact person and/or person responsible for updating. There are the following alternatives:\nM = Main contact person (one person)\nC = Contact person (0 – several persons)\nU = Person responsible for updating (1 – several persons)\nI = Person responsible for international reporting (0 - 1 person). \nAt statistics \n,\n This code is not in use\nV = Person that verifies not yet published data  (0 – several persons)\nAt statistics \n,\n The maximum number of contact persons allowed at present is two and two persons responsible for updating. 
        /// </summary>
        public String RolePerson
        {
            get { return mRolePerson; }
        }

        public MainTablePersonRow(DataRow myRow, SqlDbConfig_22 dbconf)
        {
            this.mMainTable = myRow[dbconf.MainTablePerson.MainTableCol.Label()].ToString();
            this.mPersonCode = myRow[dbconf.MainTablePerson.PersonCodeCol.Label()].ToString();
            this.mRolePerson = myRow[dbconf.MainTablePerson.RolePersonCol.Label()].ToString();
        }
    }
}
