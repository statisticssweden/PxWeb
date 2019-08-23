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
        public int GetNumberOfDistinctAttributeColumns(string aMainTable)
        {
            SqlDbConfig dbconf = DB;

            string sqlString = "select count(distinct " + DB.Attribute.AttributeColumnCol.PureColumnName() + ") FROM " + DB.Attribute.GetNameAndAlias() +
                " WHERE " + DB.Attribute.MainTableCol.Is();

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];

            parameters[0] = DB.Attribute.MainTableCol.GetStringParameter(aMainTable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35, " MainTable = " + aMainTable);
            }
            return int.Parse(myRows[0][0].ToString());
        }

    }
}
