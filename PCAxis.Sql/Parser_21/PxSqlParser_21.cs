using System;
using System.Collections.Specialized;
using PCAxis.Paxiom;
//ny




using log4net;

namespace PCAxis.Sql.Parser_21
{
    public class PXSqlParser_21 : PCAxis.PlugIn.Sql.PXSqlParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParser_21));
        public PXSqlMeta_21 mPXSqlMeta;

        private bool mHasParsedMeta = false;

        // private SqlDbConfig mConfig;
        internal PXSqlNpm symbols;
        #region Constructor

        public PXSqlParser_21() { }


        public PXSqlParser_21(PXSqlMeta_21 inPXSqlMeta) {

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
            //CellNote
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.CellNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }

            //MainTableNote + SubTableNote + VariableNotes + ContentsNotes
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.PaxiomNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }
           
            //MaintValueNotes
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.MaintValueNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }

            //ValueNotes
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.ValueNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }
            //ContValueNotes
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.ContValueNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }
            //ContTimeNotes
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.ContTimeNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }
            
            //ContVblNotes
            foreach (string langCode in mPXSqlMeta.LanguageCodes) {
                mPXSqlMeta.ContVblNotes.ParseNote(mPXSqlMeta, handler, langCode);
            }
            
            
            #endregion Footnotes

            #region NPMish stuff

            if (mPXSqlMeta.inPresentationModus )
            {
                if (String.Compare(mPXSqlMeta.MetaModelVersion, "2.0", false, System.Globalization.CultureInfo.InvariantCulture) > 0)
                {
                    // NPM Characters
                    //DataNoteSum
                    {
                        subkey = null;
                        keyword = PXKeywords.DATANOTESUM;
                        foreach (string langCode in mPXSqlMeta.LanguageCodes)
                        {
                            values = new StringCollection();
                            values.Add(symbols.DataSymbolSumPresChar(langCode));
                            handler(keyword, langCode, subkey, values);
                        }
                        values = null;
                    }
                    //DataSymbolN
                    {

                        for (int i = 1; i <= symbols.maxDatasymbolN; i++)
                        {
                            subkey = null;
                            values = new StringCollection();
                            keyword = "DATASYMBOL" + i.ToString();
                            values.Add(symbols.DataSymbolNPresChar(i, mPXSqlMeta.LanguageCodes[0]));
                            handler(keyword, noLanguage, subkey, values);
                            values = null;
                        }
                    }
                    //DataSymbolNil
                    {
                        subkey = null;
                        values = new StringCollection();
                        keyword = PXKeywords.DATASYMBOLNIL;
                        values.Add(symbols.DataSymbolNilPresChar(mPXSqlMeta.LanguageCodes[0]));
                        handler(keyword, noLanguage, subkey, values);
                        values = null;
                    }
                    //DataSymbolSum
                    {
                        subkey = null;
                        values = new StringCollection();
                        keyword = PXKeywords.DATASYMBOLSUM;
                        values.Add(symbols.DataSymbolSumPresChar(mPXSqlMeta.LanguageCodes[0]));
                        handler(keyword, noLanguage, subkey, values);
                        values = null;
                    }
                }
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

