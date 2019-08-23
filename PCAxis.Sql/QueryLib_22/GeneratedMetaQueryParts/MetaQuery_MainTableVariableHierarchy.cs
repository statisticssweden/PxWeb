using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;


//This code is generated. 

namespace PCAxis.Sql.QueryLib_22
{
    public partial class MetaQuery
    {
        //returns the single "row" found when all PKs are spesified
        public MainTableVariableHierarchyRow GetMainTableVariableHierarchyRow(string aMainTable, string aVariable, string aGrouping)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMainTableVariableHierarchy_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MainTableVariableHierarchy.MainTableCol.Is(aMainTable)  + 
                             " AND " +DB.MainTableVariableHierarchy.VariableCol.Is(aVariable)  + 
                             " AND " +DB.MainTableVariableHierarchy.GroupingCol.Is(aGrouping) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," MainTable = " + aMainTable + " Variable = " + aVariable + " Grouping = " + aGrouping);
            }

            MainTableVariableHierarchyRow myOut = new MainTableVariableHierarchyRow(myRows[0], DB); 
            return myOut;
        }

        public Dictionary<string, MainTableVariableHierarchyRow> GetMainTableVariableHierarchyRows_KeyIsGroupingID(string aMainTable, string aVariable, bool emptyRowSetIsOK)
        {
            Dictionary<string, MainTableVariableHierarchyRow> myOut = new Dictionary<string, MainTableVariableHierarchyRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetMainTableVariableHierarchy_SQLString_NoWhere();
            //
            // WHERE MTP.MainTable = '<aMainTable>'
            //    AND MTP.Variable = '<aVariable>'
            //
            sqlString += " WHERE " + DB.MainTableVariableHierarchy.MainTableCol.Is(aMainTable) + 
                         " AND " +DB.MainTableVariableHierarchy.VariableCol.Is(aVariable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable +  " Variable = " + aVariable);
            }

            foreach (DataRow sqlRow in myRows)
            {
                MainTableVariableHierarchyRow outRow = new MainTableVariableHierarchyRow(sqlRow, DB);
                myOut.Add(outRow.Grouping, outRow);
            }
            return myOut;
        }

        private String GetMainTableVariableHierarchy_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MainTableVariableHierarchy.MainTableCol.ForSelect() + ", " +
                DB.MainTableVariableHierarchy.VariableCol.ForSelect() + ", " +
                DB.MainTableVariableHierarchy.GroupingCol.ForSelect() + ", " +
                DB.MainTableVariableHierarchy.ShowLevelsCol.ForSelect() + ", " +
                DB.MainTableVariableHierarchy.AllLevelsStoredCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMainTableVariableHierarchyRows_KeyIsGroupingID_01 ***" + "/ ";
            sqlString += " FROM " + DB.MainTableVariableHierarchy.GetNameAndAlias();
            return sqlString;
        }
    }
}
