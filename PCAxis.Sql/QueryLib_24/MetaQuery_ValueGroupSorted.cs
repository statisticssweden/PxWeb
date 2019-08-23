using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;



namespace PCAxis.Sql.QueryLib_24 {

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
            System.Data.Common.DbParameter[] parameters = null;

            sqlString += " WHERE " + DB.ValueGroup.GroupingCol.Is();
            if (aLevel != null)
            {
                sqlString += " AND " + DB.ValueGroup.GroupLevelCol.Is();

                parameters = new System.Data.Common.DbParameter[2];
                parameters[0] = DB.ValueGroup.GroupingCol.GetStringParameter(aGrouping);
                parameters[1] =  DB.ValueGroup.GroupLevelCol.GetStringParameter(aLevel);
            } 
            else
            {
                parameters = new System.Data.Common.DbParameter[1];
                parameters[0] = DB.ValueGroup.GroupingCol.GetStringParameter(aGrouping);
            }

            if (sortOnPrimaryLanguage)
            {
                sqlString += " ORDER BY " + DB.ValueGroup.SortCodeCol.Id() + ", ";
            }
            else
            {
                sqlString += " ORDER BY "
                    + DB.ValueGroupLang2.SortCodeCol.Id(sortOrderLanguage) + ", " ;
            }
            sqlString += DB.ValueGroup.GroupCodeCol.Id() + ", " +
            DB.ValueGroup.ValueCodeCol.Id();
            

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
             
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
