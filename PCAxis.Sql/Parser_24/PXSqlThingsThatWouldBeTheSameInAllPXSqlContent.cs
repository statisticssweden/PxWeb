using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Paxiom;
using log4net;
using PCAxis.Sql.QueryLib_24;
using PCAxis.Sql.DbConfig;
using System.Collections.Specialized;

namespace PCAxis.Sql.Parser_24 {

    ///<summary>Class for columns in the Content-tabell that are independent of content (they will possibly be 
    /// moved to maintabletable in a later version of the metamodel)</summary>
    class PXSqlThingsThatWouldBeTheSameInAllPXSqlContent {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlThingsThatWouldBeTheSameInAllPXSqlContent));

        /// <summary> When not sure, Copyright = true is used.
        /// Metamodell (2.2) documentation: 
        /// Copyright: Here it should be shown whether material is included in Sweden's Official Statistics (SOS) or not,
        /// and which rules apply to the distribution of the statistics (copyright). All content columns within a main table
        /// must belong to the same category, i.e. the same code should be in all the fields. 
        /// There are the following alternatives:
        /// 1 = included in Sweden's Official Statistics (no copyright)
        /// 2 = not included in Sweden's Official Statistics (no copyright)
        /// 3 = not included in Sweden's Official Statistics (copyright)
        /// </summary>
        private bool mCopyright;

        /// <summary>
        /// Metamodell (2.2) documentation: 
        /// StatAuthority: Code for the authority responsible for the statistics (statistical authority).
        ///All content columns in a main table must belong to the same statistical authority.
        ///Data is taken from the column OrganizationCode in the table Organization. For a more detailed description, see this table.
        /// </summary>

        private string mStatAuthorityCode;

        //the translated name of StatAuthority indexed in language code

        /// <summary>
        /// Metamodell (2.2) documentation: 
        /// OfficialStatistics:Data is taken from column copyright. copyright=1 means Official statistics
        /// </summary>
        private bool mOfficialStatistics;

        private Dictionary<string, string> nameByLangCode = new Dictionary<string, string>();
        
        internal PXSqlThingsThatWouldBeTheSameInAllPXSqlContent(ContentsRow someContentsRow, PXSqlMeta_24 meta, SqlDbConfig_24 config) {
            mStatAuthorityCode = someContentsRow.StatAuthority;
            string copyright = someContentsRow.Copyright;
            if (copyright.Equals(config.Codes.Copyright1) || copyright.Equals(config.Codes.Copyright2)) {
                mCopyright = false;
            } else if (copyright.Equals(config.Codes.Copyright3)) {
                mCopyright = true ;
            } else {
                mCopyright = true;
                log.Error("The database has copyright=" + copyright + ", but the valid codes from config are " + config.Codes.Copyright1 + "," + config.Codes.Copyright2 + " or " + config.Codes.Copyright3 + ".");
            }
            if (copyright.Equals(config.Codes.Copyright1)) {
                mOfficialStatistics = true;
            }    else {
                   mOfficialStatistics = false;
            }
            OrganizationRow org = meta.MetaQuery.GetOrganizationRow(mStatAuthorityCode);
            foreach (string language in org.texts.Keys) {
                nameByLangCode.Add(language, org.texts[language].OrganizationName);
            }
        }


        /// <summary>For PXKeywords.SOURCE and PXKeywords.COPYRIGHT
        /// </summary>
        /// <param name="handler"></param>
        /// <PXKeyword name="SOURCE">
        ///   <rule>
        ///     <description>Language dependent.</description>
        ///     <table modelName ="Organization">
        ///     <column modelname="OrganizationName"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>  
        /// <PXKeyword name="COPYRIGHT">
        ///   <rule>
        ///     <description>If db config value code is Copyright1 or Copyright2 then PXKeywords.COPYRIGHT is set to PXConstant.NO else set to PXConstant.YES</description>
        ///     <table modelName ="Organization">
        ///     <column modelname="Copyright"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>   
        public void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler) {

            StringCollection values = new StringCollection();

            string subkey = null;

            foreach (string langCode in nameByLangCode.Keys) {

                // SOURCE
                values.Clear();
                values.Add(this.nameByLangCode[langCode]);
                handler(PXKeywords.SOURCE, langCode, subkey, values);

            }
            string noLanguage = null;

            // COPYRIGHT
            values.Clear();
            if (mCopyright) {
                values.Add(PXConstant.YES);
            } else {
                values.Add(PXConstant.NO);
            }
            handler(PXKeywords.COPYRIGHT, noLanguage, subkey, values);

            // OFFICIALSTATISTICS
            values.Clear();
            if (mOfficialStatistics)
            {
                values.Add(PXConstant.YES);
            }
            else
            {
                values.Add(PXConstant.NO);
            }
            handler(PXKeywords.OFFICIAL_STATISTICS, noLanguage, subkey, values);
        }
    }
}
