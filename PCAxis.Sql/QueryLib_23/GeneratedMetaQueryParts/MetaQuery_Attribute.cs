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
        public Dictionary<string, AttributeRow> GetAttributeRows(string aMainTable, bool emptyRowSetIsOK)
        {
            Dictionary<string, AttributeRow> myOut = new Dictionary<string, AttributeRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetAttribute_SQLString_NoWhere();
            //
            // WHERE ATT.MainTable = <"MainTable as parameter reference for your db vendor">
            //
            sqlString += " WHERE " + DB.Attribute.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1 && ! emptyRowSetIsOK)
            {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable);
            }

            foreach (DataRow sqlRow in myRows)
            {
                AttributeRow outRow = new AttributeRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.Attribute, outRow);
            }
            return myOut;
        }

        private String GetAttribute_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Attribute.MainTableCol.ForSelect() + ", " +
                DB.Attribute.AttributeCol.ForSelect() + ", " +
                DB.Attribute.AttributeColumnCol.ForSelect() + ", " +
                DB.Attribute.PresTextCol.ForSelect() + ", " +
                DB.Attribute.SequenceNoCol.ForSelect() + ", " +
                DB.Attribute.DescriptionCol.ForSelect() + ", " +
                DB.Attribute.ValueSetCol.ForSelect() + ", " +
                DB.Attribute.ColumnLengthCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.AttributeLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Attribute.PresTextCol);
                    sqlString += ", " + DB.AttributeLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.Attribute.DescriptionCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetAttributeRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.Attribute.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.AttributeLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Attribute.MainTableCol.Is(DB.AttributeLang2.MainTableCol, langCode) +
                                 " AND " + DB.Attribute.AttributeCol.Is(DB.AttributeLang2.AttributeCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
