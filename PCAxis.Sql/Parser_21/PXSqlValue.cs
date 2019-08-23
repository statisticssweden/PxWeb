using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_21;
using System.Collections.Specialized;
using log4net;
namespace PCAxis.Sql.Parser_21
{
    public class SortByVsValueHelper : IComparer<PXSqlValue>
    {
        int IComparer<PXSqlValue>.Compare(PXSqlValue a, PXSqlValue b)
        {
            PXSqlValue valA = a;
            PXSqlValue valB = b;
            return String.Compare(valA.SortCodeVsValue,valB.SortCodeVsValue);
        }
    }
    public class SortByValueHelper : IComparer<PXSqlValue>
    {
        int IComparer<PXSqlValue>.Compare(PXSqlValue a, PXSqlValue b)
        {
            PXSqlValue valA = a;
            PXSqlValue valB = b;
            return String.Compare(valA.SortCodeValue, valB.SortCodeValue);
        }
    }
    public class SortByPxsHelper : IComparer<PXSqlValue>
    {
        int IComparer<PXSqlValue>.Compare(PXSqlValue a, PXSqlValue b)
        {
            PXSqlValue valA = a;
            PXSqlValue valB = b;
            if (valA.SortCodePxs > valB.SortCodePxs)
                return 1;
            if (valA.SortCodePxs < valB.SortCodePxs)
                return -1;
            else
                return 0;
        }
    }
    public class PXSqlValue:IComparable
    {
        #region members
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlValue));

        private string mContentsCode;
        private string mValueCode;
        private string mValuePool;
        private string mValueSet;
        private int mSortCodePxs;
        private string mSortCodeValue;
        private string mSortCodeVsValue;
        private string mSortCodeDb; // this is the sortorder returned from dababase sort on vsvalue,value
        private Dictionary<string,string> mValueTextS =  new Dictionary<string,string>();
        private Dictionary<string,string> mValueTextL = new Dictionary<string,string>();
        private string mPrecision;
        private bool mIsSelected;
        private bool mExistInDb;


        #endregion
        public string ContentsCode
        {
            get { return mContentsCode; }
            set { mContentsCode = value; }
        }

        public string ValueCode
        {
            get { return mValueCode; }
            set { mValueCode = value;}
        }
        public string ValuePool
        {
            get { return mValuePool; }
            set { mValuePool = value; }
        }
        public string ValueSet
        {   
            get { return mValueSet; }
            set { mValueSet = value; }  
        }
        public int SortCodePxs
        {
            get { return mSortCodePxs; }
            set { mSortCodePxs = value; }
        }

        public string SortCodeValue
        {
            get { return mSortCodeValue; }
            //set { mSortCodeValue = value; }
        }
        public string SortCodeVsValue
        {
            get { return mSortCodeVsValue; }
            //set { mSortCodeVsValue = value; }
        }
        public string SortCodeDb
        {
            get { return mSortCodeDb; }
            set { mSortCodeDb = value; }
        }
        public Dictionary<string,string> ValueTextS
        {
            get { return mValueTextS; }
            set { mValueTextS = value; }
        }
        public Dictionary<string, string> ValueTextL
        {
            get { return mValueTextL; }
            set { mValueTextL = value; }
        }
        public string Precision
        {
            get { return mPrecision; }
            set { mPrecision = value; }
        }
        public bool IsSelected
        {
            get { return mIsSelected; }
            set { mIsSelected = value; }
        }
        public bool ExistInDb
        {
        get { return mExistInDb; }
        set { mExistInDb = value; }
        }
        //private List<FootnoteRow> mFootNoteRows;
        //public List<FootnoteRow> FootNoteRows
        //{
        //    get { return mFootNoteRows; }
        //    set { mFootNoteRows = value; }
        //}

        //Not used
        //private List<RelevantFootNotesRow> mFootNotesValue;
        //public List<RelevantFootNotesRow> FootNotesValue
        //{
        //    get { return mFootNotesValue; }
        //    set { mFootNotesValue = value; }
        //}


        public PXSqlValue()
        {
        //    mFootNotesValue = new List<RelevantFootNotesRow>();
        }


        public PXSqlValue(ValueRow2 myValueRow, StringCollection LanguageCodes, string MainLanguageCode) {
            

            //this.meta = meta;
            this.mContentsCode = null; // only for contentsvalues
            this.ValueCode = myValueRow.ValueCode;
            this.ValueSet = myValueRow.ValueSet;
            this.ValuePool = myValueRow.ValuePool;
            this.SortCodePxs = 0;  // is sometimes overridden from outside


            this.mSortCodeValue = myValueRow.texts[MainLanguageCode].SortCodeValue;
            this.mSortCodeVsValue = myValueRow.texts[MainLanguageCode].SortCodeVsValue;
            //this.SecondarySortOrder = myValueRow.ValueSortCode;
            this.SortCodeDb = myValueRow.SortOrder;
            foreach (string langCode in LanguageCodes) {
                this.ValueTextS[langCode] = myValueRow.texts[langCode].ValueTextS;
                this.ValueTextL[langCode] = myValueRow.texts[langCode].ValueTextL;
            }
        }

        /// <summary>
        /// Constructor to receive ValueRow. Used for receiving values for groups, should be used to receive all classification values in the future.
        /// </summary>
        /// <param name="myValueRow"></param>
        /// <param name="LanguageCodes"></param>
        /// <param name="MainLanguageCode"></param>
        public PXSqlValue(ValueRow myValueRow, StringCollection LanguageCodes, string MainLanguageCode)
        {


            //this.meta = meta;
            this.mContentsCode = null; // only for contentsvalues
            this.ValueCode = myValueRow.ValueCode;
            this.ValueSet = null;
            this.ValuePool = myValueRow.ValuePool;
            this.SortCodePxs = 0;  // is sometimes overridden from outside


            this.mSortCodeValue = myValueRow.texts[MainLanguageCode].SortCode;
            this.mSortCodeVsValue = null;
            //this.SortCodeDb = myValueRow.SortOrder;
            foreach (string langCode in LanguageCodes)
            {
                this.ValueTextS[langCode] = myValueRow.texts[langCode].ValueTextS;
                this.ValueTextL[langCode] = myValueRow.texts[langCode].ValueTextL;
            }
        }



        /// <summary>
        /// For Time Values
        /// </summary>
        /// <param name="timeCode">Used as ValueCode, ValueTextS and ValueTextL</param>
        /// <param name="LanguageCodes"></param>
        public PXSqlValue(string timeCode,  StringCollection LanguageCodes)
            {

            //this.meta = meta;
                this.mContentsCode = null; // only for contentsvalues
            this.ValueCode = timeCode;
            this.SortCodePxs = 0;  // is sometimes overridden from outside
            this.mSortCodeValue = timeCode;  
            this.mSortCodeVsValue = "0"; //hmmm
            this.SortCodeDb = "0"; //hmmm

            foreach (string langCode in LanguageCodes) {
                this.ValueTextS[langCode] = timeCode;
                this.ValueTextL[langCode] = timeCode;
            }
        }



        public PXSqlValue(PXSqlContent content, int sortCode)
        {
            this.mContentsCode = content.Contents;
            //this.ValueCode = content.Contents;
            this.ValueCode = content.PresCode; // ValueCode should be PresCode. Contents is no sent to Paxiom as Reference_ID an used as code in PXS.
            this.ValueTextL = content.PresText;
            this.ValueTextS = content.PresTextS;
            // SortCodePxs and SortCode are now set to the same value. Maybe no a good thing if sortorder should be reset to db
            // default after read from pxs. //TODO send two sortcodes from PXSQlMeta.   
            this.SortCodePxs = sortCode; 
            this.mSortCodeValue = sortCode.ToString(); 
           
        }




        public int CompareTo(object obj)
        {
            if (this.GetType() != obj.GetType())
            {
                throw new PCAxis.Sql.Exceptions.BugException(10000);    
            }
            else
            {
                PXSqlValue SqlValueCompare = (PXSqlValue)obj;
                int primaryComparison = this.SortCodePxs.CompareTo(SqlValueCompare.SortCodePxs);
                if (primaryComparison == 0)
                {
                    return this.SortCodeDb.CompareTo(SqlValueCompare.SortCodeDb);
                }
                else
                {
                    return primaryComparison;
                }
            }
        }
        public  static IComparer<PXSqlValue> SortByVsValue()
        {
            return (IComparer<PXSqlValue>)new SortByVsValueHelper();
        }
        public static IComparer<PXSqlValue> SortByValue()
        {
            return (IComparer<PXSqlValue>)new SortByValueHelper();
        }
        public static IComparer<PXSqlValue> SortByPxs()
        {
            return (IComparer<PXSqlValue>)new SortByPxsHelper();
        }
    }
}
