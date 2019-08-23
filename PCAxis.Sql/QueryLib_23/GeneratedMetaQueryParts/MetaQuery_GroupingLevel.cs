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
        public Dictionary<string, GroupingLevelRow> GetGroupingLevelRows_KeyIsLevel(string aGrouping, bool emptyRowSetIsOK)
        {
            Dictionary<string, GroupingLevelRow> myOut = new Dictionary<string, GroupingLevelRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetGroupingLevel_SQLString_NoWhere();
            //
            // WHERE GRP.Grouping = <"Grouping as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.GroupingLevel.GroupingCol.Is(mSqlCommand.GetParameterRef("aGrouping"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aGrouping", aGrouping);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " Grouping = " + aGrouping);
            }

            foreach (DataRow sqlRow in myRows)
            {
                GroupingLevelRow outRow = new GroupingLevelRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.LevelNo, outRow);
            }
            return myOut;
        }

        private String GetGroupingLevel_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.GroupingLevel.GroupingCol.ForSelect() + ", " +
                DB.GroupingLevel.LevelNoCol.ForSelect() + ", " +
                DB.GroupingLevel.LevelTextCol.ForSelect() + ", " +
                DB.GroupingLevel.GeoAreaNoCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.GroupingLevelLang2.LevelTextCol.ForSelectWithFallback(langCode, DB.GroupingLevel.LevelTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetGroupingLevelRows_KeyIsLevel_01 ***" + "/ ";
            sqlString += " FROM " + DB.GroupingLevel.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.GroupingLevelLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.GroupingLevel.GroupingCol.Is(DB.GroupingLevelLang2.GroupingCol, langCode) +
                                 " AND " + DB.GroupingLevel.LevelNoCol.Is(DB.GroupingLevelLang2.LevelNoCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
