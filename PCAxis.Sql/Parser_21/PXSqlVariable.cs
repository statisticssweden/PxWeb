using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using PCAxis.Sql.QueryLib_21;
using PCAxis.Paxiom;
using PCAxis.Sql.Pxs;
using PCAxis.PlugIn.Sql;

namespace PCAxis.Sql.Parser_21
{
    public abstract class PXSqlVariable : IComparable, ISqlItem
    {

        #region members
        protected PXSqlMeta_21 meta;
        protected MetaQuery metaQuery;
        private string mName;
        private bool mIsContentVariable;
        private bool mIsTimeVariable;
        private bool mIsClassificationVariable;
        protected bool mIsSelected;

        private Dictionary<string, string> mPresText;
        private bool mIsWildCardsUsed;
        protected int mIndex;
        private bool mIsStub;
        private bool mIsHeading;
        protected string mPresTextOption;
        protected PxSqlValues mValues;
        private int mNumberOfPxsValues;
        private int mTotalNumberOfValuesInDB;
        private string mTempTableName;
        private string mTempTableNo;
        private bool mVarNoteExist;
        private bool mValueNoteExist;
        private bool mDoubleColumn;

        // only classification should be moved
        protected int mStoreColumnNo;
        private bool mIsEliminatedByValue;
        private bool mUsesGrouping = false;
        private string mVariableType;

        private String mValueTextOption = PXConstant.VALUETEXTOPTION_NORMAL;
        
        //protected const string allValuesets = "_ALL_";
        protected string selectedValueset;
        public string SelectedValueset
        {
            get { return selectedValueset; }
        }
        //private Dictionary<string, string> mPaxiomElimination;
        private string mPaxiomElimination;
        private string mPaxiomMap;
        protected PXSqlValueSet mValueSet;
        protected Dictionary<string, PXSqlValueSet> mValueSets;
        private PXSqlValuepool mValuePool;

        #endregion

        #region properties
        public bool IsContentVariable
        {
            get { return mIsContentVariable; }
            set { mIsContentVariable = value; }
        }
        public bool IsTimevariable
        {
            get { return mIsTimeVariable; }
            set { mIsTimeVariable = value; }
        }
        public bool IsClassificationVariable
        {
            get { return mIsClassificationVariable; }
            set { mIsClassificationVariable = value; }
        }
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        public bool isSelected
        {
            get { return mIsSelected; }
            set { mIsSelected = value; }
        }
        public bool IsEliminatedByValue
        {
            get { return mIsEliminatedByValue; }
            set { mIsEliminatedByValue = value; }
        }
        public string VariableType
        {
            get { return mVariableType; }
            set { mVariableType = value; }
        }

        public Dictionary<string, string> PresText
        {
            get { return mPresText; }
            set { mPresText = value; }
        }
        public bool WildCardsUsed
        {
            get { return mIsWildCardsUsed; }
            set { mIsWildCardsUsed = value; }
        }
        public int Index
        {
            get { return mIndex; }
            set { mIndex = value; }
        }
        public bool DoubleColumn
        {
            get { return mDoubleColumn; }
            set { mDoubleColumn = value; }
        }
        public bool IsStub
        {
            get { return mIsStub; }
            set { mIsStub = value; }
        }
        public bool IsHeading
        {
            get { return mIsHeading; }
            set { mIsHeading = value; }
        }
        public string PresTextOption
        {
            get { return mPresTextOption; }
            set { mPresTextOption = value; }
        }
        public PxSqlValues Values
        {
            get { return mValues; }
            set { mValues = value; }
        }
        public int NumberOfPxsValues
        {
            get { return mNumberOfPxsValues; }
            set { mNumberOfPxsValues = value; }
        }
        public int TotalNumberOfValuesInDB
        {
            get { return mTotalNumberOfValuesInDB; }
            set { mTotalNumberOfValuesInDB = value; }
        }
        public string TempTableName
        {
            get { return mTempTableName; }
            set { mTempTableName = value; }
        }
        public string TempTableNo
        {
            get { return mTempTableNo; }
            set { mTempTableNo = value; }
        }

        public bool VarNoteExist
        {
            get { return mVarNoteExist; }
            set { mVarNoteExist = value; }
        }
        public bool ValueNoteExist
        {
            get { return mValueNoteExist; }
            set { mValueNoteExist = value; }
        }
        public bool UsesGrouping
        {
            get { return mUsesGrouping; }
            set { mUsesGrouping = value; }
        }


        //
        public int StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }
        public string PaxiomElimination
        {
            get { return mPaxiomElimination; }
            set { mPaxiomElimination = value; }
        }
        public string PaxiomMap
        {
            get { return mPaxiomMap; }
            set { mPaxiomMap = value; }
        }
        public PXSqlValueSet ValueSet
        {
            get { return mValueSet; }
            set { mValueSet = value; }
        }

        public Dictionary<string, PXSqlValueSet> ValueSets
        {
            get { return mValueSets; }
            set { mValueSets = value; }
        }

        public PXSqlValuepool ValuePool
        {
            get { return mValuePool; }
            set { mValuePool = value; 
            if (mValuePool == null)
            {
                mValueTextOption = PXConstant.VALUETEXTOPTION_NORMAL;
            } else {
                mValueTextOption = mValuePool.ValueTextOption;
            }
            }
        }

        #endregion

        #region setters
        //move setting of properties from pxsqlmeta to classes derived from this base class.
        //protected bool SetEliminatedByValue
        //{
        //    set { mIsEliminatedByValue = value; }
        //}
        #endregion
        #region constructors
        public PXSqlVariable() { }


        public PXSqlVariable(string name, PXSqlMeta_21 meta, bool isContVar, bool isTimeVar, bool isClassVar)
        {

            this.Name = name;
            this.IsTimevariable = isTimeVar;
            this.IsContentVariable = isContVar;
            this.IsClassificationVariable = isClassVar;
            this.IsStub = false;
            this.IsHeading = false;
            this.Values = new PxSqlValues();
            this.PresText = new Dictionary<string, string>();
            this.meta = meta;
            this.metaQuery = meta.MetaQuery;
            // should be moved to classvar
            this.IsEliminatedByValue = false;
            this.selectedValueset = PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS;



        }

        #endregion constructors
    


        protected virtual void SetPresText()
        {
            VariableRow mVariableRow = meta.MetaQuery.GetVariableRow(this.Name);
            foreach (string langCode in meta.LanguageCodes)
            {
                this.PresText[langCode] = mVariableRow.texts[langCode].PresText;
            }
            if (mVariableRow.Footnote != meta.Config.Codes.FootnoteN)
            {
                this.VarNoteExist = true;
            }
            else
                this.VarNoteExist = false;
        }
        protected virtual void SetElimForPresentation()
        {

            this.IsEliminatedByValue = true;
            this.PaxiomElimination = PXConstant.NO;
        }

        #region CompareTo
        public int CompareTo(object obj)
        {
            //if (this.GetType() != obj.GetType())
            //{
            //    throw new PCAxis.Sql.Exceptions.BugException(10000);
            //}
            //else
            {
                PXSqlVariable SqlVariableCompare = (PXSqlVariable)obj;
                return this.Index.CompareTo(SqlVariableCompare.Index);   //will not compare time-,classification and contens
                // return ((PXSqlVariable)this).Index.CompareTo(SqlVariableCompare.Index);
                /*if (this.Index == sqlVariableCompare.Index)
                {
                    return(0);
                }
                else if(this.Index > sqlVariableCompare.Index)
                {
                    return(1);
                }
                else if (this.Index < sqlVariableCompare.Index)
                {
                    return(-1);
                }
                else 
                {
                    return(-1);
                }
                */
            }

        }
        #endregion


        /// <PXKeyword name="VARIABLENAME (used in local SetMeta) ">
        ///   <rule>
        ///     <description>Sets the Paxiom-variable-name.</description>
        ///     <table modelName ="Variable">
        ///     <column modelName="PresText"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        internal virtual void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage)
        {
            if (this.isSelected)
            {
                StringCollection values = new StringCollection();
                string subkey = this.Name;
                

                //VARIABLENAME  //OBS located in PXSQLBuilder near SetVariableName

                foreach (string langCode in LanguageCodes)
                {
                    values.Clear();
                    values.Add(this.PresText[langCode]);
                    handler("VARIABLENAME", langCode, subkey, values);
                }

                // PRESTEXT
                ParsePresTextOption(handler, LanguageCodes, preferredLanguage);
                
                // VALUE_TEXT_OPTION
                ParseValueTextOption(handler);



                // denne ble kjørt uavhenging av isSelected da den lå i ParseMeta.cs
                //keyword = "VALUES" and CODES
                ParseCodeAndValues(handler, LanguageCodes, preferredLanguage);
            }
        }

        /// <PXKeyword name="VALUE_TEXT_OPTION">
        ///   <rule>
        ///     <description>Is "normal" except for those classification variables which has a ValuePool where ValueTextExists is coded as no text or loooong text.</description>
        ///     <table modelName ="ValuePool">
        ///     <column modelName="ValueTextExists"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        private void ParseValueTextOption(PCAxis.Paxiom.IPXModelParser.MetaHandler handler)
        {
            StringCollection values = new StringCollection();
            string subkey = this.Name;
            string noLanguage = null;
            values.Add(this.mValueTextOption);
            handler(PXKeywords.VALUE_TEXT_OPTION, noLanguage, subkey, values);
        }


        /// <PXKeyword name="PRESTEXT">
        ///   <rule>
        ///     <description>For Contents and Time value is read from Config.Codes.ValuePresT (T for Long Value Text, not time :-) and ValuePresC (C for Codes). </description>
        ///     <table modelName ="ValueSet">
        ///       <column modelName="ValuePres"/>
        ///     </table>
        ///     <table modelName ="ValuePool">
        ///       <column modelName="ValuePres"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        internal virtual void ParsePresTextOption(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage)
        {
            // PRESTEXT
            StringCollection values = new StringCollection();
            string subkey = this.Name;
            string noLanguage = null;
            values.Clear();
            values.Add(PresTextOptionToPxiomPresText(this.PresTextOption));
            handler(PXKeywords.PRESTEXT, noLanguage, subkey, values);
        }



        /// <PXKeyword name="VALUES">
        ///   <rule>
        ///     <description> In the normal case the Value table is used. ValueExtra is for very long texts. If the valuepool has only code, VALUES is not sent.</description>
        ///     <table modelName ="Value">
        ///     <column modelName="ValueTextS"/>
        ///     <column modelName="ValueTextL"/>
        ///     </table>
        ///     <table modelName ="ValueExtra">
        ///     <column modelName="ValueTextX1"/>
        ///     <column modelName="ValueTextX2"/>
        ///     <column modelName="ValueTextX3"/>
        ///     <column modelName="ValueTextX4"/>
        ///     </table>
        ///    <table modelName ="ContentsTime">
        ///     <column modelName="TimePeriod"/>
        ///     </table>
        ///   
        ///   <table modelName ="Content">
        ///     <column modelName="PresText"/>
        ///     </table>
        ///     </rule>
        /// </PXKeyword>
        /// <PXKeyword name="CODES">
        ///   <rule>
        ///     <description> </description>
        ///     <table modelName ="Value">
        ///     <column modelName="ValueCode"/>
        ///     </table>
        ///    <table modelName ="ContentsTime">
        ///     <column modelName="TimePeriod"/>
        ///     </table>
        ///     <table modelName ="Content">
        ///     <column modelName="PresCode"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        internal virtual void ParseCodeAndValues(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage)
        {
            string subkey = this.Name;
            string noLanguage = null;
            StringCollection values = new StringCollection();
            //VALUES and Codes
            List<PXSqlValue> _values = this.GetValuesForParsing();
            //VALUES
            if (! this.mValueTextOption.Equals(PXConstant.VALUETEXTOPTION_NOTEXT))
            {
                //            if ( this.PresTextOption.Equals(this.meta.Config.Codes.ValuePresC))
                foreach (string langCode in LanguageCodes)
                {
                    values.Clear();
                    int i = 0;
                    foreach (PXSqlValue val in _values)
                    {
                        i++;
                        if (this.IsContentVariable)
                        {
                            if (val.ValueTextS[langCode].Length > 0)
                            {
                                values.Add(val.ValueTextS[langCode]);
                            }
                            else
                            {
                                values.Add(val.ValueTextL[langCode]);
                            }
                        }
                        else if (this.IsTimevariable)
                        {
                            if (val.ValueTextL[langCode].Length > 0)
                            {
                                values.Add(val.ValueTextL[langCode]);
                            }
                            else
                            {
                                throw new ApplicationException("No text for timevariable value");
                            }
                        }
                        else
                        {
                            if (this.ValuePool.ValuePres.Equals(meta.Config.Codes.ValuePresB) || this.ValuePool.ValuePres.Equals(meta.Config.Codes.ValuePresT))
                            {
                                if (val.ValueTextL[langCode].Length>0)
                                {
                                    values.Add(val.ValueTextL[langCode]);
                                }
                                else
                                {
                                    throw new ApplicationException("Long valuetext should be used according to Valuepool.Valuepres, but long text doesn't exist. ValuePool=" +  val.ValuePool + " Valuecode= " + val.ValueCode);
                                }
                            }
                            else if (this.ValuePool.ValuePres.Equals(meta.Config.Codes.ValuePresA) || this.ValuePool.ValuePres.Equals(meta.Config.Codes.ValuePresS))
                            {
                                if (val.ValueTextS[langCode].Length > 0)
                                {
                                    values.Add(val.ValueTextS[langCode]);
                                }
                                else
                                {
                                    throw new ApplicationException("Short valuetext should be used according to Valuepool.Valuepres, but short text doesn't exist. ValuePool=" + val.ValuePool + " Valuecode= " + val.ValueCode);
                                }
                            }
                            else 
                            {
                                // ValuePres = C
                                if (val.ValueTextL[langCode].Length > 0)
                                {
                                    values.Add(val.ValueTextL[langCode]);
                                }
                                else
                                {
                                    if (val.ValueTextS[langCode].Length > 0)
                                    {
                                        values.Add(val.ValueTextS[langCode]);
                                    }
                                }
                            }
                        }
                    }
                  
                    handler(PXKeywords.VALUES, langCode, subkey, values);
                }
            }

            //CODES
            values.Clear();
            foreach (PXSqlValue val in _values)
            {
                values.Add(val.ValueCode);
            }

            handler(PXKeywords.CODES, noLanguage, subkey, values);
            values = null;


        }

        // end New





        //PXSqlParserForCodelists codelistParser = new PXSqlParserForCodelists(meta, mName);
        //codelistParser.ParseMeta(handler, preferredLanguage);
        //codelistParser.Dispose();



        internal abstract List<PXSqlValue> GetValuesForParsing();

        private string PresTextOptionToPxiomPresText(string presText)
        {
            if (presText == meta.Config.Codes.ValuePresC)
                return "0";

            if (presText == meta.Config.Codes.ValuePresT)
                return "1";

            if (presText == meta.Config.Codes.ValuePresB)
                return "2";
            else
                return "2";
        }
        protected string ConvertToDbPresTextOption(string pxsPresTextOption)
        {
            switch (pxsPresTextOption)
            {
                case "Text":
                    return meta.Config.Codes.ValuePresT;
                case "Code":
                    return meta.Config.Codes.ValuePresC;
                case "Both":
                    return meta.Config.Codes.ValuePresB;
                default:
                    return meta.Config.Codes.ValuePresB;
            }
        }
    }

}
