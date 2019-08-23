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
        public Dictionary<string, ValueSetGroupingRow> GetValueSetGroupingRowskeyGrouping(string aValueSet, bool emptyRowSetIsOK)
        {
            Dictionary<string, ValueSetGroupingRow> myOut = new Dictionary<string, ValueSetGroupingRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetValueSetGrouping_SQLString_NoWhere();
            //
            // WHERE VBL.ValueSet = '<aValueSet>'
            //
            sqlString += " WHERE " + DB.ValueSetGrouping.ValueSetCol.Is(aValueSet);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " ValueSet = " + aValueSet);
            }

            foreach (DataRow sqlRow in myRows)
            {
                ValueSetGroupingRow outRow = new ValueSetGroupingRow(sqlRow, DB);
                myOut.Add(outRow.Grouping, outRow);
            }
            return myOut;
        }

        private String GetValueSetGrouping_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.ValueSetGrouping.ValueSetCol.ForSelect() + ", " +
                DB.ValueSetGrouping.GroupingCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetValueSetGroupingRowskeyGrouping_01 ***" + "/ ";
            sqlString += " FROM " + DB.ValueSetGrouping.GetNameAndAlias();
            return sqlString;
        }
    }
}
