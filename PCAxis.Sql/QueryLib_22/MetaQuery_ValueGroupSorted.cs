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

namespace PCAxis.Sql.QueryLib_22 {

    public partial class MetaQuery {
        #region for ValueGroup
        public List<ValueGroupRow> GetValueGroupRowsSorted(string aGrouping,string aLevel, bool emptyRowSetIsOK, string aSortOrderLanguage)
        {
            string sortOrderLanguage = aSortOrderLanguage;
            if (!this.mLanguageCodes.Contains(aSortOrderLanguage))
            {
                log.Error("requsted sortOrder language not in extaction, using first in list of languages");
                sortOrderLanguage = this.mLanguageCodes[0];
            }
            bool sortOnPrimaryLanguage = !this.mDbConfig.isSecondaryLanguage(sortOrderLanguage);
            List<ValueGroupRow> myOut = new List<ValueGroupRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetValueGroup_SQLString_NoWhere();
            //
            // WHERE VPL.Grouping = '<aGrouping>'
            //    AND VPL.GroupCode = '<aGroupCode>'
            //
            sqlString += " WHERE " + DB.ValueGroup.GroupingCol.Is(aGrouping);
            if (aLevel != null)
            {
                sqlString += " AND " + DB.ValueGroup.GroupLevelCol.Is(aLevel);
            }
            if (sortOnPrimaryLanguage)
            {
                sqlString += " ORDER BY " + DB.ValueGroup.SortCodeCol.Id() + ", ";
            }
            else
            {
                sqlString += " ORDER BY "
                    + DB.ValueGroupLang2.SortCodeCol.Id(sortOrderLanguage) + ", ";
            }
            sqlString += DB.ValueGroup.GroupCodeCol.Id() + ", " +
            DB.ValueGroup.ValueCodeCol.Id();
            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK) {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " Grouping = " + aGrouping);
            }

            foreach (DataRow sqlRow in myRows) {
                ValueGroupRow outRow = new ValueGroupRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow);
            }
            return myOut;
        }

 

        #endregion for ValueGroup

    }
}
