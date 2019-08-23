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
        public GroupingRow GetGroupingRow(string aGrouping)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetGrouping_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Grouping.GroupingCol.Is(aGrouping) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," Grouping = " + aGrouping);
            }

            GroupingRow myOut = new GroupingRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetGrouping_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Grouping.GroupingCol.ForSelect() + ", " +
                DB.Grouping.ValuePoolCol.ForSelect() + ", " +
                DB.Grouping.PresTextCol.ForSelect() + ", " +
                DB.Grouping.HierarchyCol.ForSelect() + ", " +
                DB.Grouping.SortCodeCol.ForSelect() + ", " +
                DB.Grouping.GroupPresCol.ForSelect() + ", " +
                DB.Grouping.DescriptionCol.ForSelect() + ", " +
                DB.Grouping.KDBidCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.GroupingLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Grouping.PresTextCol);
                    sqlString += ", " + DB.GroupingLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.Grouping.SortCodeCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetGroupingRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Grouping.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.GroupingLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Grouping.GroupingCol.Is(DB.GroupingLang2.GroupingCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
