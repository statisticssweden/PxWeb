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
    /// Holds the attributes for SubTableVariable. (This entity is language independent.)
    /// 
    /// The table links variables with value sets in the sub-tables. The variable name is made up of the name for the corresponding metadata column in the data tables.
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
        /// Name of sub-table
        /// 
        /// Data can be missing, with a dash in its place.
        /// 
        /// See further description in the table SubTable.
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
        /// Name of value set. See further description in the table ValueSet.
        /// For rows with variable types V and G, the name of the value set must be filled in. For VariableType = T, the field is left empty, as there is no value set for the variable Time.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mVariableType;
        /// <summary>
        /// Code for type of variable. There are three alternatives:
        /// 
        /// - V = variable, i.e. dividing variable, not time.
        /// - G = geographical information for map program.
        /// - T = time.
        /// 
        /// If VariableType = G, the field GeoAreaNo in the tables ValueSet and Grouping should be filled in (however not yet implemented).
        /// </summary>
        public String VariableType
        {
            get { return mVariableType; }
        }
        private String mStoreColumnNo;
        /// <summary>
        /// The variable's column number in the data table.
        /// The variable Time should always be included and be the last column in the data table. If the material is divided by region, the variable Region should be the first column.
        /// Written as 1, 2, 3, etc.
        /// </summary>
        public String StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }
        private String mSortCode;
        /// <summary>
        /// The <i>Sortcode</i> field makes it possible to control the sortorder of the valuesets that are displayed in the dropdown for a variable on the selection page of PX-Web
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }
        private String mDefaultInGui;
        /// <summary>
        /// The <i>Default</i> field will define if a valueset or grouping shall be selected by default for a variable of the selection page of PX-Web. Values can be: Y/N
        /// </summary>
        public String DefaultInGui
        {
            get { return mDefaultInGui; }
        }

        public SubTableVariableRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mMainTable = myRow[dbconf.SubTableVariable.MainTableCol.Label()].ToString();
            this.mSubTable = myRow[dbconf.SubTableVariable.SubTableCol.Label()].ToString();
            this.mVariable = myRow[dbconf.SubTableVariable.VariableCol.Label()].ToString();
            this.mValueSet = myRow[dbconf.SubTableVariable.ValueSetCol.Label()].ToString();
            this.mVariableType = myRow[dbconf.SubTableVariable.VariableTypeCol.Label()].ToString();
            this.mStoreColumnNo = myRow[dbconf.SubTableVariable.StoreColumnNoCol.Label()].ToString();
            this.mSortCode = myRow[dbconf.SubTableVariable.SortCodeCol.Label()].ToString();
            this.mDefaultInGui = myRow[dbconf.SubTableVariable.DefaultInGuiCol.Label()].ToString();
        }
    }
}
