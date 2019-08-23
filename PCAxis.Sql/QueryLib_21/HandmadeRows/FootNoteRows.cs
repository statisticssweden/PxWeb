using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using PCAxis.Sql.DbClient; //For executing SQLs.
using PCAxis.Sql.DbConfig; // ReadSqlDbConfig;
//using PCAxis.Sql.QueryLib;

using log4net;

namespace PCAxis.Sql.QueryLib_21

{
    public enum PXSqlNoteType
    {
        Contents = 2,
        ContentsVbl = 3,
        ContentsValue = 4,
        ContentsTime = 41,
        Variable = 5,
        Value = 6,
        MainTable = 7,
        SubTable = 8,
        MainTableValue = 9,
        CellNote = 999
    }
    public class RelevantFootNotesRow
    {
        private string mMainTable;
        public string MainTable
        {
            get { return mMainTable; }
        }
        private string mFootNoteNo;
        public string FootNoteNo
        {
            get { return mFootNoteNo; }
        }
        //private string mFootNoteType;
        //public string FootNoteType
        //{
        //    get { return mFootNoteType; }
        //}
        private PXSqlNoteType mFootNoteType;
        public PXSqlNoteType FootNoteType
        {
            get { return mFootNoteType; }
        }
        private string mContents;
        public string Contents
        {
            get { return mContents; }
        }

        private string mVariable;
        public string Variable
        {
            get { return mVariable; }
        }
        private string mValuePool;
        public string ValuePool
        {
            get { return mValuePool; }
        }
        private string mValueCode;
        public string ValueCode
        {
            get { return mValueCode; }
        }
        private string mTimePeriod;
        public string TimePeriod
        {
            get { return mTimePeriod; }
        }
        private string mSubTable;
        public string SubTable
        {
            get { return mSubTable; }
        }
        private string mMandOpt;
        public string MandOpt
        {
            get { return mMandOpt; }
        }
        private string mShowFootNote;
        public string ShowFootNote
        {
            get { return mShowFootNote; }
        }
        public Dictionary<string, RelevantFoonotesTexts> texts = new Dictionary<string, RelevantFoonotesTexts>();
        public RelevantFootNotesRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, string metaModel)
        {
            this.mMainTable = myRow["MainTable"].ToString();
            this.mFootNoteNo = myRow["FootNoteNo"].ToString();
            //this.mFootNoteType = myRow["FootNoteType"].ToString();
            this.mFootNoteType = (PXSqlNoteType)Enum.Parse(typeof(PXSqlNoteType), myRow["FootNoteType"].ToString());
            this.mContents = myRow["Contents"].ToString();
            this.mVariable = myRow["Variable"].ToString();
            this.mValuePool = myRow["ValuePool"].ToString();
            this.mValueCode = myRow["ValueCode"].ToString();
            this.mTimePeriod = myRow["TimePeriod"].ToString();
            this.mSubTable = myRow["SubTable"].ToString();
            this.mMandOpt = myRow["MandOpt"].ToString();
            this.mShowFootNote = myRow["ShowFootNotes"].ToString();
            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new RelevantFoonotesTexts(myRow, dbconf, languageCode, metaModel));
            }
        }
    }
    public class RelevantFoonotesTexts
    {
        private String mFootNotetext;
        public String FootNoteText
        {
            get { return mFootNotetext; }
        }
        public RelevantFoonotesTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, string metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mFootNotetext = myRow[dbconf.FootnoteLang2.Alias + dbconf.GetMetaSuffix(languageCode) + "_" + dbconf.FootnoteLang2.FootnoteText].ToString();
            }
            else
            {
                this.mFootNotetext = myRow[dbconf.Footnote.Alias + "_" + dbconf.Footnote.FootnoteText].ToString();
            }
        }
    }
}
