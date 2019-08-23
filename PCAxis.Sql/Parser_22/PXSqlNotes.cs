using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using PCAxis.Sql.QueryLib_22;

namespace PCAxis.Sql.Parser_22 {


    /// <PXKeyword name="NOTE(X), CELLNOTE(X) and VALUENOTE(X)">
    ///   <rule>
    ///     <description>X or not: The X in NOTEX, CELLNOTEX and VALUENOTEX is determined by the MandOpt column.</description>
    ///     <table modelName ="Footnote">
    ///     <column modelName="MandOpt"/>
    ///     </table>
    ///   </rule>
    ///   <rule>
    ///     <description>The texts for is read from the Footnote table.</description>
    ///     <table modelName ="Footnote">
    ///     <column modelName="FootnoteText"/>
    ///     </table>
    ///   </rule>  
    ///   <rule>
    ///     <description>Attaching a note to the cube, is done in the Footnote*-table ( FootnoteContTime,FootnoteContVbl ...). If a note should be sent as NOTE, CELLNOTE and VALUENOTE will, in general, depend on the selections made by the user at runtime.</description>
    ///     <table modelName ="Footnote*">
    ///     <column modelName="All"/>
    ///     </table>
    ///   </rule>   
    /// </PXKeyword>

    internal class PXSqlNotes
    {
        private PaxiomNotes mPaxiomNotes;
        public PaxiomNotes PaxiomNotes
        {
            get { return mPaxiomNotes; }
        }

        private PXSqlMenuSelNotes mMenuSelNotes;
        public PXSqlMenuSelNotes MenuSelNotes
        {
            get { return mMenuSelNotes; }
        }
        private PXSqlMainTablesValueNotes mMaintValueNotes;
        public PXSqlMainTablesValueNotes MaintValueNotes
        {
            get { return mMaintValueNotes; }
        }
        private PXSqlValueNotes mValueNotes;
        public PXSqlValueNotes ValueNotes
        {
            get { return mValueNotes; }
        }
        private PXSqlContValueNotes mContValueNotes;
        public PXSqlContValueNotes ContValueNotes
        {
            get { return mContValueNotes; }
        }
        private PXSqlContTimeNotes mContTimeNotes;
        public PXSqlContTimeNotes ContTimeNotes
        {
            get { return mContTimeNotes; }
        }
        private PXSqlContentsNotes mContentsNotes;
        public PXSqlContentsNotes ContentsNotes
        {
            get { return mContentsNotes; }
        }
        private PXSqlContVblNotes mContVblNotes;
        public PXSqlContVblNotes ContVblNotes
        {
            get { return mContVblNotes; }
        }
        private PXSqlVariableNotes mVariableNotes;
        public PXSqlVariableNotes VariableNotes
        {
            get { return mVariableNotes; }
        }
        private PXSqlCellNotes mCellNotes;
        public PXSqlCellNotes CellNotes
        {
            get { return mCellNotes; }
        }

        //
        List<RelevantFootNotesRow> allRelevantFoonotes;
        
        private StringCollection showFootnotesCodes;

        private PXSqlMeta_22 mMeta;
        private PXSqlVariables mVariables;
        private PXSqlSubTables mSubTables;
        private Dictionary<string, PXSqlContent> mContents;
        private PXSqlVariable mTimeVariable;
        private readonly string mMainTableId;

        internal PXSqlNotes(PXSqlMeta_22 mMeta, string mMainTableId, bool inPresentationModus)
        {
            this.mMeta = mMeta;
            this.mVariables = mMeta.Variables;

            this.mSubTables = mMeta.SubTables;
            this.mContents = mMeta.Contents;
            this.mTimeVariable = mMeta.TimeVariable;
            this.mMainTableId = mMainTableId;


            this.showFootnotesCodes = new StringCollection();
            if (inPresentationModus)
            {
                showFootnotesCodes.Add(mMeta.Config.Codes.FootnoteShowP);
            }
            else 
            {
                showFootnotesCodes.Add(mMeta.Config.Codes.FootnoteShowS);
            }
            showFootnotesCodes.Add(mMeta.Config.Codes.FootnoteShowB);

            allRelevantFoonotes = mMeta.MetaQuery.GetRelevantFoonotes(this.mMainTableId);
            //Adding footnotes for grouping 
            allRelevantFoonotes.AddRange(this.getRelevantGroupingNotes(inPresentationModus));

            

            this.SetFootNotes();


           
        }

        private List<RelevantFootNotesRow> getRelevantGroupingNotes(bool inPresentationModus)
        {
            List<RelevantFootNotesRow> myOut = new List<RelevantFootNotesRow>();
            foreach (PXSqlVariable var in mMeta.Variables.Values)
            {

                if (var.IsClassificationVariable)
                {
                    PXSqlVariableClassification clVar = (PXSqlVariableClassification)var;

                    StringCollection groupingids = new StringCollection();
                    if (inPresentationModus)
                    {
                        if (clVar.isSelected && !String.IsNullOrEmpty(clVar.CurrentGroupingId))
                        {
                            groupingids.Add(clVar.CurrentGroupingId);
                        }
                    }
                    else
                    {
                        groupingids = clVar.GroupingInfos.GroupingIDs;
                    }
                    if (groupingids.Count > 0)
                    {
                        foreach (FootnoteGroupingRow fgr in mMeta.MetaQuery.GetFootnoteGroupingRows(groupingids, true).Values)
                        {
                            myOut.Add(new RelevantFootNotesRow(clVar.Name, mMeta.MetaQuery.GetFootnoteRow(fgr.FootnoteNo), mMeta.Config, mMeta.LanguageCodes));

                        }
                    }
                }
            }
            return myOut;
        }



        

        private void SetMainTableNotes(RelevantFootNotesRow footNoteRow)
        {
            // MainTable allways maps to Table in paxiom 

            mPaxiomNotes.addTableNote(new PXSqlNote(footNoteRow, this.mMeta));
        }

        private void SetSubTableNotes(RelevantFootNotesRow footNoteRow)
        {
            // SubTable allways maps to Table in paxiom 
            if (mSubTables.ContainsKey(footNoteRow.SubTable))
            {
                if (mSubTables[footNoteRow.SubTable].IsSelected)
                {
                    mPaxiomNotes.addTableNote(new PXSqlNote(footNoteRow, this.mMeta));
                }

            }
        }

        private void SetVariableNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    PXSqlNote mNote;
                    mNote = new PXSqlNote(footNoteRow, this.mMeta);
                    mVariableNotes.Add(mNote);
                    //  mPXSqlVariables[footNoteRow.Variable].FootNotesVariable.Add(footNoteRow);
                }
            }
        }

        private void SetValueNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                    {
                        if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                        {
                            PXSqlNote mNote;
                            mNote = new PXSqlNote(footNoteRow, this.mMeta);
                            mValueNotes.Add(mNote);
                            //          mPXSqlVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].FootNotesValue.Add(footNoteRow);
                        }
                    }
                }
            }
        }
        private void SetMaintValueNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                    {
                        if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                        {
                            PXSqlNote mNote;
                            mNote = new PXSqlNote(footNoteRow, this.mMeta);
                            mMaintValueNotes.Add(mNote);
                            //            mPXSqlVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].FootNotesValue.Add(footNoteRow);
                        }
                    }
                }
            }
        }


        private void SetContentsNotes(RelevantFootNotesRow footNoteRow)
        {
            // Contents

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                PXSqlNote mNote;
                mNote = new PXSqlNote(footNoteRow, this.mMeta);
                mContentsNotes.Add(mNote);
                //  mPXSqlContents[footNoteRow.Contents].FootNotesContents.Add(footNoteRow);
            }
        }

        private void SetContVblNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {
                        PXSqlNote mNote;
                        mNote = new PXSqlNote(footNoteRow, this.mMeta);
                        mContVblNotes.Add(mNote);
                        //  mPXSqlContents[footNoteRow.Contents].FootNotesContVariable.Add(footNoteRow);
                    }
                }
            }
        }

        private void SetContValueNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable
            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {
                        if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                            if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                            {
                                PXSqlNote mNote;
                                mNote = new PXSqlNote(footNoteRow, this.mMeta);
                                mContValueNotes.Add(mNote);
                                //    mPXSqlContents[footNoteRow.Contents].FootNotesContValue.Add(footNoteRow);
                            }
                    }
                }
            }
        }

        private void SetContTimeNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mTimeVariable.Values.ContainsKey(footNoteRow.TimePeriod))
                {
                    PXSqlNote mNote;
                    mNote = new PXSqlNote(footNoteRow, this.mMeta);
                    mContTimeNotes.Add(mNote);
                    //  mPXSqlContents[footNoteRow.Contents].FootNotesContTime.Add(footNoteRow);
                }
            }
        }

        private void SetCellNotesNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable
            PXSqlNote mNote;
            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {
                        if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                        {
                            if (mTimeVariable.Values.ContainsKey(footNoteRow.TimePeriod))
                            {
                                mNote = new PXSqlNote(footNoteRow, this.mMeta);
                                mCellNotes.Add(mNote);
                            }

                        }
                    }


                }
            }
        }








        private void SetFootNotes()
        {

            /* the CNMM has many footnote types,  Paxiom has fewer.
             * "Upgrade" from "VALUE"  or from "Variable" to "Table" where possible*/
       
            
            mMenuSelNotes = new PXSqlMenuSelNotes();
            mMaintValueNotes = new PXSqlMainTablesValueNotes();
            mValueNotes = new PXSqlValueNotes();
            mContValueNotes = new PXSqlContValueNotes();
            mContTimeNotes = new PXSqlContTimeNotes();
            mContentsNotes = new PXSqlContentsNotes();
            mContVblNotes = new PXSqlContVblNotes();
            mVariableNotes = new PXSqlVariableNotes();
            mCellNotes = new PXSqlCellNotes();

            mPaxiomNotes = new PaxiomNotes();

            if (this.mMeta.MainTable.ContainsOnlyMetaData)
            {
                mPaxiomNotes.addTableNote(new PXSqlNote(this.mMeta));
            }

           

            foreach (RelevantFootNotesRow footNoteRow in allRelevantFoonotes)
            {
                if (!this.showFootnotesCodes.Contains(footNoteRow.ShowFootNote))
                {
                    continue;
                }
                // MainTableNotes
                switch (footNoteRow.FootNoteType)
                {
                    case PXSqlNoteType.MainTable:
                        SetMainTableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.SubTable:
                        SetSubTableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Contents:
                        SetContentsNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Variable:
                        SetVariableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Value:
                        SetValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.MainTableValue:
                        SetMaintValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsVbl:
                        SetContVblNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsValue:
                        SetContValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsTime:
                        SetContTimeNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.CellNote:
                        SetCellNotesNotes(footNoteRow);
                        break;

                }
            }


            //TODO; refactor: Make new class for mVariableNotes and mContentsNotes type data 
            #region mVariableNotes
            // rearranges the note according to notenumbe
            Dictionary<String, List<PXSqlNote>> varNotesByNoteNo = GetNotesByNoteNo(mVariableNotes);

            StringCollection outputVariables = this.mVariables.GetSelectedClassificationVarableIds();

            foreach (String noteno in varNotesByNoteNo.Keys)
            {
                StringCollection varsWhichHasANote = new StringCollection();
                foreach (PXSqlNote anote in varNotesByNoteNo[noteno])
                {
                    if (!varsWhichHasANote.Contains(anote.Variable))
                    {
                        varsWhichHasANote.Add(anote.Variable);
                    }
                }

                
                if (PCAxis.Sql.Parser.PXSqlMeta.containsAll(outputVariables, varsWhichHasANote))
                {
                    mPaxiomNotes.addTableNote(varNotesByNoteNo[noteno][0]);
                } else
                {
                    mPaxiomNotes.addVariableNotes(varNotesByNoteNo[noteno]);
                }

            }
            #endregion


            #region mContentsNotes
            // rearranges the note according to notenumbe
            Dictionary<String, List<PXSqlNote>> conentsNotesByNoteNo = GetNotesByNoteNo(mContentsNotes);


            StringCollection outputContentsValues = new StringCollection(); 

            foreach (String contentsValue in this.mMeta.Contents.Keys) {
                outputContentsValues.Add(contentsValue);
            }

            foreach (String noteno in conentsNotesByNoteNo.Keys)
            {
                StringCollection usedValues = new StringCollection();
                foreach (PXSqlNote anote in conentsNotesByNoteNo[noteno])
                {
                    if (!usedValues.Contains(anote.Contents))
                    {
                        usedValues.Add(anote.Contents);
                    }
                }




                if (PCAxis.Sql.Parser.PXSqlMeta.containsAll(outputContentsValues, usedValues))
                {
                    mPaxiomNotes.addTableNote(conentsNotesByNoteNo[noteno][0]);
                } else
                {
                    mPaxiomNotes.addContentsNotes(conentsNotesByNoteNo[noteno]);
                }

            }
            #endregion
            

        }

        private Dictionary<String, List<PXSqlNote>> GetNotesByNoteNo(List<PXSqlNote> longNoteList)
        {
            Dictionary<String, List<PXSqlNote>> NotesByNoteNo = new Dictionary<string, List<PXSqlNote>>();
            foreach (PXSqlNote note in longNoteList)
            {
                List<PXSqlNote> tmp;
                if (NotesByNoteNo.ContainsKey(note.FootNoteNo))
                {
                    tmp = NotesByNoteNo[note.FootNoteNo];
                }
                else
                {
                    tmp = new List<PXSqlNote>();
                }
                tmp.Add(note);
                NotesByNoteNo[note.FootNoteNo] = tmp;
            }
            return NotesByNoteNo;
        }

        internal void ParseAllNotes(PCAxis.Paxiom.IPXModelParser.MetaHandler handler)
        {
            //CellNote
            foreach (string langCode in mMeta.LanguageCodes)
            {
                this.CellNotes.ParseNote(mMeta, handler, langCode);
            }

            //MainTableNote + SubTableNote + VariableNotes + ContentsNotes
            foreach (string langCode in mMeta.LanguageCodes)
            {
                mMeta.TheNotes.PaxiomNotes.ParseNote(mMeta, handler, langCode);
            }

            //MaintValueNotes
            foreach (string langCode in mMeta.LanguageCodes)
            {
                mMeta.TheNotes.MaintValueNotes.ParseNote(mMeta, handler, langCode);
            }

            //ValueNotes
            foreach (string langCode in mMeta.LanguageCodes)
            {
                mMeta.TheNotes.ValueNotes.ParseNote(mMeta, handler, langCode);
            }
            //ContValueNotes
            foreach (string langCode in mMeta.LanguageCodes)
            {
                mMeta.TheNotes.ContValueNotes.ParseNote(mMeta, handler, langCode);
            }
            //ContTimeNotes
            foreach (string langCode in mMeta.LanguageCodes)
            {
                mMeta.TheNotes.ContTimeNotes.ParseNote(mMeta, handler, langCode);
            }

            //ContVblNotes
            foreach (string langCode in mMeta.LanguageCodes)
            {
                mMeta.TheNotes.ContVblNotes.ParseNote(mMeta, handler, langCode);
            }
            
        }
    }





    public class PXSqlNote {
        protected const string star = "*";
        protected string mShowFootNote;
        public string ShowFootNote {
            get { return this.mShowFootNote; }
        }
        protected string mMandOpt;
        public string MandOpt {
            get { return this.mMandOpt; }
        }
        protected string mFootNoteNo;
        public string FootNoteNo {
            get { return this.mFootNoteNo; }
        }
        protected string mContents;
        public string Contents {
            get { return this.mContents; }
        }
        protected string mVariable;
        public string Variable {
            get { return this.mVariable; }
        }
        protected string mValueCode;
        public string ValueCode {
            get { return this.mValueCode; }
        }
        protected string mTimePeriode;
        public string TimePeriode {
            get { return this.mTimePeriode; }
        }
        protected Dictionary<string, String> mNotePresTexts;
        public Dictionary<string, String> NotePresTexts {
            get { return this.mNotePresTexts; }
        }
        protected String mNotePresText;
        protected const string FootNoteTableContainsOnlyMetadata = "This table contains only metadata";
        protected const string FootNoteTableContainsOnlyMetadataDummyNumber = "-999"; 

        public PXSqlNote(RelevantFootNotesRow footNoteRow, PXSqlMeta_22 mMeta) {
            this.mShowFootNote = footNoteRow.ShowFootNote;
            this.mMandOpt = footNoteRow.MandOpt;
            this.mFootNoteNo = footNoteRow.FootNoteNo;
            this.mContents = footNoteRow.Contents;
            this.mVariable = footNoteRow.Variable;
            this.mValueCode = footNoteRow.ValueCode;
            this.mTimePeriode = footNoteRow.TimePeriod;
            this.mNotePresTexts = new Dictionary<string, String>();
            foreach (string langCode in mMeta.LanguageCodes)
            {
                this.mNotePresTexts.Add(langCode, footNoteRow.texts[langCode].FootNoteText);
            }
        }


        public PXSqlNote(PXSqlMeta_22 mMeta)
        {
            this.mShowFootNote = mMeta.Config.Codes.FootnoteShowB;
            this.mMandOpt = mMeta.Config.Codes.FootnoteM;
            this.mFootNoteNo = FootNoteTableContainsOnlyMetadataDummyNumber;
            this.mContents = "";
            this.mVariable = "";
            this.mValueCode = "";
            this.mTimePeriode = "";
            this.mNotePresTexts = new Dictionary<string, String>();
            foreach (string langCode in mMeta.LanguageCodes)
            {
                this.mNotePresTexts.Add(langCode, FootNoteTableContainsOnlyMetadata);
            }
        }
    }

    
    public abstract class PXSqlGeneralNotes : List<PXSqlNote> {
        public void ParseNote(PXSqlMeta_22 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
            string keyWord;
            string subKeyWord;
            StringCollection parseValue;
            foreach (PXSqlNote note in this) {
                if ((note.ShowFootNote == mMeta.Config.Codes.FootnoteShowP) || (note.ShowFootNote == mMeta.Config.Codes.FootnoteShowB)) {
                    if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                        keyWord = "VALUENOTEX";
                    else
                        keyWord = "VALUENOTE";
                    subKeyWord =  note.Variable + "\",\"" + note.ValueCode;
                    parseValue = new StringCollection();
                    parseValue.Add(note.NotePresTexts[langCode]);
                    handler(keyWord, langCode, subKeyWord, parseValue);
                }
            }
        }
    }



    public class PXSqlValueNotes : PXSqlGeneralNotes {}

    public class PXSqlMainTablesValueNotes : PXSqlGeneralNotes {}

    public class PXSqlMenuSelNotes : List<PXSqlNote> { }

    public class PXSqlContValueNotes : List<PXSqlNote> {
        private const string star = "*";
        public void ParseNote(PXSqlMeta_22 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
            string keyWord;
            string subKeyWord;
            StringCollection parseValue;
            if (mMeta.Contents.Count == 1) {
                foreach (PXSqlNote note in this) {
                    if ((note.ShowFootNote == mMeta.Config.Codes.FootnoteShowP) || (note.ShowFootNote == mMeta.Config.Codes.FootnoteShowB)) {
                        if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                            keyWord = "VALUENOTEX";
                        else
                            keyWord = "VALUENOTE";
                        subKeyWord =   note.Variable + "\",\"" + note.ValueCode ;
                        parseValue = new StringCollection();
                        parseValue.Add(note.NotePresTexts[langCode]);
                        handler(keyWord, langCode, subKeyWord, parseValue);
                    }

                }
            }
            else {
                List<PXSqlVariable> stubAndHead = new List<PXSqlVariable>();
                stubAndHead.AddRange(mMeta.Stubs);
                stubAndHead.AddRange(mMeta.Headings);
                StringCollection CellNoteDimensions;
                foreach (PXSqlNote note in this) {
                    if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                        keyWord = "CELLNOTEX";
                    else
                        keyWord = "CELLNOTE";
                    CellNoteDimensions = new StringCollection();
                    foreach (PXSqlVariable var in stubAndHead) {
                        if (var.IsContentVariable) {
                            if (var.Values.GetValueByContentsCode(note.Contents) !=  null)
                            {
                                CellNoteDimensions.Add(var.Values.GetValueByContentsCode(note.Contents).ValueCode);
                            }
                            //if (var.Values.ContainsKey(note.Contents))
                            //    CellNoteDimensions.Add(note.Contents);
                            else
                                throw new PCAxis.Sql.Exceptions.DbException(12);
                        }
                        else if (var.Name == note.Variable) {
                            if (var.Values.ContainsKey(note.ValueCode))
                                CellNoteDimensions.Add(note.ValueCode);
                            else
                                throw new PCAxis.Sql.Exceptions.DbException(12);

                        }
                        else
                            CellNoteDimensions.Add(star);
                    }
                    subKeyWord = "";
                    parseValue = new StringCollection();
                    foreach (string dim in CellNoteDimensions)
                        subKeyWord += "\"" + dim + "\",";
                    subKeyWord = subKeyWord.TrimStart('"');
                    subKeyWord = subKeyWord.TrimEnd(',', '"');
                    parseValue.Add(note.NotePresTexts[langCode]);
                    handler(keyWord, langCode, subKeyWord, parseValue);
                }
            }
        }
    }
    
    public class PXSqlContTimeNotes : List<PXSqlNote> {
        private const string star = "*";
        public void ParseNote(PXSqlMeta_22 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
            string keyWord;
            string subKeyWord;
            StringCollection parseValue;
            if (mMeta.Contents.Count == 1) {
                foreach (PXSqlNote note in this) {
                    if ((note.ShowFootNote == mMeta.Config.Codes.FootnoteShowP) || (note.ShowFootNote == mMeta.Config.Codes.FootnoteShowB)) {
                        if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                            keyWord = "VALUENOTEX";
                        else
                            keyWord = "VALUENOTE";
                        subKeyWord = mMeta.TimeVariable.Name + "\",\"" + note.TimePeriode;
                        parseValue = new StringCollection();
                        parseValue.Add(note.NotePresTexts[langCode]);
                        handler(keyWord, langCode, subKeyWord, parseValue);
                    }

                }
            }
            else {
                List<PXSqlVariable> stubAndHead = new List<PXSqlVariable>();
                stubAndHead.AddRange(mMeta.Stubs);
                stubAndHead.AddRange(mMeta.Headings);
                StringCollection CellNoteDimensions;
                foreach (PXSqlNote note in this) {
                    if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                        keyWord = "CELLNOTEX";
                    else
                        keyWord = "CELLNOTE";
                    CellNoteDimensions = new StringCollection();
                    foreach (PXSqlVariable var in stubAndHead) {
                        if (var.IsTimevariable) {
                            if (var.Values.ContainsKey(note.TimePeriode))
                                CellNoteDimensions.Add(note.TimePeriode);
                            else
                                throw new PCAxis.Sql.Exceptions.DbException(12);

                        }
                        else if (var.IsContentVariable) {
                            if (var.Values.GetValueByContentsCode(note.Contents) !=  null)
                            {
                                CellNoteDimensions.Add(var.Values.GetValueByContentsCode(note.Contents).ValueCode);
                            }
                            //if (var.Values.ContainsKey(note.Contents))
                            //    CellNoteDimensions.Add(note.Contents);
                            else
                                throw new PCAxis.Sql.Exceptions.DbException(13);

                        }
                        else
                            CellNoteDimensions.Add(star);
                    }
                    subKeyWord = "";
                    parseValue = new StringCollection();
                    foreach (string dim in CellNoteDimensions)
                        subKeyWord += "\"" + dim + "\",";
                    subKeyWord = subKeyWord.TrimStart('"');
                    subKeyWord = subKeyWord.TrimEnd(',', '"');
                    parseValue.Add(note.NotePresTexts[langCode]);
                    handler(keyWord, langCode, subKeyWord, parseValue);
                }
            }
        }
    }
    
    public class PXSqlContentsNotes : List<PXSqlNote> {
        
    }

    public class PXSqlContVblNotes : List<PXSqlNote> {
        public void ParseNote(PXSqlMeta_22 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
            string keyWord;
            string subKeyWord = null;
            StringCollection parseValue;
            foreach (PXSqlNote note in this) {
                if (mMeta.Contents.Count == 1) {
                    if ((note.ShowFootNote == mMeta.Config.Codes.FootnoteShowP) || (note.ShowFootNote == mMeta.Config.Codes.FootnoteShowB)) {
                        if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                            keyWord = "NOTEX";
                        else
                            keyWord = "NOTE";
                        subKeyWord = note.Variable;
                        parseValue = new StringCollection();
                        parseValue.Add(note.NotePresTexts[langCode]);
                        handler(keyWord, langCode, subKeyWord, parseValue);
                    }
                }
                // if more than one contentsvariable is selected the note should be presented different, but there are
                // no way to parse it to PAXIOm and specify that the note is valid for only one contents.
                else {
                    if ((note.ShowFootNote == mMeta.Config.Codes.FootnoteShowP) || (note.ShowFootNote == mMeta.Config.Codes.FootnoteShowB)) {
                        if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                            keyWord = "NOTEX";
                        else
                            keyWord = "NOTE";
                        subKeyWord = note.Variable;
                        parseValue = new StringCollection();
                        parseValue.Add(note.NotePresTexts[langCode]);
                        handler(keyWord, langCode, subKeyWord, parseValue);
                    }
                }

            }
        }
    }
    
    public class PXSqlVariableNotes : List<PXSqlNote> {
       
    }


    public class PaxiomNotes
    {

        private Dictionary<String, PXSqlNote> tableNotes = new Dictionary<string, PXSqlNote>();

        private List<PXSqlNote> variableNotes = new List<PXSqlNote>();

        private List<PXSqlNote> contentsNotes = new List<PXSqlNote>();

        //adds tableNote unless it is already added
        public void addTableNote(PXSqlNote tableNote) {
            if ( ! tableNotes.ContainsKey(tableNote.FootNoteNo)) {
                tableNotes.Add(tableNote.FootNoteNo, tableNote);
            }
        }

        //adds each tableNote in the list unless the note is already added
        public void addTableNote(List<PXSqlNote> tableNotes)
        {
            foreach (PXSqlNote note in tableNotes)
            {
                this.addTableNote(note);
            }
        }

        public void addVariableNotes(List<PXSqlNote> variableNotes)
        {
            this.variableNotes.AddRange(variableNotes);
        }

        public void addContentsNotes(List<PXSqlNote> contentsNotes)
        {
            this.contentsNotes.AddRange(contentsNotes);
        }



        public void ParseNote(PXSqlMeta_22 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode)
        {
            string keyWord;
            string subKeyWord = null;
            StringCollection parseValue;

            foreach (PXSqlNote note in this.tableNotes.Values)
            {
                    if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                        keyWord = "NOTEX";
                    else
                        keyWord = "NOTE";
                    parseValue = new StringCollection();
                    parseValue.Add(note.NotePresTexts[langCode]);
                    handler(keyWord, langCode, subKeyWord, parseValue);   
            }



            foreach (PXSqlNote note in this.variableNotes)
            {    
                    if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                        keyWord = "NOTEX";
                    else
                        keyWord = "NOTE";
                    subKeyWord = note.Variable;
                    parseValue = new StringCollection();
                    parseValue.Add(note.NotePresTexts[langCode]);
                    handler(keyWord, langCode, subKeyWord, parseValue);
                
            }



            foreach (PXSqlNote note in this.contentsNotes)
            {
               
                    subKeyWord = mMeta.ContensCode + "\",\"" + mMeta.ContentsVariable.Values.GetValueByContentsCode(note.Contents).ValueCode;
                    //subKeyWord = mMeta.ContensCode + "," + note.Contents;
                    if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                        keyWord = "VALUENOTEX";
                    else
                        keyWord = "VALUENOTE";

                    parseValue = new StringCollection();
                    parseValue.Add(note.NotePresTexts[langCode]);
                    handler(keyWord, langCode, subKeyWord, parseValue);
                
            }

        }

    }


    public class PXSqlCellNotes : List<PXSqlNote> {
        private const string star = "*";
        public void ParseNote(PXSqlMeta_22 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
            List<PXSqlVariable> stubAndHead = new List<PXSqlVariable>();
            stubAndHead.AddRange(mMeta.Stubs);
            stubAndHead.AddRange(mMeta.Headings);
            StringCollection CellNoteDimensions;
            string subKeyWord;
            string keyWord; 
            StringCollection parseValue;
            foreach (PXSqlNote note in this) {
                if (note.MandOpt == mMeta.Config.Codes.FootnoteM)
                    keyWord = "CELLNOTEX";
                else
                    keyWord = "CELLNOTE";
                CellNoteDimensions = new StringCollection();
                foreach (PXSqlVariable var in stubAndHead) {
                    if (var.IsTimevariable) {
                        if (var.Values.ContainsKey(note.TimePeriode))
                            CellNoteDimensions.Add(note.TimePeriode);
                        else
                            CellNoteDimensions.Add(star);
                    }
                    else if (var.IsContentVariable) {
                        if (var.Values.ContainsKey(note.Contents))
                            CellNoteDimensions.Add(note.Contents);
                        else
                            CellNoteDimensions.Add(star);
                    }
                    else if (var.Name == note.Variable) {
                        if (var.Values.ContainsKey(note.ValueCode))
                            CellNoteDimensions.Add(note.ValueCode);
                        else
                            CellNoteDimensions.Add(star);
                    }
                    else
                        CellNoteDimensions.Add(star);
                }
                subKeyWord = "";
                parseValue = new StringCollection();
                foreach (string dim in CellNoteDimensions)
                    subKeyWord += "\"" + dim + "\",";
                subKeyWord = subKeyWord.TrimStart('"');
                subKeyWord = subKeyWord.TrimEnd(',', '"');
                parseValue.Add(note.NotePresTexts[langCode]);
                handler(keyWord, langCode, subKeyWord, parseValue);
            }


        }
    }

 
}
