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
        public FootnoteRow GetFootnoteRow(string aFootnoteNo)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetFootnote_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Footnote.FootnoteNoCol.Is(aFootnoteNo) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," FootnoteNo = " + aFootnoteNo);
            }

            FootnoteRow myOut = new FootnoteRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetFootnote_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Footnote.FootnoteNoCol.ForSelect() + ", " +
                DB.Footnote.FootnoteTypeCol.ForSelect() + ", " +
                DB.Footnote.ShowFootnoteCol.ForSelect() + ", " +
                DB.Footnote.MandOptCol.ForSelect() + ", " +
                DB.Footnote.FootnoteTextCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.FootnoteLang2.FootnoteTextCol.ForSelectWithFallback(langCode, DB.Footnote.FootnoteTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetFootnoteRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Footnote.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.FootnoteLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Footnote.FootnoteNoCol.Is(DB.FootnoteLang2.FootnoteNoCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
