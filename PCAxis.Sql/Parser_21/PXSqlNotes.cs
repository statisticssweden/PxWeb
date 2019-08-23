using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using PCAxis.Sql.QueryLib_21;

namespace PCAxis.Sql.Parser_21 {


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

        public PXSqlNote(RelevantFootNotesRow footNoteRow, PXSqlMeta_21 mMeta) {
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


        public PXSqlNote(PXSqlMeta_21 mMeta)
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
        public void ParseNote(PXSqlMeta_21 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
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
        public void ParseNote(PXSqlMeta_21 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
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
        public void ParseNote(PXSqlMeta_21 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
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
        public void ParseNote(PXSqlMeta_21 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
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



        public void ParseNote(PXSqlMeta_21 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode)
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
        public void ParseNote(PXSqlMeta_21 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string langCode) {
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
