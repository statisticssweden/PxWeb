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
        public Dictionary<string, List<FootnoteContValueRow>> GetFootnoteContValueRows(string aMainTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, List<FootnoteContValueRow>> myOut = new Dictionary<string, List<FootnoteContValueRow>>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnoteContValue_SQLString_NoWhere();
            //
            // WHERE FCA.MainTable = <"MainTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.FootnoteContValue.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"));

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
                FootnoteContValueRow outRow = new FootnoteContValueRow(sqlRow, DB);
                if (!myOut.ContainsKey(outRow.FootnoteNo))
                {
                    myOut[outRow.FootnoteNo] = new List<FootnoteContValueRow>();
                }
                myOut[outRow.FootnoteNo].Add(outRow);
            }
            return myOut;
        }

        private String GetFootnoteContValue_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.FootnoteContValue.MainTableCol.ForSelect() + ", " +
                DB.FootnoteContValue.ContentsCol.ForSelect() + ", " +
                DB.FootnoteContValue.VariableCol.ForSelect() + ", " +
                DB.FootnoteContValue.ValuePoolCol.ForSelect() + ", " +
                DB.FootnoteContValue.ValueCodeCol.ForSelect() + ", " +
                DB.FootnoteContValue.FootnoteNoCol.ForSelect() + ", " +
                DB.FootnoteContValue.CellnoteCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetFootnoteContValueRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.FootnoteContValue.GetNameAndAlias();
            return sqlString;
        }
    }
}
