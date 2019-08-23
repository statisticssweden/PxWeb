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
        public SpecialCharacterRow GetSpecialCharacterRow(string aCharacterType)
        {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetSpecialCharacter_SQLString_NoWhere();
            sqlString += " WHERE " + DB.SpecialCharacter.CharacterTypeCol.Is(mSqlCommand.GetParameterRef("aCharacterType"));

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aCharacterType", aCharacterType);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36," CharacterType = " + aCharacterType);
            }

            SpecialCharacterRow myOut = new SpecialCharacterRow(myRows[0], DB, mLanguageCodes); 
            return myOut;
        }

        //returns the all  "rows" found in database
        public Dictionary<string, SpecialCharacterRow> GetSpecialCharacterAllRows()
        {
            string sqlString = GetSpecialCharacter_SQLString_NoWhere();
            Dictionary<string, SpecialCharacterRow> myOut = new Dictionary<string, SpecialCharacterRow>();

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, null);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(44, "SpecialCharacter", "SPECIALCHARACTER");
            }

            foreach (DataRow sqlRow in myRows)
            {
                SpecialCharacterRow outRow = new SpecialCharacterRow(sqlRow, DB, mLanguageCodes);
                myOut.Add(outRow.CharacterType, outRow);
            }
            return myOut;
        }


        private String GetSpecialCharacter_SQLString_NoWhere()
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.SpecialCharacter.CharacterTypeCol.ForSelect() + ", " +
                DB.SpecialCharacter.PresCharacterCol.ForSelect() + ", " +
                DB.SpecialCharacter.AggregPossibleCol.ForSelect() + ", " +
                DB.SpecialCharacter.DataCellPresCol.ForSelect() + ", " +
                DB.SpecialCharacter.DataCellFilledCol.ForSelect() + ", " +
                DB.SpecialCharacter.PresTextCol.ForSelect();


            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.SpecialCharacterLang2.PresCharacterCol.ForSelectWithFallback(langCode, DB.SpecialCharacter.PresCharacterCol);
                    sqlString += ", " + DB.SpecialCharacterLang2.PresTextCol.ForSelectWithFallback(langCode, DB.SpecialCharacter.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetSpecialCharacterAllRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.SpecialCharacter.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN "  + DB.SpecialCharacterLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.SpecialCharacter.CharacterTypeCol.Is(DB.SpecialCharacterLang2.CharacterTypeCol, langCode);
                }
            }

            return sqlString;
        }
    }
}
