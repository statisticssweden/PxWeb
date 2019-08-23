using System.Collections.Generic;
using System.Collections.Specialized;

namespace PCAxis.Sql.DbConfig
{

    /// <summary>
    /// Util Class for writing sqlcommands. For the columns which is not lang2.
    /// </summary>
    public class Col
    {

        //Name of column in table
        private string colName;

        // The alias of the table
        private string tableAlias;

       
        public Col(string colName, string tableAliasBase)
        {
            this.colName = colName;
            this.tableAlias = tableAliasBase;
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
            return this.Id() + " AS " + this.Label();
        }

        /// <summary>
        /// Id() + " = '" + constantValue + "'"
        /// </summary>
        /// <param name="constantValue">The single value to which the column should equal</param>
        /// <returns></returns>
        public string Is(string constantValue)
        {
            return this.Id() + " = '" + constantValue + "'"; 
        }

        /// <summary>
        /// Id() + " LIKE '" + wildcard + "'"
        /// </summary>
        /// <param name="wildcard">The expresion the column should match</param>
        /// <returns></returns>
        public string Like(string wildcard)
        {
            return this.Id() + " LIKE '" + wildcard + "'";
        }

        /// <summary>
        /// Id() + " IN (" + concatedValues + ")"
        /// </summary>
        /// <param name="listOfValues">Each values gets "'" around them and is joined by a , </param>
        /// <returns></returns>
        public string In(ICollection<string> listOfValues)
        {
            string concatedValues = "";
            string glue ="";
            ///<NumberOfValues: workaround for the 1000-cells limitation in SELECT WHERE IN in Oracle>
            int NumberOfValues = 1;
			concatedValues = "(" + this.Id() + " IN (";
			
            foreach (string val in listOfValues)
            {
                if (NumberOfValues >= 250)
                {
                    concatedValues = concatedValues + ") OR "+ this.Id() + " IN ('" + val + "'";
					NumberOfValues = 1;
                }
				else
				{
					concatedValues = concatedValues + glue + "'" + val + "'";
					glue = ", ";
					NumberOfValues++;
				}
            }
            return concatedValues + "))"; 
        }

        public string In(StringCollection listOfValues)     
        {
            string concatedValues = "";
            string glue ="";
            ///<NumberOfValues: workaround for the 1000-cells limitation in SELECT WHERE IN in Oracle>
            int NumberOfValues = 1;
			concatedValues = "(" + this.Id() + " IN (";
			
            foreach (string val in listOfValues)
            {
                if (NumberOfValues >= 250)
                {
                    concatedValues = concatedValues + ") OR "+ this.Id() + " IN ('" + val + "'";
					NumberOfValues = 1;
                }
				else
				{
					concatedValues = concatedValues + glue + "'" + val + "'";
					glue = ", ";
					NumberOfValues++;
				}
            }
            return concatedValues + "))"; 
        }

        /// <summary>
        /// "upper("+Id() + ") = upper('" + constantValue + "')"
        /// </summary>
        /// <param name="constantValue">The single value to which the column should equal</param>
        /// <returns></returns>
        public string IsUppered(string constantValue)
        {
            return "upper("+this.Id() + ") = upper('" + constantValue + "')";
        }


        /// <summary>
        /// Id() + " = " + otherCol.Id()
        /// </summary>
        /// <param name="otherCol">The column this columns must be equal to.</param>
        /// <returns></returns>
        public string Is(Col otherCol)
        {
            return this.Id() + " = " + otherCol.Id();
        }

        /// <summary>
        /// Id() + " = " + lang2Col.Id(lang)
        /// </summary>
        /// <param name="lang2Col">The lang2 column this columns must be equal to.</param>
        /// <param name="lang">The language</param>
        /// <returns></returns>
        public string Is(Lang2Col lang2Col, string lang)
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
        
    }
}
