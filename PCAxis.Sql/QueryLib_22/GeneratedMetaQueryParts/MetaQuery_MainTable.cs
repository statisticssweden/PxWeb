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
        //returns the single "row" found when all PKs are spesified
        public MainTableRow GetMainTableRow(string aMainTable)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMainTable_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MainTable.MainTableCol.Is(aMainTable) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," MainTable = " + aMainTable);
            }

            MainTableRow myOut = new MainTableRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetMainTable_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MainTable.MainTableCol.ForSelect() + ", " +
                DB.MainTable.TableStatusCol.ForSelect() + ", " +
                DB.MainTable.PresTextCol.ForSelect() + ", " +
                DB.MainTable.PresTextSCol.ForSelect() + ", " +
                DB.MainTable.ContentsVariableCol.ForSelect() + ", " +
                DB.MainTable.TableIdCol.ForSelect() + ", " +
                DB.MainTable.PresCategoryCol.ForSelect() + ", " +
                DB.MainTable.SpecCharExistsCol.ForSelect() + ", " +
                DB.MainTable.SubjectCodeCol.ForSelect() + ", " +
                DB.MainTable.ProductIdCol.ForSelect() + ", " +
                DB.MainTable.TimeScaleCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.MainTableLang2.PresTextCol.ForSelectWithFallback(langCode, DB.MainTable.PresTextCol);
                    sqlString += ", " + DB.MainTableLang2.PresTextSCol.ForSelectWithFallback(langCode, DB.MainTable.PresTextSCol);
                    sqlString += ", " + DB.MainTableLang2.ContentsVariableCol.ForSelectWithFallback(langCode, DB.MainTable.ContentsVariableCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetMainTableRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.MainTable.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.MainTableLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.MainTable.MainTableCol.Is(DB.MainTableLang2.MainTableCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
