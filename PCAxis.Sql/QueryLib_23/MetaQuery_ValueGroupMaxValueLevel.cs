using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;



namespace PCAxis.Sql.QueryLib_23 {

    public partial class MetaQuery
    {
        #region for GetValueGroupMaxValueLevel
        public string GetValueGroupMaxValueLevel(string aGrouping, bool emptyRowSetIsOK)
        {
            string myOut;
            SqlDbConfig dbconf = DB;
            string sqlString = "SELECT ";
            sqlString +=
                "MAX(" +
                DB.ValueGroup.GroupLevelCol.Id() +") ";
            sqlString += " FROM " + DB.ValueGroup.GetNameAndAlias();
            sqlString += " WHERE " + DB.ValueGroup.GroupingCol.Is(mSqlCommand.GetParameterRef("aGrouping"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aGrouping", aGrouping);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count < 1 && ! emptyRowSetIsOK) {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " Grouping = " + aGrouping);
            }
            myOut = myRows[0][0].ToString();
            return myOut;
        }
        #endregion for GetValueGroupMaxValueLevel
    }
}
