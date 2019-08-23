using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

using Oracle.ManagedDataAccess.Client;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Text;// For Oracle-connections.

namespace PCAxis.Sql.DbConfig
{


    /// <summary>
    /// Util Class for writing Parameterized sqlcommands. For the columns which is not lang2.
    /// </summary>
    public class Column4Parameterized
    {

        //Name of column in table
        private string colName;

        // The alias of the table
        private string tableAlias;

        //The modelName (for creating parameters )
        private string modelName;

        //"OleDb","Oracle","Sql","Odbc" (for creating parameters )
        private string dbProvider;


        /// <summary>
        /// The constructor, the two last parameters are only use for  creating parameters
        /// </summary>
        public Column4Parameterized(string colName, string tableAliasBase, string modelName, string dbProvider)
        {
            this.colName = colName;
            this.tableAlias = tableAliasBase;
            this.modelName = modelName;
            this.dbProvider = dbProvider;
        }


        /// <summary>
        /// Just colName, normally not very usefulll
        /// </summary>
        /// <returns></returns>
        public string PureColumnName()
        {
            return this.colName;
        }


        /// <summary>
        /// tableAlias + "." + colName
        /// </summary>
        /// <returns></returns>
        public string Id()
        {
            return this.tableAlias + "." + this.colName;
        }

        /// <summary>
        /// tableAlias + "_" + colName
        /// </summary>
        /// <returns></returns>
        public string Label()
        {
            return this.tableAlias + "_" + this.colName;
        }

        /// <summary>
        /// Id() + " AS " + Label()
        /// </summary>
        /// <returns></returns>
        public string ForSelect()
        {
            var doubleQuote = @"""";
            if (this.PureColumnName().ToUpper() == "DEFAULT") return this.tableAlias + "." + doubleQuote +  this.PureColumnName() + doubleQuote + " AS " + this.Label();
            return this.Id() + " AS " + this.Label();
        }


        

        /// <summary>
        /// Id() + " = " + parameterRef + "'"
        /// </summary>
        /// <param name="parameterRef">The reference to a parameter, e.g. @maintable,:maintable or just ? depending on your db</param>
        /// <returns></returns>
        public string Is(string parameterRef)
        {
            return this.Id() + " = " + parameterRef;
        }

        /// <summary>
        /// Id() + " = " + parameter_with_default_name
        /// </summary>
        /// <returns>string</returns>
        public string Is()
        {
            return this.Is(GetParameterReference(this.modelName, this.dbProvider));
        }


        /// <summary>
        /// upper( Id() + " ) = upper( " + parameterRef + "'"  )
        /// </summary>
        /// <param name="parameterRef">The reference to a parameter, e.g. @maintable,:maintable or just ? depending on your db</param>
        /// <returns></returns>
        public string IsUppered(string parameterRef)
        {
            return "upper(" + this.Id() + ") = upper(" + parameterRef + ")";
        }


        /// <summary>
        /// upper( Id() ) = upper( parameter_with_default_name )
        /// </summary>
        public string IsUppered()
        {
            return "upper(" + this.Id() + ") = upper(" + GetParameterReference(this.modelName, this.dbProvider) + ")";
        }


        /// <summary>
        /// Id() >= parameter_with_default_name
        /// </summary>
        public string GreaterOrEqual()
        {
            return this.Id() + " >= " + GetParameterReference(this.modelName, this.dbProvider);
        }

        /// <summary>
        /// Id() + " LIKE parameter_with_default_name
        /// </summary>
        /// <param name="wildcard">The expresion the column should match</param>
        /// <returns></returns>
        public string Like()
        {
            return this.Id() + " LIKE " + GetParameterReference(this.modelName, this.dbProvider);
        }



        /// <summary>
        /// Id()   in (  parameterRef1 ,,,,  parameterRefN )
        /// </summary>
        /// <param name="parameterRef">The reference to a parameter, e.g. @maintable,:maintable or just ? depending on your db</param>
        /// <returns></returns>
        public string In(string parameterRefBase, int numberOfValues)
        {
            string concatedValues = "";

            string glue = "";

            string tmpParameterRef = "?";  //Odbc and Ole uses ? for all, and don't really use the parameter name.

            for (int counter = 1; counter <= numberOfValues; counter++)
            {
                if (!parameterRefBase.Equals("?"))
                {
                    //Our vendor uses the parameternames
                    tmpParameterRef = parameterRefBase + counter;
                }

                concatedValues = concatedValues + glue + tmpParameterRef;
                glue = ", ";
            }
            return this.Id() + " IN (" + concatedValues + ")";
        }




        /// <summary>
        /// Id() + " = " + otherCol.Id()
        /// </summary>
        /// <param name="otherCol">The column this columns must be equal to.</param>
        /// <returns></returns>
        public string Is(Column4Parameterized otherCol)
        {
            return this.Id() + " = " + otherCol.Id();
        }

        /// <summary>
        /// Id() + " = " + lang2Col.Id(lang)
        /// </summary>
        /// <param name="lang2Col">The lang2 column this columns must be equal to.</param>
        /// <param name="lang">The language</param>
        /// <returns></returns>
        public string Is(Lang2Column4Parameterized lang2Col, string lang)
        {
            return this.Id() + " = " + lang2Col.Id(lang);
        }

        /// <summary>
        /// Id() + " = " + lang2Col.Id(lang)
        /// </summary>
        public string IsNotNULL()
        {
            return this.Id() + " IS NOT NULL";
        }

        public const int InSplitTreshold = 250;

        /// <summary>
        /// ( Id()  IN  ( parameter_with_default_name1,,,parameter_with_default_name250) OR Id()  IN  (parameter_with_default_name251,,,parameter_with_default_name500 ) OR ..)
        /// </summary>
        /// <param name="listOfValues">Each values gets "'" around them and is joined by a , </param>
        /// <returns></returns>
        public string In(StringCollection listOfValues)
        {
            StringBuilder tmpOut = new StringBuilder();
            string start = "(" + this.Id() + " IN ( ";
            int paramCounter = 1;

            if (listOfValues.Count > 0)
            {
                tmpOut.Append("( " + start);

                int NumberOfCodes = 0;

                foreach (string VC in listOfValues)
                {
                    if (NumberOfCodes >= InSplitTreshold)
                    {
                        tmpOut.Append(")) OR " + start);

                        NumberOfCodes = 0;
                    }

                    if (NumberOfCodes > 0)
                    {
                        tmpOut.Append(",");
                    }
                    tmpOut.Append(GetParameterReference(this.modelName + paramCounter, this.dbProvider));
                    paramCounter++;
                    NumberOfCodes++;
                }

                tmpOut.Append(")))");
            }

            return tmpOut.ToString();
        }


        /// <summary>
        /// Returns a DbParameter[] with type= string, the default parameterName(with a 1 based counter) and value as given in parameterValue
        /// </summary>
        public DbParameter[] GetStringParameters(StringCollection parameterValues)
        {
            DbParameter[] myOut = new DbParameter[parameterValues.Count];

            for (int counter = 0; counter < parameterValues.Count; counter++)
            {
                myOut[counter] = GetStringParameter(parameterValues[counter], "a" + this.modelName + (counter + 1));
            }

            return myOut;
        }

        /// <summary>
        /// Returns a DbParameter with type= string, the default parameterName and value as given in parameterValue
        /// </summary>
        public DbParameter GetStringParameter(string parameterValue)
        {
            return this.GetStringParameter(parameterValue, "a" + this.modelName);
        }

        /// <summary>
        /// Returns a DbParameter with type= string and  parameterName and parameterValue as given
        /// </summary>
        public DbParameter GetStringParameter(string parameterValue, string parameterName)
        {
            DbParameter myOut = GetEmptyDbParameter(this.dbProvider);
            myOut.DbType = DbType.String;
            myOut.ParameterName = parameterName;
            myOut.Value = parameterValue;
            return myOut;
        }

        private static DbParameter GetEmptyDbParameter(string dataProvider)
        {
            DbParameter myOut = null;
            string lDataProvider = dataProvider.ToUpper();
            if (lDataProvider.Equals("ORACLE"))
            {
                myOut = new OracleParameter();
            }
            else if (lDataProvider.Equals("SQL"))
            {
                myOut = new SqlParameter();
            }
            else if (lDataProvider.Equals("OLEDB"))
            {
                myOut = new OleDbParameter();
            }
            else if (lDataProvider.Equals("ODBC"))
            {
                myOut = new OdbcParameter();
            }
            else
            {
                throw new Exceptions.ConfigException(dataProvider + " should be one of  \"OleDb\",\"Oracle\",\"Sql\", \"Odbc\"");
            }

            return myOut;
        }

        /// <summary>
        /// The reference to a parameter, e.g. @maintable,:maintable or just ? depending on your db
        /// </summary>
        /// <param name="propertyName">The "base" e.g. maintable</param>
        /// <param name="dataProvider">"OleDb","Oracle","Sql","Odbc"</param>
        /// <returns>string</returns>
        private static string GetParameterReference(string propertyName, string dataProvider)
        {
            string myOut = "";
            string lDataProvider = dataProvider.ToUpper();
            if (lDataProvider.Equals("ORACLE"))
            {
                myOut = ":a" + propertyName;
            }
            else if (lDataProvider.Equals("SQL"))
            {
                myOut = "@a" + propertyName;
            }
            else if (lDataProvider.Equals("OLEDB") || (lDataProvider.Equals("ODBC")))
            {
                myOut = "?";
            }
            else
            {
                throw new Exceptions.ConfigException(dataProvider + " should be one of  \"OleDb\",\"Oracle\",\"Sql\", \"Odbc\"");
            }

            return myOut;
        }



    }
}
