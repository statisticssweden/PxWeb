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
        public SubTableVariableRow GetSubTableVariableRow(string aMainTable, string aSubTable, string aVariable)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetSubTableVariable_SQLString_NoWhere();
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) + 
                             " AND " +DB.SubTableVariable.SubTableCol.Is(mSqlCommand.GetParameterRef("aSubTable")) + 
                             " AND " +DB.SubTableVariable.VariableCol.Is(mSqlCommand.GetParameterRef("aVariable"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[3];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);
            parameters[1] = mSqlCommand.GetStringParameter("aSubTable", aSubTable);
            parameters[2] = mSqlCommand.GetStringParameter("aVariable", aVariable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," MainTable = " + aMainTable + " SubTable = " + aSubTable + " Variable = " + aVariable);
            }

            SubTableVariableRow myOut = new SubTableVariableRow(myRows[0], DB); 
            return myOut;
        }

        public Dictionary<string, SubTableVariableRow> GetSubTableVariableRowskeyVariable(string aMainTable, string aSubTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, SubTableVariableRow> myOut = new Dictionary<string, SubTableVariableRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetSubTableVariable_SQLString_NoWhere();
            //
            // WHERE STV.MainTable = <"MainTable as parameter reference for your db vendor">
            //    AND STV.SubTable = <"SubTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) + 
                         " AND " +DB.SubTableVariable.SubTableCol.Is(mSqlCommand.GetParameterRef("aSubTable"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);
            parameters[1] = mSqlCommand.GetStringParameter("aSubTable", aSubTable);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable +  " SubTable = " + aSubTable);
            }

            foreach (DataRow sqlRow in myRows)
            {
                SubTableVariableRow outRow = new SubTableVariableRow(sqlRow, DB);
                myOut.Add(outRow.Variable, outRow);
            }
            return myOut;
        }

        private String GetSubTableVariable_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.SubTableVariable.MainTableCol.ForSelect() + ", " +
                DB.SubTableVariable.SubTableCol.ForSelect() + ", " +
                DB.SubTableVariable.VariableCol.ForSelect() + ", " +
                DB.SubTableVariable.ValueSetCol.ForSelect() + ", " +
                DB.SubTableVariable.VariableTypeCol.ForSelect() + ", " +
                DB.SubTableVariable.StoreColumnNoCol.ForSelect() + ", " +
                DB.SubTableVariable.SortCodeCol.ForSelect() + ", " +
                DB.SubTableVariable.DefaultInGuiCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetSubTableVariableRowskeyVariable_01 ***" + "/ ";
            sqlString += " FROM " + DB.SubTableVariable.GetNameAndAlias();
            return sqlString;
        }
    }
}
