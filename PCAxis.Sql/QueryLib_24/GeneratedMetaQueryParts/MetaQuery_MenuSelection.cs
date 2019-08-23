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
        /* For SubjectArea*/
        //returns the single "row" found when all PKs are spesified
        public MenuSelectionRow GetMenuSelectionRow(string aMenu, string aSelection)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMenuSelection_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MenuSelection.MenuCol.Is(mSqlCommand.GetParameterRef("aMenu")) + 
                             " AND " +DB.MenuSelection.SelectionCol.Is(mSqlCommand.GetParameterRef("aSelection"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = mSqlCommand.GetStringParameter("aMenu", aMenu);
            parameters[1] = mSqlCommand.GetStringParameter("aSelection", aSelection);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," Menu = " + aMenu + " Selection = " + aSelection);
            }

            MenuSelectionRow myOut = new MenuSelectionRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetMenuSelection_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MenuSelection.MenuCol.ForSelect() + ", " +
                DB.MenuSelection.SelectionCol.ForSelect() + ", " +
                DB.MenuSelection.PresTextCol.ForSelect() + ", " +
                DB.MenuSelection.PresTextSCol.ForSelect() + ", " +
                DB.MenuSelection.DescriptionCol.ForSelect() + ", " +
                DB.MenuSelection.LevelNoCol.ForSelect() + ", " +
                DB.MenuSelection.SortCodeCol.ForSelect() + ", " +
                DB.MenuSelection.PresentationCol.ForSelect() + ", " +
                DB.MenuSelection.MetaIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.MenuSelectionLang2.PresTextCol.ForSelectWithFallback(langCode, DB.MenuSelection.PresTextCol);
                    sqlString += ", " + DB.MenuSelectionLang2.PresTextSCol.ForSelectWithFallback(langCode, DB.MenuSelection.PresTextSCol);
                    sqlString += ", " + DB.MenuSelectionLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.MenuSelection.DescriptionCol);
                    sqlString += ", " + DB.MenuSelectionLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.MenuSelection.SortCodeCol);
                    sqlString += ", " + DB.MenuSelectionLang2.PresentationCol.ForSelectWithFallback(langCode, DB.MenuSelection.PresentationCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetMenuSelectionRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.MenuSelection.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.MenuSelectionLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.MenuSelection.MenuCol.Is(DB.MenuSelectionLang2.MenuCol, langCode) +
                                 " AND " + DB.MenuSelection.SelectionCol.Is(DB.MenuSelectionLang2.SelectionCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
