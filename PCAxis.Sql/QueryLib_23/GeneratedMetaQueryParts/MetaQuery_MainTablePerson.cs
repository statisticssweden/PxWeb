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

namespace PCAxis.Sql.QueryLib_23
{
    public partial class MetaQuery
    {
        public Dictionary<string, MainTablePersonRow> GetMainTablePersonRows(string aMainTable, string aRolePerson, bool emptyRowSetIsOK)
        {
            Dictionary<string, MainTablePersonRow> myOut = new Dictionary<string, MainTablePersonRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetMainTablePerson_SQLString_NoWhere();
            //
            // WHERE MTP.MainTable = <"MainTable as parameter reference for your db vendor">
            //    AND MTP.RolePerson = <"RolePerson as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.MainTablePerson.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) + 
                         " AND " +DB.MainTablePerson.RolePersonCol.Is(mSqlCommand.GetParameterRef("aRolePerson"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);
            parameters[1] = mSqlCommand.GetStringParameter("aRolePerson", aRolePerson);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable +  " RolePerson = " + aRolePerson);
            }

            foreach (DataRow sqlRow in myRows)
            {
                MainTablePersonRow outRow = new MainTablePersonRow(sqlRow, DB);
                myOut.Add(outRow.PersonCode, outRow);
            }
            return myOut;
        }

        private String GetMainTablePerson_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MainTablePerson.MainTableCol.ForSelect() + ", " +
                DB.MainTablePerson.PersonCodeCol.ForSelect() + ", " +
                DB.MainTablePerson.RolePersonCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMainTablePersonRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.MainTablePerson.GetNameAndAlias();
            return sqlString;
        }
    }
}
