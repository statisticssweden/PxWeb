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
        /*This table is not suitable for generated extractions such as Get...*/

        private String GetTextCatalog_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.TextCatalog.TextCatalogNoCol.ForSelect() + ", " +
                DB.TextCatalog.TextTypeCol.ForSelect() + ", " +
                DB.TextCatalog.PresTextCol.ForSelect() + ", " +
                DB.TextCatalog.DescriptionCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.TextCatalogLang2.TextTypeCol.ForSelectWithFallback(langCode, DB.TextCatalog.TextTypeCol);
                    sqlString += ", " + DB.TextCatalogLang2.PresTextCol.ForSelectWithFallback(langCode, DB.TextCatalog.PresTextCol);
                    sqlString += ", " + DB.TextCatalogLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.TextCatalog.DescriptionCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetSubTableVariableRowskeyVariable_01 ***" + "/ ";
            sqlString += " FROM " + DB.TextCatalog.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.TextCatalogLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.TextCatalog.TextCatalogNoCol.Is(DB.TextCatalogLang2.TextCatalogNoCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
