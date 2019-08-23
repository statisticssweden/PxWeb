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
        public ValueSetRow GetValueSetRow(string aValueSet)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetValueSet_SQLString_NoWhere();
            sqlString += " WHERE " + DB.ValueSet.ValueSetCol.Is(mSqlCommand.GetParameterRef("aValueSet"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aValueSet", aValueSet);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValueSet = " + aValueSet);
            }

            ValueSetRow myOut = new ValueSetRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetValueSet_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.ValueSet.ValueSetCol.ForSelect() + ", " +
                DB.ValueSet.PresTextCol.ForSelect() + ", " +
                DB.ValueSet.DescriptionCol.ForSelect() + ", " +
                DB.ValueSet.EliminationCol.ForSelect() + ", " +
                DB.ValueSet.ValuePoolCol.ForSelect() + ", " +
                DB.ValueSet.ValuePresCol.ForSelect() + ", " +
                DB.ValueSet.GeoAreaNoCol.ForSelect() + ", " +
                DB.ValueSet.MetaIdCol.ForSelect() + ", " +
                DB.ValueSet.SortCodeExistsCol.ForSelect() + ", " +
                DB.ValueSet.FootnoteCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.ValueSetLang2.PresTextCol.ForSelectWithFallback(langCode, DB.ValueSet.PresTextCol);
                    sqlString += ", " + DB.ValueSetLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.ValueSet.DescriptionCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetValueSetRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.ValueSet.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.ValueSetLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.ValueSet.ValueSetCol.Is(DB.ValueSetLang2.ValueSetCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
