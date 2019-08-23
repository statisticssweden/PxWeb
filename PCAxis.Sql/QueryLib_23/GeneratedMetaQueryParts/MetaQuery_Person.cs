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
        public PersonRow GetPersonRow(string aPersonCode)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetPerson_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Person.PersonCodeCol.Is(mSqlCommand.GetParameterRef("aPersonCode"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aPersonCode", aPersonCode);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," PersonCode = " + aPersonCode);
            }

            PersonRow myOut = new PersonRow(myRows[0], DB); 
            return myOut;
        }


        private String GetPerson_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Person.PersonCodeCol.ForSelect() + ", " +
                DB.Person.OrganizationCodeCol.ForSelect() + ", " +
                DB.Person.ForenameCol.ForSelect() + ", " +
                DB.Person.SurnameCol.ForSelect() + ", " +
                DB.Person.PhonePrefixCol.ForSelect() + ", " +
                DB.Person.PhoneNoCol.ForSelect() + ", " +
                DB.Person.FaxNoCol.ForSelect() + ", " +
                DB.Person.EmailCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetPersonRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Person.GetNameAndAlias();
            return sqlString;
        }
    }
}
