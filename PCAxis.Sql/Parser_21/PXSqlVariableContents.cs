using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using PCAxis.Sql.QueryLib_21;
using PCAxis.Paxiom;
using log4net;

namespace PCAxis.Sql.Parser_21
{
    class PXSqlVariableContents : PXSqlVariable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlVariableContents));
    
        #region  contructor
        public PXSqlVariableContents(String name, PXSqlMeta_21 meta)
            : base(name, meta, true, false, false)
        {
            mStoreColumnNo = -1;
            SetSelected();
            if (!meta.ConstructedFromPxs)
            {
                mIndex = mStoreColumnNo;
            }              
            SetDefaultPresTextOption();
            SetPresText();
        }
        #endregion

        private void SetSelected()
        {
            this.mIsSelected = false;
            if (meta.ConstructedFromPxs)
            {
                if (meta.PxsFile.Query.Contents.Content.Length > 0)
                    this.isSelected = true;
            }
            else
                this.isSelected = true;

        }
        protected sealed override void SetPresText()
        {
            if (meta.MainTable.hasContentsVariable(meta.LanguageCodes[0]))
            {
                foreach (string langCode in meta.LanguageCodes)
                {
                    this.PresText[langCode] = meta.MainTable.getContentsVariable(langCode);
                }
            }
            else
            {
                TextCatalogRow myTextCatalogRow = meta.MetaQuery.GetTextCatalogRow(meta.Config.Keywords.ContentVariable);
                foreach (string langCode in meta.LanguageCodes)
                {
                    this.PresText[langCode] = myTextCatalogRow.texts[langCode].PresText;
                }
            }
        }

        private void SetDefaultPresTextOption()
        {
            // if prestextoption not set e.g from Pxs then apply PresTextOption from db
            if (string.IsNullOrEmpty(this.PresTextOption))
                this.PresTextOption = meta.Config.Codes.ValuePresT;
        }

        /// <PXKeyword name="CONTVARIABLE">
        ///   <rule>
        ///     <description>Hardcoded to ContentsCode</description>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="AGGREGALLOWED">
        ///   <rule>
        ///     <description>"No" if any of the selected content has a "no".</description>
        ///     <table modelName ="Contents">
        ///     <column modelName="AggregPossible"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        internal override void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage) {
 
            base.ParseMeta(handler, LanguageCodes, preferredLanguage);
            //CONTVARIABLE

            string language = null;
            string subkey = null;
            StringCollection values = new StringCollection();
            values.Add(this.Name);
            handler(PXKeywords.CONTVARIABLE, language, subkey, values);


            // AggregAllowed
            language = null;
            subkey = null;
            string tmpAggregPossible = PXConstant.YES;
            foreach (PXSqlContent pxsqlCont in meta.Contents.Values)
            {
               
                if (! pxsqlCont.AggregPossible)
                {
                    tmpAggregPossible = PXConstant.NO;
                    break;
                }
            }
            
            values.Clear();
            values.Add(tmpAggregPossible);
            handler(PXKeywords.AGGREGALLOWED, language, subkey, values);


            log.Debug("meta.Contents.Values.Count=" + meta.Contents.Values.Count.ToString());
            // "ContentInfo"
            // og PXKeywords.PRECISION 
            foreach (PXSqlContent pxsqlCont in meta.Contents.Values) {
                pxsqlCont.ParseMeta(handler, LanguageCodes);

            }
        }

        internal override List<PXSqlValue> GetValuesForParsing()
        {
            if ((meta.inPresentationModus) && meta.ConstructedFromPxs)
            {
                return mValues.GetValuesSortedByPxs(mValues.GetValuesForSelectedValueset(selectedValueset));
                //return GetValuesSortedDefault(GetValuesForSelectedValueset()); // old sorting Thomas say its how Old Pcaxis does
            }
            else
            {
                //todo; own sort rutine for contentsvalues ??
                //return mValues.GetValuesSortedByValue(mValues.GetValuesForSelectedValueset(selectedValueset));
                //TODO; the sortroutine above gave wrong result because StoreColumnNo in Contents was parsed to string in PXSQLContents and when used in sortroutine for 
                // valuecode stringcompare is used. SortByPXS use int compare.
                return mValues.GetValuesSortedByPxs(mValues.GetValuesForSelectedValueset(selectedValueset));
            }
        }

    }
}
