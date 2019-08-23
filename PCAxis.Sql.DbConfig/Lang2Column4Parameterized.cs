using System.Collections.Generic;

namespace PCAxis.Sql.DbConfig
{

    /// <summary>
    /// Util Class for writing Parameterized sqlcommands. For the columns which is lang2.
    /// </summary>
    public class Lang2Column4Parameterized
    {
        //Name of column in table
        private string colName;

        // The alias of the table without language suffix
        private string tableAliasBase;

        //metaSuffixByLanguage
        private Dictionary<string, string> suffixes;

        public Lang2Column4Parameterized(string colName, string tableAliasBase, Dictionary<string, string> metaSuffixByLanguage)
        {
            this.colName = colName;
            this.tableAliasBase = tableAliasBase;
            this.suffixes = metaSuffixByLanguage;
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
        /// tableAliasBase + suffixes[language] + "." + colName
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string Id(string language)
        {
            return this.tableAliasBase + suffixes[language] + "." + this.colName;
        }

        /// <summary>
        /// tableAliasBase + suffixes[language] + "_" + colName
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string Label(string language)
        {
            return this.tableAliasBase + suffixes[language] + "_" + this.colName;
        }

        /// <summary>
        /// Id(language) + " AS " + Label(language)
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string ForSelect(string language)
        {
            return this.Id(language) + " AS " + this.Label(language);
        }

        /// <summary>
        /// Use the value from col if this column is null. 
        /// "CASE WHEN " + this.Id(langCode) + " IS NOT NULL THEN " + Id(langCode) + 
        /// " ELSE " + col.Id() + " END " + 
        /// " AS " + this.Label(langCode)
        /// </summary>
        /// <param name="langCode"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string ForSelectWithFallback(string langCode, Column4Parameterized col)
        {
            return "CASE WHEN " + this.Id(langCode) + " IS NOT NULL THEN " +
                         this.Id(langCode) +
                         " ELSE " + col.Id() + " END " +
                         " AS " + this.Label(langCode) + " ";
        }
    }
}
