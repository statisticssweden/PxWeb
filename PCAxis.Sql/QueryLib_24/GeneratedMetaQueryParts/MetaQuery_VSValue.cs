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
        public VSValueRow GetVSValueRow(string aValueSet, string aValuePool, string aValueCode)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetVSValue_SQLString_NoWhere();
            sqlString += " WHERE " + DB.VSValue.ValueSetCol.Is(mSqlCommand.GetParameterRef("aValueSet")) + 
                             " AND " +DB.VSValue.ValuePoolCol.Is(mSqlCommand.GetParameterRef("aValuePool")) + 
                             " AND " +DB.VSValue.ValueCodeCol.Is(mSqlCommand.GetParameterRef("aValueCode"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[3];
            parameters[0] = mSqlCommand.GetStringParameter("aValueSet", aValueSet);
            parameters[1] = mSqlCommand.GetStringParameter("aValuePool", aValuePool);
            parameters[2] = mSqlCommand.GetStringParameter("aValueCode", aValueCode);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValueSet = " + aValueSet + " ValuePool = " + aValuePool + " ValueCode = " + aValueCode);
            }

            VSValueRow myOut = new VSValueRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetVSValue_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.VSValue.ValueSetCol.ForSelect() + ", " +
                DB.VSValue.ValuePoolCol.ForSelect() + ", " +
                DB.VSValue.ValueCodeCol.ForSelect() + ", " +
                DB.VSValue.SortCodeCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.VSValueLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.VSValue.SortCodeCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetVSValueRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.VSValue.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.VSValueLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.VSValue.ValueSetCol.Is(DB.VSValueLang2.ValueSetCol, langCode) +
                                 " AND " + DB.VSValue.ValuePoolCol.Is(DB.VSValueLang2.ValuePoolCol, langCode) +
                                 " AND " + DB.VSValue.ValueCodeCol.Is(DB.VSValueLang2.ValueCodeCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
