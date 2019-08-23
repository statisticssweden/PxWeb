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
        public ValuePoolRow GetValuePoolRow(string aValuePool)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetValuePool_SQLString_NoWhere();
            sqlString += " WHERE " + DB.ValuePool.ValuePoolCol.Is(mSqlCommand.GetParameterRef("aValuePool"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aValuePool", aValuePool);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValuePool = " + aValuePool);
            }

            ValuePoolRow myOut = new ValuePoolRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetValuePool_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.ValuePool.ValuePoolCol.ForSelect() + ", " +
                DB.ValuePool.ValuePoolAliasCol.ForSelect() + ", " +
                DB.ValuePool.PresTextCol.ForSelect() + ", " +
                DB.ValuePool.DescriptionCol.ForSelect() + ", " +
                DB.ValuePool.ValueTextExistsCol.ForSelect() + ", " +
                DB.ValuePool.ValuePresCol.ForSelect() + ", " +
                DB.ValuePool.MetaIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.ValuePoolLang2.ValuePoolAliasCol.ForSelectWithFallback(langCode, DB.ValuePool.ValuePoolAliasCol);
                    sqlString += ", " + DB.ValuePoolLang2.PresTextCol.ForSelectWithFallback(langCode, DB.ValuePool.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetValuePoolRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.ValuePool.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.ValuePoolLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.ValuePool.ValuePoolCol.Is(DB.ValuePoolLang2.ValuePoolCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
