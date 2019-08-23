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
        public Dictionary<string, FootnoteGroupingRow> GetFootnoteGroupingRows(StringCollection aGrouping, bool emptyRowSetIsOK)
        {
            Dictionary<string, FootnoteGroupingRow> myOut = new Dictionary<string, FootnoteGroupingRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnoteGrouping_SQLString_NoWhere();
            //
            // WHERE FCO.Grouping = <"Grouping as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.FootnoteGrouping.GroupingCol.In(mSqlCommand.GetParameterRef("aGrouping"), aGrouping.Count);

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[aGrouping.Count];
            for (int counter = 1; counter <= aGrouping.Count; counter++)
            {
                        parameters[counter - 1] = mSqlCommand.GetStringParameter("aGrouping" + counter, aGrouping[counter - 1]);
            }



            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
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
