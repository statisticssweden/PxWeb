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
        public MetabaseInfoRow GetMetabaseInfoRow(string aModel)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMetabaseInfo_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MetabaseInfo.ModelCol.IsUppered(aModel) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," Model = " + aModel);
            }

            MetabaseInfoRow myOut = new MetabaseInfoRow(myRows[0], DB); 
            return myOut;
        }


        private String GetMetabaseInfo_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MetabaseInfo.ModelCol.ForSelect() + ", " +
                DB.MetabaseInfo.ModelVersionCol.ForSelect() + ", " +
                DB.MetabaseInfo.DatabaseRoleCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMetabaseInfoRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.MetabaseInfo.GetNameAndAlias();
            return sqlString;
        }
    }
}
