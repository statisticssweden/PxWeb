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
        //returns the single "row" found when all PKs are spesified
        public FootnoteRow GetFootnoteRow(string aFootnoteNo)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetFootnote_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Footnote.FootnoteNoCol.Is(mSqlCommand.GetParameterRef("aFootnoteNo"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aFootnoteNo", aFootnoteNo);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," FootnoteNo = " + aFootnoteNo);
            }

            FootnoteRow myOut = new FootnoteRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }

        public Dictionary<string, FootnoteRow> GetFootnoteRows(StringCollection aFootnoteNo, bool emptyRowSetIsOK)
        {
            Dictionary<string, FootnoteRow> myOut = new Dictionary<string, FootnoteRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetFootnote_SQLString_NoWhere();
            //
            // WHERE FNT.FootnoteNo = <"FootnoteNo as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.Footnote.FootnoteNoCol.In(mSqlCommand.GetParameterRef("aFootnoteNo"), aFootnoteNo.Count);

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[aFootnoteNo.Count];
            for (int counter = 1; counter <= aFootnoteNo.Count; counter++)
            {
                        parameters[counter - 1] = mSqlCommand.GetStringParameter("aFootnoteNo" + counter, aFootnoteNo[counter - 1]);
            }



            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35, " query, see log. ");
            }

            foreach (DataRow sqlRow in myRows)
            {
                FootnoteRow outRow = new FootnoteRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.FootnoteNo, outRow);
            }
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
                DB.Footnote.FootnoteTextCol.ForSelect() + ", " +
                DB.Footnote.PresCharacterCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.FootnoteLang2.FootnoteTextCol.ForSelectWithFallback(langCode, DB.Footnote.FootnoteTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetFootnoteRows_01 ***" + "/ ";
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
