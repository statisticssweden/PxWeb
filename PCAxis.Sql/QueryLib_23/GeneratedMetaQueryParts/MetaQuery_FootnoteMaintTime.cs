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
        public Dictionary<string, FootnoteMaintTimeRow> GetFootnoteMaintTimeRows(string aMainTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, FootnoteMaintTimeRow> myOut = new Dictionary<string, FootnoteMaintTimeRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnoteMaintTime_SQLString_NoWhere();
            //
            // WHERE FNM.MainTable = <"MainTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.FootnoteMaintTime.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable);
            }

            foreach (DataRow sqlRow in myRows)
            {
                FootnoteMaintTimeRow outRow = new FootnoteMaintTimeRow(sqlRow, DB);
                myOut.Add(outRow.FootnoteNo, outRow);
            }
            return myOut;
        }

        private String GetFootnoteMaintTime_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.FootnoteMaintTime.MainTableCol.ForSelect() + ", " +
                DB.FootnoteMaintTime.TimePeriodCol.ForSelect() + ", " +
                DB.FootnoteMaintTime.FootnoteNoCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetFootnoteMaintTimeRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.FootnoteMaintTime.GetNameAndAlias();
            return sqlString;
        }
    }
}
