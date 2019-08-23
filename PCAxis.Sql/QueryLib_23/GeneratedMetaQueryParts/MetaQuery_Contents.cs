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
        public Dictionary<string, ContentsRow> GetContentsRows(string aMainTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, ContentsRow> myOut = new Dictionary<string, ContentsRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetContents_SQLString_NoWhere();
            //
            // WHERE CNT.MainTable = <"MainTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.Contents.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"));

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
                ContentsRow outRow = new ContentsRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.Contents, outRow);
            }
            return myOut;
        }

        private String GetContents_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Contents.MainTableCol.ForSelect() + ", " +
                DB.Contents.ContentsCol.ForSelect() + ", " +
                DB.Contents.PresTextCol.ForSelect() + ", " +
                DB.Contents.PresTextSCol.ForSelect() + ", " +
                DB.Contents.PresCodeCol.ForSelect() + ", " +
                DB.Contents.CopyrightCol.ForSelect() + ", " +
                DB.Contents.StatAuthorityCol.ForSelect() + ", " +
                DB.Contents.ProducerCol.ForSelect() + ", " +
                DB.Contents.LastUpdatedCol.ForSelect() + ", " +
                DB.Contents.PublishedCol.ForSelect() + ", " +
                DB.Contents.UnitCol.ForSelect() + ", " +
                DB.Contents.PresDecimalsCol.ForSelect() + ", " +
                DB.Contents.PresCellsZeroCol.ForSelect() + ", " +
                DB.Contents.PresMissingLineCol.ForSelect() + ", " +
                DB.Contents.AggregPossibleCol.ForSelect() + ", " +
                DB.Contents.RefPeriodCol.ForSelect() + ", " +
                DB.Contents.StockFACol.ForSelect() + ", " +
                DB.Contents.BasePeriodCol.ForSelect() + ", " +
                DB.Contents.CFPricesCol.ForSelect() + ", " +
                DB.Contents.DayAdjCol.ForSelect() + ", " +
                DB.Contents.SeasAdjCol.ForSelect() + ", " +
                DB.Contents.StoreColumnNoCol.ForSelect() + ", " +
                DB.Contents.StoreFormatCol.ForSelect() + ", " +
                DB.Contents.StoreNoCharCol.ForSelect() + ", " +
                DB.Contents.StoreDecimalsCol.ForSelect() + ", " +
                DB.Contents.MetaIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.ContentsLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Contents.PresTextCol);
                    sqlString += ", " + DB.ContentsLang2.PresTextSCol.ForSelectWithFallback(langCode, DB.Contents.PresTextSCol);
                    sqlString += ", " + DB.ContentsLang2.UnitCol.ForSelectWithFallback(langCode, DB.Contents.UnitCol);
                    sqlString += ", " + DB.ContentsLang2.RefPeriodCol.ForSelectWithFallback(langCode, DB.Contents.RefPeriodCol);
                    sqlString += ", " + DB.ContentsLang2.BasePeriodCol.ForSelectWithFallback(langCode, DB.Contents.BasePeriodCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetContentsRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.Contents.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.ContentsLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Contents.MainTableCol.Is(DB.ContentsLang2.MainTableCol, langCode) +
                                 " AND " + DB.Contents.ContentsCol.Is(DB.ContentsLang2.ContentsCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
