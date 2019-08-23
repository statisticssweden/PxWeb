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
        public Dictionary<string, ValueRow> GetValueRows(string aValuePool, bool emptyRowSetIsOK)
        {
            Dictionary<string, ValueRow> myOut = new Dictionary<string, ValueRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetValue_SQLString_NoWhere();
            //
            // WHERE VAL.ValuePool = <"ValuePool as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.Value.ValuePoolCol.Is(mSqlCommand.GetParameterRef("aValuePool")) + 
                         " AND " +DB.Value.MetaIdCol.IsNotNULL();

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aValuePool", aValuePool);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " ValuePool = " + aValuePool);
            }

            foreach (DataRow sqlRow in myRows)
            {
                ValueRow outRow = new ValueRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.ValueCode, outRow);
            }
            return myOut;
        }

        private String GetValue_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Value.ValuePoolCol.ForSelect() + ", " +
                DB.Value.ValueCodeCol.ForSelect() + ", " +
                DB.Value.SortCodeCol.ForSelect() + ", " +
                DB.Value.UnitCol.ForSelect() + ", " +
                DB.Value.ValueTextSCol.ForSelect() + ", " +
                DB.Value.ValueTextLCol.ForSelect() + ", " +
                DB.Value.MetaIdCol.ForSelect() + ", " +
                DB.Value.FootnoteCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.ValueLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.Value.SortCodeCol);
                    sqlString += ", " + DB.ValueLang2.UnitCol.ForSelectWithFallback(langCode, DB.Value.UnitCol);
                    sqlString += ", " + DB.ValueLang2.ValueTextSCol.ForSelectWithFallback(langCode, DB.Value.ValueTextSCol);
                    sqlString += ", " + DB.ValueLang2.ValueTextLCol.ForSelectWithFallback(langCode, DB.Value.ValueTextLCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetValueRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.Value.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.ValueLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Value.ValuePoolCol.Is(DB.ValueLang2.ValuePoolCol, langCode) +
                                 " AND " + DB.Value.ValueCodeCol.Is(DB.ValueLang2.ValueCodeCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
