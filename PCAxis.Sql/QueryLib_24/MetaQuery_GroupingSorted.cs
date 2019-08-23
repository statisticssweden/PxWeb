using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using PCAxis.Sql.DbConfig;

namespace PCAxis.Sql.QueryLib_24
{

    public partial class MetaQuery
    {
        /// <summary>
        /// Returns rows for the given grouping ids sorted by sortorder, prestext from given language.
        /// </summary>
        /// <param name="aGroupings">if this is empty, an empty list is returned </param>
        /// <param name="emptyRowSetIsOK"></param>
        /// <param name="aSortOrderLanguage"></param>
        /// <returns></returns>
        public List<GroupingRow> GetGroupingRowsSorted(StringCollection aGroupings, bool emptyRowSetIsOK, string aSortOrderLanguage)
        {
            string sortOrderLanguage = aSortOrderLanguage;
            if (!this.mLanguageCodes.Contains(aSortOrderLanguage))
            {
                log.Error("requsted sortOrder language not in extaction, using first in list of languages");
                sortOrderLanguage = this.mLanguageCodes[0];
            }
            bool sortOnPrimaryLanguage = !this.mDbConfig.isSecondaryLanguage(sortOrderLanguage);
            List<GroupingRow> myOut = new List<GroupingRow>();
            if(aGroupings.Count == 0)
            {
                return myOut;
            }


            SqlDbConfig dbconf = DB;

            

            string sqlString = GetGrouping_SQLString_NoWhere();

            sqlString += " WHERE " + DB.Grouping.GroupingCol.In(mSqlCommand.GetParameterRef("aGroupings"), aGroupings.Count);

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[aGroupings.Count];
            for (int counter = 1; counter <= aGroupings.Count; counter++)
            {
                parameters[counter - 1] = mSqlCommand.GetStringParameter("aGroupings" + counter, aGroupings[counter - 1]);
            }


            if (sortOnPrimaryLanguage)
            {
                sqlString += " ORDER BY " + DB.Grouping.SortCodeCol.Id() + ", " + DB.Grouping.PresTextCol.Id() + ", ";
            }
            else
            {
                sqlString += " ORDER BY " + DB.GroupingLang2.SortCodeCol.Id(sortOrderLanguage) + ", "+
                                            DB.GroupingLang2.PresTextCol.Id(sortOrderLanguage) + ", ";
            }
            sqlString += DB.Grouping.GroupingCol.Id();


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);

            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && !emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35, " Grouping = " + aGroupings.ToString());
            }

            foreach (DataRow sqlRow in myRows)
            {
                GroupingRow outRow = new GroupingRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow);
            }
            return myOut;
        }

    }
}
