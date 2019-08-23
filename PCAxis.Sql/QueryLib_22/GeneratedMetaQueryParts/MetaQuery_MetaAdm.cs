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
        public MetaAdmRow GetMetaAdmRow(string aProperty)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMetaAdm_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MetaAdm.PropertyCol.IsUppered(aProperty) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," Property = " + aProperty);
            }

            MetaAdmRow myOut = new MetaAdmRow(myRows[0], DB); 
            return myOut;
        }

        //returns the all  "rows" found in database
        public Dictionary<string, MetaAdmRow> GetMetaAdmAllRows()
        {
            string sqlString = GetMetaAdm_SQLString_NoWhere();
            Dictionary<string, MetaAdmRow> myOut = new Dictionary<string, MetaAdmRow>();

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(44, "MetaAdm", "METAADM");
            }

            foreach (DataRow sqlRow in myRows)
            {
                MetaAdmRow outRow = new MetaAdmRow(sqlRow, DB);
                myOut.Add(outRow.Property, outRow);
            }
            return myOut;
        }


        private String GetMetaAdm_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MetaAdm.PropertyCol.ForSelect() + ", " +
                DB.MetaAdm.ValueCol.ForSelect() + ", " +
                DB.MetaAdm.DescriptionCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMetaAdmAllRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.MetaAdm.GetNameAndAlias();
            return sqlString;
        }
    }
}
