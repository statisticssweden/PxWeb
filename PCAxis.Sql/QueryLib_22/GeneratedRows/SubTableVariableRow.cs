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
    /// Holds the attributes for SubTableVariable. (This entity is language independent.) 
    /// 
    /// The table links variables with value sets in the sub-tables.  The variable name is made up of the name for the corresponding metadata column in the data tables.
    /// </summary>
    public class SubTableVariableRow
    {
        private String mMainTable;
        /// <summary>
        /// The name of the main table to which the variable and the sub-table are linked. See further description in the MainTable table. 
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mSubTable;
        /// <summary>
        /// Name of sub-table\nData can be missing, with a dash in its place.\nSee further description in the table SubTable.
        /// </summary>
        public String SubTable
        {
            get { return mSubTable; }
        }
        private String mVariable;
        /// <summary>
        /// Variable name, which makes up the column name for metadata in the data table. See further description in the table Variable.
        /// </summary>
        public String Variable
        {
            get { return mVariable; }
        }
        private String mValueSet;
        /// <summary>
        /// Name of value set. See further description in the table ValueSet.\nFor rows with variable types V and G, the name of the value set must be filled in. For VariableType = T, the field is left empty, as there is no value set for the variable Time.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mVariableType;
        /// <summary>
        /// Code for type of variable. There are three alternatives:\n- V = variable, i.e. dividing variable, not time.\n- G = geographical information for map program. \nAt statistics \n,\n Not yet implemented.\n- T = time.\nIf VariableType = G, the field GeoAreaNo in the tables ValueSet and Grouping should be filled in (however not yet implemented).
        /// </summary>
        public String VariableType
        {
            get { return mVariableType; }
        }
        private String mStoreColumnNo;
        /// <summary>
        /// The variable's column number in the data table.\nThe variable Time should always be included and be the last column in the data table. If the material is divided by region, the variable Region should be the first column.\nWritten as 1, 2, 3, etc.
        /// </summary>
        public String StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }

        public SubTableVariableRow(DataRow myRow, SqlDbConfig_22 dbconf)
        {
            this.mMainTable = myRow[dbconf.SubTableVariable.MainTableCol.Label()].ToString();
            this.mSubTable = myRow[dbconf.SubTableVariable.SubTableCol.Label()].ToString();
            this.mVariable = myRow[dbconf.SubTableVariable.VariableCol.Label()].ToString();
            this.mValueSet = myRow[dbconf.SubTableVariable.ValueSetCol.Label()].ToString();
            this.mVariableType = myRow[dbconf.SubTableVariable.VariableTypeCol.Label()].ToString();
            this.mStoreColumnNo = myRow[dbconf.SubTableVariable.StoreColumnNoCol.Label()].ToString();
        }
    }
}
