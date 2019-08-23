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
        public OrganizationRow GetOrganizationRow(string aOrganizationCode)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetOrganization_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Organization.OrganizationCodeCol.Is(mSqlCommand.GetParameterRef("aOrganizationCode"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aOrganizationCode", aOrganizationCode);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," OrganizationCode = " + aOrganizationCode);
            }

            OrganizationRow myOut = new OrganizationRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }


        private String GetOrganization_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Organization.OrganizationCodeCol.ForSelect() + ", " +
                DB.Organization.OrganizationNameCol.ForSelect() + ", " +
                DB.Organization.DepartmentCol.ForSelect() + ", " +
                DB.Organization.UnitCol.ForSelect() + ", " +
                DB.Organization.WebAddressCol.ForSelect() + ", " +
                DB.Organization.MetaIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.OrganizationLang2.OrganizationNameCol.ForSelectWithFallback(langCode, DB.Organization.OrganizationNameCol);
                    sqlString += ", " + DB.OrganizationLang2.DepartmentCol.ForSelectWithFallback(langCode, DB.Organization.DepartmentCol);
                    sqlString += ", " + DB.OrganizationLang2.UnitCol.ForSelectWithFallback(langCode, DB.Organization.UnitCol);
                    sqlString += ", " + DB.OrganizationLang2.WebAddressCol.ForSelectWithFallback(langCode, DB.Organization.WebAddressCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetOrganizationRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Organization.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.OrganizationLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Organization.OrganizationCodeCol.Is(DB.OrganizationLang2.OrganizationCodeCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
