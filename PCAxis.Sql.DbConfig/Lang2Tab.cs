using System;
using System.Collections.Generic;
using System.Text;

namespace PCAxis.Sql.DbConfig
{
    public class Lang2Tab
    {
        
        // The name of the table in the database without language suffix
        private string localTableNameBase;

        /// <summary>
        /// The name of the table in the database without language suffix
        /// </summary>
        public String TableName
        {
            get { return localTableNameBase; }
        }
        
        // The alias of the table without language suffix
        private string tableAliasBase;
        /// <summary>
        /// // The alias of the table without language suffix
        /// </summary>
        public String Alias
        {
            get { return tableAliasBase; }
        }

        
        private Dictionary<string, string> metaSuffixByLanguage;
        internal Dictionary<string, string> Suffixes
        {
            get { return metaSuffixByLanguage; }
        }

        private string metaOwner;
        
       

        internal Lang2Tab(string tableAliasBase, string localTableNameBase, string metaOwner,Dictionary<string, string> metaSuffixByLanguage)
        {
            this.tableAliasBase = tableAliasBase;
            this.localTableNameBase = localTableNameBase;
            this.metaOwner = metaOwner;
            this.metaSuffixByLanguage = metaSuffixByLanguage;
        }

        /// <summary>"full_name full_alias" :
        /// metaOwner + localTableNameBase + Suffix[languageCode] 
        /// + " " 
        /// + tableAliasBase + Suffix[languageCode]
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public string GetNameAndAlias(string languageCode)
            {
                return this.metaOwner+this.localTableNameBase+this.metaSuffixByLanguage[languageCode] + " " + this.tableAliasBase + this.metaSuffixByLanguage[languageCode];
            }
    }
}
