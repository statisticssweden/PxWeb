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
        //returns the single "row" found when all PKs are spesified
        public DataStorageRow GetDataStorageRow(string aProductCode)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetDataStorage_SQLString_NoWhere();
            sqlString += " WHERE " + DB.DataStorage.ProductCodeCol.Is(mSqlCommand.GetParameterRef("aProductCode"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aProductCode", aProductCode);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," ProductCode = " + aProductCode);
            }

            DataStorageRow myOut = new DataStorageRow(myRows[0], DB); 
            return myOut;
        }


        private String GetDataStorage_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.DataStorage.ProductCodeCol.ForSelect() + ", " +
                DB.DataStorage.ServerNameCol.ForSelect() + ", " +
                DB.DataStorage.DatabaseNameCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetDataStorageRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.DataStorage.GetNameAndAlias();
            return sqlString;
        }
    }
}
