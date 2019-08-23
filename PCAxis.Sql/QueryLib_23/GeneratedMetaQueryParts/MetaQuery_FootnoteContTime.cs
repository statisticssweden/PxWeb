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
        public Dictionary<string, List<FootnoteContTimeRow>> GetFootnoteContTimeRows(string aMainTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, List<FootnoteContTimeRow>> myOut = new Dictionary<string, List<FootnoteContTimeRow>>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnoteContTime_SQLString_NoWhere();
            //
            // WHERE FCT.MainTable = <"MainTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.FootnoteContTime.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"));

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
                FootnoteContTimeRow outRow = new FootnoteContTimeRow(sqlRow, DB);
                if (!myOut.ContainsKey(outRow.FootnoteNo))
                {
                    myOut[outRow.FootnoteNo] = new List<FootnoteContTimeRow>();
                }
                myOut[outRow.FootnoteNo].Add(outRow);
            }
            return myOut;
        }

        private String GetFootnoteContTime_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.FootnoteContTime.MainTableCol.ForSelect() + ", " +
                DB.FootnoteContTime.ContentsCol.ForSelect() + ", " +
                DB.FootnoteContTime.TimePeriodCol.ForSelect() + ", " +
                DB.FootnoteContTime.FootnoteNoCol.ForSelect() + ", " +
                DB.FootnoteContTime.CellnoteCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetFootnoteContTimeRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.FootnoteContTime.GetNameAndAlias();
            return sqlString;
        }
    }
}
