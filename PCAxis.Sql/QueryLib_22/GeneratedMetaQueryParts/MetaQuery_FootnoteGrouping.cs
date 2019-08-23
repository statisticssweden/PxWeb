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
        public Dictionary<string, FootnoteGroupingRow> GetFootnoteGroupingRows(StringCollection aGrouping, bool emptyRowSetIsOK)
        {
            Dictionary<string, FootnoteGroupingRow> myOut = new Dictionary<string, FootnoteGroupingRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnoteGrouping_SQLString_NoWhere();
            //
            // WHERE FCO.Grouping = '<aGrouping>'
            //
            sqlString += " WHERE " + DB.FootnoteGrouping.GroupingCol.In(aGrouping);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35, " query, see log. ");
            }

            foreach (DataRow sqlRow in myRows)
            {
                FootnoteGroupingRow outRow = new FootnoteGroupingRow(sqlRow, DB);
                myOut.Add(outRow.FootnoteNo, outRow);
            }
            return myOut;
        }

        private String GetFootnoteGrouping_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.FootnoteGrouping.GroupingCol.ForSelect() + ", " +
                DB.FootnoteGrouping.FootnoteNoCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetFootnoteGroupingRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.FootnoteGrouping.GetNameAndAlias();
            return sqlString;
        }
    }
}
