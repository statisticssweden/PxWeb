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
        public Dictionary<string, FootnoteValueSetValueRow> GetFootnoteValueSetValueRows(StringCollection aValueSet, bool emptyRowSetIsOK)
        {
            Dictionary<string, FootnoteValueSetValueRow> myOut = new Dictionary<string, FootnoteValueSetValueRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnoteValueSetValue_SQLString_NoWhere();
            //
            // WHERE FVS.ValueSet = <"ValueSet as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.FootnoteValueSetValue.ValueSetCol.In(mSqlCommand.GetParameterRef("aValueSet"), aValueSet.Count);

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[aValueSet.Count];
            for (int counter = 1; counter <= aValueSet.Count; counter++)
            {
                        parameters[counter - 1] = mSqlCommand.GetStringParameter("aValueSet" + counter, aValueSet[counter - 1]);
            }



            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35, " query, see log. ");
            }

            foreach (DataRow sqlRow in myRows)
            {
                FootnoteValueSetValueRow outRow = new FootnoteValueSetValueRow(sqlRow, DB);
                myOut.Add(outRow.FootnoteNo, outRow);
            }
            return myOut;
        }

        private String GetFootnoteValueSetValue_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.FootnoteValueSetValue.ValuePoolCol.ForSelect() + ", " +
                DB.FootnoteValueSetValue.ValueSetCol.ForSelect() + ", " +
                DB.FootnoteValueSetValue.ValueCodeCol.ForSelect() + ", " +
                DB.FootnoteValueSetValue.FootnoteNoCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetFootnoteValueSetValueRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.FootnoteValueSetValue.GetNameAndAlias();
            return sqlString;
        }
    }
}
