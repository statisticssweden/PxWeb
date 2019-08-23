using System;
using System.Collections.Specialized;
using System.Collections.Generic;

using PCAxis.Paxiom;
using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Pxs;

using log4net;

namespace PCAxis.Sql.Parser
{

    /// <summary>
    /// Enumeration for selection or presentation
    /// </summary>
    public enum Instancemodus
    {
        selection = 0,
        presentation = 1
    }

    public abstract class PXSqlMeta : IDisposable
    {


        #region members
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlMeta));


        private string mCNMMVersion;
        /// <summary>
        /// As given in SqlDbConfig
        /// </summary>
        public string CNMMVersion
        {
            get { return mCNMMVersion; }

        }

        private Instancemodus mInstanceModus;

        private bool constructedFromPxs = false;
        public bool ConstructedFromPxs
        {
            get { return constructedFromPxs; }
        }


        /// <summary>
        /// We are building for selection
        /// </summary>
        public bool inSelectionModus { get { return !mInstanceModus.Equals(Instancemodus.presentation); } }

        /// <summary>
        /// We are building for presentation
        /// </summary>
        public bool inPresentationModus { get { return mInstanceModus.Equals(Instancemodus.presentation); } }


        protected bool mHasLanguage = false;
        public bool HasLanguage
        {
            get { return mHasLanguage; }
        }



        protected StringCollection mLanguageCodes;
        public StringCollection LanguageCodes
        {
            get { return mLanguageCodes; }
        }


        protected string mMainLanguageCode;
        public string MainLanguageCode
        {
            get { return mMainLanguageCode; }

        }


        private InfoForDbConnection mSelectedDbInfo;
        public InfoForDbConnection SelectedDbInfo
        {
            get { return mSelectedDbInfo; }
        }





        #endregion

        #region Constructor


        /// <summary>
        /// For use with maintableID
        /// </summary>
        /// <param name="mainTableId"></param>
        /// <param name="preferredLang">The code ("no","en",...) of the language the client wants as main language in paxiom. May be null or empty, indicating the client dont care, in which case a "random" language is choosen.</param>
        /// <param name="getAllLangs"></param>
        /// <param name="config"></param>
        /// <param name="selectedDbInfo"></param>
        /// <param name="aModus"></param>

        public static PXSqlMeta GetPXSqlMeta(string mainTableId, string preferredLang, bool getAllLangs, SqlDbConfig config, InfoForDbConnection selectedDbInfo, Instancemodus aModus, bool useTempTables)
        {
            if (config.MetaModel.Equals("2.1"))
            {
                return new Parser_21.PXSqlMeta_21(mainTableId, preferredLang, getAllLangs, config, selectedDbInfo, aModus);
            }
            else if (config.MetaModel.Equals("2.2"))
            {
                return new Parser_22.PXSqlMeta_22(mainTableId, preferredLang, getAllLangs, config, selectedDbInfo, aModus);
            }
            else if (config.MetaModel.Equals("2.3"))
            {
                return new Parser_23.PXSqlMeta_23(mainTableId, preferredLang, getAllLangs, config, selectedDbInfo, aModus);
            }
            else if (config.MetaModel.Equals("2.4"))
            {
                return new Parser_24.PXSqlMeta_24(mainTableId, preferredLang, getAllLangs, config, selectedDbInfo, aModus, useTempTables);
            }
            else
            {
                log.Debug("creating Parser_21.PXSqlMeta_21, but config.MetaModel is " + config.MetaModel);
                return new Parser_21.PXSqlMeta_21(mainTableId, preferredLang, getAllLangs, config, selectedDbInfo, aModus);
            }
        }

        public static PXSqlMeta GetPXSqlMeta(PxsQuery mPxsObject, string preferredLang, SqlDbConfig config, InfoForDbConnection selectedDbInfo, Instancemodus aModus, bool useTempTables)
        {
            if (config.MetaModel.Equals("2.1"))
            {
                return new Parser_21.PXSqlMeta_21(mPxsObject, preferredLang, config, selectedDbInfo, aModus);
            }
            else if (config.MetaModel.Equals("2.2"))
            {
                return new Parser_22.PXSqlMeta_22(mPxsObject, preferredLang, config, selectedDbInfo, aModus);
            }
            else if (config.MetaModel.Equals("2.3"))
            {
                return new Parser_23.PXSqlMeta_23(mPxsObject, preferredLang, config, selectedDbInfo, aModus);
            }
            else if (config.MetaModel.Equals("2.4"))
            {
                return new Parser_24.PXSqlMeta_24(mPxsObject, preferredLang, config, selectedDbInfo, aModus, useTempTables);
            }
            else
            {
                log.Debug("creating Parser_21.PXSqlMeta_21, but config.MetaModel is " + config.MetaModel);
                return new Parser_21.PXSqlMeta_21(mPxsObject, preferredLang, config, selectedDbInfo, aModus);
            }
        }



        protected PXSqlMeta(SqlDbConfig config, InfoForDbConnection selectedDbInfo, Instancemodus aModus, bool constructedFromPxs)
        {

            log.Info("PXSqlMeta  this.mCNMMVersion = " + config.MetaModel + " Instancemodus aModus(=" + aModus.ToString() + "constructedFromPxs=" + constructedFromPxs.ToString());

            this.mCNMMVersion = config.MetaModel;
            this.mSelectedDbInfo = selectedDbInfo;
            this.mInstanceModus = aModus;
            this.constructedFromPxs = constructedFromPxs;

        }





        #endregion





        public abstract bool MainTableContainsOnlyMetaData();

        public abstract InfoFromPxSqlMeta2PxsQuery GetInfoFromPxSqlMeta2PxsQuery();


        #region helproutines

        /// <summary>
        /// true if candidateList contains all values of allList 
        /// </summary>
        /// <param name="allList"></param>
        /// <param name="candidateList"></param>
        /// <returns></returns>
        internal static bool containsAll(StringCollection allList, StringCollection candidateList)
        {
            bool myOut = true;
            foreach (String aValue in allList)
            {
                if (!candidateList.Contains(aValue))
                {
                    myOut = false;

                }
            }
            return myOut;
        }

        /// <summary>
        /// Return false if all values != each other 
        /// </summary>
        /// <param name="ValuesToCompare"></param>
        /// <returns></returns>
        protected bool CompareListValues(List<string> ValuesToCompare)
        {
            string CompareValue = ValuesToCompare[0];
            foreach (string val in ValuesToCompare)
            {
                if (val != CompareValue)
                {
                    return false;
                }
                CompareValue = val;
            }
            return true;
        }





        #endregion


        /// <summary>Finds the variable and passes to call on to it</summary>
        /// <param name="paxiomVariable">The paxiom Varable, but both this and the name?? </param>
        /// <param name="variableCode">The variable to which the new Grouping should be applied</param>
        /// <param name="groupingId">The id of the new grouping</param>
        /// <param name="include">Emun value inducating what the new codelist should include:parents,childern or both </param>
        internal abstract void ApplyGrouping(Variable paxiomVariable, string variableCode, string groupingId, GroupingIncludesType include);

        /// <summary>Finds the variable and passes to call on</summary>
        /// <param name="variableCode">The variable to which the new ValueSet should be applied</param>
        /// <param name="valueSetId">The id of the new ValueSet</param>
        internal abstract void ApplyValueSet(string variableCode, string valueSetId);


        /// <summary>
        /// IDisposable implemenatation
        /// </summary>
        public abstract void Dispose();

    }
}

