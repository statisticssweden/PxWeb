using System;
using System.Collections.Specialized;
using PCAxis.Paxiom;

using log4net;

namespace PCAxis.Sql.Parser_23
{
    public class PXSqlParser_23 : PCAxis.PlugIn.Sql.PXSqlParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParser_23));
        private PXSqlMeta_23 mPXSqlMeta;

        private bool mHasParsedMeta = false;

        // private SqlDbConfig mConfig;
        internal PXSqlNpm symbols;
        #region Constructor

       // public PXSqlParser_23() { }


        public PXSqlParser_23(PXSqlMeta_23 inPXSqlMeta) {

            mPXSqlMeta = inPXSqlMeta;
            if (mPXSqlMeta.inPresentationModus)
            {
                symbols = mPXSqlMeta.mPxsqlNpm;
            }

        }


        #endregion
        #region ParseMeta
        /// <PXKeyword name="LANGUAGE">
        ///   <rule>
        ///     <description>If Preferredlanguage is set in the builder, use this,  else use main language specified in db config file</description>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="LANGUAGES">
        ///   <rule>
        ///     <description>If ReadAllLanguages in builder set to TRUE, then get all languages specified in db config file which applies for the selected table
        ///     else use the main language specified. 
        ///     If a pxs is input then use the lanuages specified here. </description>
        ///   </rule>
        /// </PXKeyword>
        /// 
        /// <PXKeyword name="DATANOTESUM">
        ///   <rule>
        ///     <description> </description>
        ///     <table modelName ="SpecialCharacter">
        ///     <column modelName="all"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// 
        /// <PXKeyword name="DATASYMBOLn">
        ///   <rule>
        ///     <description>n=1-6 </description>
        ///     <table modelName ="SpecialCharacter">
        ///     <column modelName="all"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// 
        /// <PXKeyword name="DATASYMBOLNIL">
        ///   <rule>
        ///     <description></description>
        ///     <table modelName ="SpecialCharacter">
        ///     <column modelName="all"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// 
        /// <PXKeyword name="DATASYMBOLSUM">
        ///   <rule>
        ///     <description></description>
        ///     <table modelName ="SpecialCharacter">
        ///     <column modelName="all"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        override public void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string preferredLanguage) {

            string keyword;
            
            string noLanguage = null;
            string subkey = null;
            StringCollection values;
            if (mHasParsedMeta) {
                return;
            }

            if (mPXSqlMeta.HasLanguage) {
                // Language
                keyword = PXKeywords.LANGUAGE;
                values = new StringCollection();
                values.Add(mPXSqlMeta.MainLanguageCode);
                handler(keyword, noLanguage, subkey, values);

                // Languages
                keyword = PXKeywords.LANGUAGES;
                values = new StringCollection();
                foreach (string lang in mPXSqlMeta.LanguageCodes) {
                    values.Add(lang);
                }
                handler(keyword, noLanguage, subkey, values);
            }

            (new ParseMetaIndependentOfMaintable()).ParseMeta(handler, mPXSqlMeta.PXMetaAdmValues);


            mPXSqlMeta.MainTable.ParseMeta(handler, mPXSqlMeta.LanguageCodes);



            mPXSqlMeta.CouldHaveBeenByMainTableOnly.ParseMeta(handler);



            foreach (PXSqlVariable var in mPXSqlMeta.Variables.Values) {
                var.ParseMeta(handler, mPXSqlMeta.LanguageCodes, preferredLanguage);

            }



            // Footnotes
            #region Footnotes

            mPXSqlMeta.TheNotes.ParseAllNotes(handler);

    
            
            #endregion Footnotes

            #region NPMish stuff

            if (mPXSqlMeta.inPresentationModus )
            {
                this.symbols.ParseMeta(handler, mPXSqlMeta);
            }
            #endregion NPMish stuff
        }



        #endregion



        /// <summary>
        /// IDisposable implemenatation
        /// </summary>
        override public void Dispose() {
            if (mPXSqlMeta != null) {
                mPXSqlMeta.Dispose();
            }
        }
        
    }
}

