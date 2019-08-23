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
    /// Holds the attributes for MainTableVariableHierarchy. (This entity is language independent.)
    /// 
    /// The table links a grouping to a main table.
    /// </summary>
    public class MainTableVariableHierarchyRow
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
        private String mVariable;
        /// <summary>
        /// Name of variable.
        /// 
        /// See further in the description of the table Variable.
        /// </summary>
        public String Variable
        {
            get { return mVariable; }
        }
        private String mGrouping;
        /// <summary>
        /// Name of grouping.
        /// 
        /// See further in the description of the table Grouping.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mShowLevels;
        /// <summary>
        /// The number of open levels that will be shown at menu selection the first time the tree is displayed. Must be 0 if all levels shall be shown.
        /// </summary>
        public String ShowLevels
        {
            get { return mShowLevels; }
        }
        private String mAllLevelsStored;
        /// <summary>
        /// Shows if all levels shall be stored or not. Can be:
        /// 
        /// Y = Yes
        /// N = No
        /// </summary>
        public String AllLevelsStored
        {
            get { return mAllLevelsStored; }
        }

        public MainTableVariableHierarchyRow(DataRow myRow, SqlDbConfig_23 dbconf)
        {
            this.mMainTable = myRow[dbconf.MainTableVariableHierarchy.MainTableCol.Label()].ToString();
            this.mVariable = myRow[dbconf.MainTableVariableHierarchy.VariableCol.Label()].ToString();
            this.mGrouping = myRow[dbconf.MainTableVariableHierarchy.GroupingCol.Label()].ToString();
            this.mShowLevels = myRow[dbconf.MainTableVariableHierarchy.ShowLevelsCol.Label()].ToString();
            this.mAllLevelsStored = myRow[dbconf.MainTableVariableHierarchy.AllLevelsStoredCol.Label()].ToString();
        }
    }
}
