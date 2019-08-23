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
        public VariableRow GetVariableRow(string aVariable)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetVariable_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Variable.VariableCol.Is(mSqlCommand.GetParameterRef("aVariable"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aVariable", aVariable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," Variable = " + aVariable);
            }

            VariableRow myOut = new VariableRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetVariable_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Variable.VariableCol.ForSelect() + ", " +
                DB.Variable.PresTextCol.ForSelect() + ", " +
                DB.Variable.VariableInfoCol.ForSelect() + ", " +
                DB.Variable.MetaIdCol.ForSelect() + ", " +
                DB.Variable.FootnoteCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.VariableLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Variable.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetVariableRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Variable.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.VariableLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Variable.VariableCol.Is(DB.VariableLang2.VariableCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
