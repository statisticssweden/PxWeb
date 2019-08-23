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
        public Dictionary<string, SecondaryLanguageRow> GetSecondaryLanguageRowsbyLanguage(string aMainTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, SecondaryLanguageRow> myOut = new Dictionary<string, SecondaryLanguageRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetSecondaryLanguage_SQLString_NoWhere();
            //
            // WHERE SLA.MainTable = <"MainTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.SecondaryLanguage.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"));

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
                SecondaryLanguageRow outRow = new SecondaryLanguageRow(sqlRow, DB);
                myOut.Add(outRow.Language, outRow);
            }
            return myOut;
        }

        private String GetSecondaryLanguage_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.SecondaryLanguage.MainTableCol.ForSelect() + ", " +
                DB.SecondaryLanguage.LanguageCol.ForSelect() + ", " +
                DB.SecondaryLanguage.CompletelyTranslatedCol.ForSelect() + ", " +
                DB.SecondaryLanguage.PublishedCol.ForSelect() + ", " +
                DB.SecondaryLanguage.UserIdCol.ForSelect() + ", " +
                DB.SecondaryLanguage.LogDateCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetSecondaryLanguageRowsbyLanguage_01 ***" + "/ ";
            sqlString += " FROM " + DB.SecondaryLanguage.GetNameAndAlias();
            return sqlString;
        }
    }
}
