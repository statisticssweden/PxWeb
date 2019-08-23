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

namespace PCAxis.Sql.QueryLib_24
{
    public partial class MetaQuery
    {
        //returns the single "row" found when all PKs are spesified
        public MainTableVariableHierarchyRow GetMainTableVariableHierarchyRow(string aMainTable, string aVariable, string aGrouping)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMainTableVariableHierarchy_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MainTableVariableHierarchy.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) + 
                             " AND " +DB.MainTableVariableHierarchy.VariableCol.Is(mSqlCommand.GetParameterRef("aVariable")) + 
                             " AND " +DB.MainTableVariableHierarchy.GroupingCol.Is(mSqlCommand.GetParameterRef("aGrouping"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[3];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);
            parameters[1] = mSqlCommand.GetStringParameter("aVariable", aVariable);
            parameters[2] = mSqlCommand.GetStringParameter("aGrouping", aGrouping);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
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
            // WHERE MTP.MainTable = <"MainTable as parameter reference for your db vendor">
            //    AND MTP.Variable = <"Variable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.MainTableVariableHierarchy.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) + 
                         " AND " +DB.MainTableVariableHierarchy.VariableCol.Is(mSqlCommand.GetParameterRef("aVariable"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);
            parameters[1] = mSqlCommand.GetStringParameter("aVariable", aVariable);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
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
