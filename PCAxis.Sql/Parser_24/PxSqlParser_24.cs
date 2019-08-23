using System;
using System.Collections.Specialized;
using PCAxis.Paxiom;
using System.Linq;
using System.Collections.Generic;

using log4net;

namespace PCAxis.Sql.Parser_24
{
    public class PXSqlParser_24 : PCAxis.PlugIn.Sql.PXSqlParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParser_24));
        private PXSqlMeta_24 mPXSqlMeta;

        private bool mHasParsedMeta = false;

        // private SqlDbConfig mConfig;
        internal PXSqlNpm symbols;
        #region Constructor

       // public PXSqlParser_24() { }


        public PXSqlParser_24(PXSqlMeta_24 inPXSqlMeta) {

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

        override public void SetCurrentValueSets(PXMeta pxMeta)
        {
            // The database alouds to have several default values but from the business point of view and also GUI point of view there can be only one default value.
            // Due to that this method will exit first time it "hits" a default value. Det default SubTableVariable might have a default grouping.

            var defaultValueSetBySubTableVar = new Dictionary<PXSqlVariable, string>();
            
            //This will set the default SubTableVariable if set
            foreach (var var in this.mPXSqlMeta.VariablesClassification)
            {
                var varName = var.Key;

                if (var.Value != null && var.Value.ValueSets != null)
                {
                    foreach (var valueSet in var.Value.ValueSets)
                    {
                        if (valueSet.Value.IsDefault)
                        {
                            var variable = pxMeta.Variables.GetByCode(varName);

                            variable.CurrentValueSet = variable.GetValuesetById(valueSet.Key);
                            if (variable.CurrentValueSet != null)
                            {
                                defaultValueSetBySubTableVar.Add(var.Value, variable.CurrentValueSet.ID);
                            }
                            
                            break;
                        }
                    }
                }    
            }

            //This will set the default grouping if set and not default SubTableVariable is set
            foreach (var var in this.mPXSqlMeta.VariablesClassification)
	        {
                var varName = var.Key;

                if (var.Value != null && var.Value.GroupingInfos != null)
                {
                    foreach (var groupingInfo in var.Value.GroupingInfos.Infos)
                    {
                        if (groupingInfo.IsDefault)
                        {
                            //if the grouping belongs to a SubTableVariable and another SubTableVariable is default, the grouping cannot be set as deault(ApplyGrouping)
                            if (defaultValueSetBySubTableVar.ContainsKey(var.Value)) 
                            {
                                var valueSetId = defaultValueSetBySubTableVar[var.Value];
                                if (!groupingInfo.ValueSetIds.Contains(valueSetId)) continue;
                            }

                            var variable = pxMeta.Variables.GetByCode(varName);
                            var.Value.ApplyGrouping(variable, groupingInfo.GroupingId, GroupingIncludesType.All);
                        }
                    }
                }
	        }
            
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

