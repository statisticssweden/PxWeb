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
        public Dictionary<string, ValueGroupRow> GetValueGroupRowskeyValueCode(string aGrouping, string aGroupCode, bool emptyRowSetIsOK)
        {
            Dictionary<string, ValueGroupRow> myOut = new Dictionary<string, ValueGroupRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetValueGroup_SQLString_NoWhere();
            //
            // WHERE VPL.Grouping = <"Grouping as parameter reference for your db vendor">
            //    AND VPL.GroupCode = <"GroupCode as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.ValueGroup.GroupingCol.Is(mSqlCommand.GetParameterRef("aGrouping")) + 
                         " AND " +DB.ValueGroup.GroupCodeCol.Is(mSqlCommand.GetParameterRef("aGroupCode"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = mSqlCommand.GetStringParameter("aGrouping", aGrouping);
            parameters[1] = mSqlCommand.GetStringParameter("aGroupCode", aGroupCode);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " Grouping = " + aGrouping +  " GroupCode = " + aGroupCode);
            }

            foreach (DataRow sqlRow in myRows)
            {
                ValueGroupRow outRow = new ValueGroupRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.ValueCode, outRow);
            }
            return myOut;
        }

        private String GetValueGroup_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.ValueGroup.GroupingCol.ForSelect() + ", " +
                DB.ValueGroup.GroupCodeCol.ForSelect() + ", " +
                DB.ValueGroup.ValueCodeCol.ForSelect() + ", " +
                DB.ValueGroup.ValuePoolCol.ForSelect() + ", " +
                DB.ValueGroup.GroupLevelCol.ForSelect() + ", " +
                DB.ValueGroup.ValueLevelCol.ForSelect() + ", " +
                DB.ValueGroup.SortCodeCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.ValueGroupLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.ValueGroup.SortCodeCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetValueGroupRowskeyValueCode_01 ***" + "/ ";
            sqlString += " FROM " + DB.ValueGroup.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.ValueGroupLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.ValueGroup.GroupingCol.Is(DB.ValueGroupLang2.GroupingCol, langCode) +
                                 " AND " + DB.ValueGroup.GroupCodeCol.Is(DB.ValueGroupLang2.GroupCodeCol, langCode) +
                                 " AND " + DB.ValueGroup.ValueCodeCol.Is(DB.ValueGroupLang2.ValueCodeCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
