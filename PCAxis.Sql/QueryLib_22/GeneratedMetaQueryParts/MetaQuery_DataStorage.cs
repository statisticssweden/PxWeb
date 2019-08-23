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
        public DataStorageRow GetDataStorageRow(string aProductId)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetDataStorage_SQLString_NoWhere();
            sqlString += " WHERE " + DB.DataStorage.ProductIdCol.Is(aProductId) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," ProductId = " + aProductId);
            }

            DataStorageRow myOut = new DataStorageRow(myRows[0], DB); 
            return myOut;
        }


        private String GetDataStorage_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.DataStorage.ProductIdCol.ForSelect() + ", " +
                DB.DataStorage.ServerNameCol.ForSelect() + ", " +
                DB.DataStorage.DatabaseNameCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetDataStorageRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.DataStorage.GetNameAndAlias();
            return sqlString;
        }
    }
}
