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
        //returns the single "row" found when all PKs are spesified
        public TimeScaleRow GetTimeScaleRow(string aTimeScale)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetTimeScale_SQLString_NoWhere();
            sqlString += " WHERE " + DB.TimeScale.TimeScaleCol.Is(mSqlCommand.GetParameterRef("aTimeScale"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aTimeScale", aTimeScale);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," TimeScale = " + aTimeScale);
            }

            TimeScaleRow myOut = new TimeScaleRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetTimeScale_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.TimeScale.TimeScaleCol.ForSelect() + ", " +
                DB.TimeScale.PresTextCol.ForSelect() + ", " +
                DB.TimeScale.TimeScalePresCol.ForSelect() + ", " +
                DB.TimeScale.RegularCol.ForSelect() + ", " +
                DB.TimeScale.TimeUnitCol.ForSelect() + ", " +
                DB.TimeScale.FrequencyCol.ForSelect() + ", " +
                DB.TimeScale.StoreFormatCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.TimeScaleLang2.PresTextCol.ForSelectWithFallback(langCode, DB.TimeScale.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetTimeScaleRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.TimeScale.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.TimeScaleLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.TimeScale.TimeScaleCol.Is(DB.TimeScaleLang2.TimeScaleCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
