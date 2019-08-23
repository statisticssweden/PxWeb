using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using PCAxis.Sql.QueryLib_23;
using PCAxis.Paxiom;

namespace PCAxis.Sql.Parser_23
{



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




    /// <summary>
    /// Handles the footnotes.
    /// get footnotes from the footnote junction tables and the footnote(_lang) table(s)
    /// and enroll them in the appropriate X-Candidate "list",if the note has a correct ShowFootnot and is placed on something that exists in the output paxiom instance. 
    ///
    /// the candidate lists are procesed:
    ///  upgrade or store in output-list (ready for parsing) 
    ///
    /// If the footnotes has presCharacter <> "" the footnote is parsed both as a normal fotnote and as a DATANOTE.  
    ///
    /// </summary>
    internal class PXSqlNotes
    {

        //the tableNotes will all be included in finalnotes, as they cant be upgraded
        /// <summary>
        /// Key is footnoteNo
        /// </summary>
        private Dictionary<String, TableAttachedNote> candidatesTableNote = new Dictionary<string, TableAttachedNote>();


        private Dictionary<String, List<VariableAttachedNote>> candidatesClassificationVariableNoteByNoteNo = new Dictionary<string, List<VariableAttachedNote>>();


        private Dictionary<String, Dictionary<String, List<ValueAttachedNote>>> candidatesClassificationValueNoteByVariableByNoteNo = new Dictionary<string, Dictionary<string, List<ValueAttachedNote>>>();

        private Dictionary<String, List<ContentsValueAttachedNote>> candidatesContentsValueNoteByNoteNo = new Dictionary<string, List<ContentsValueAttachedNote>>();


        private List<CellAttachedNote> candidatesCellNotes = new List<CellAttachedNote>();


        /// <summary>
        /// This is the only one read by ParseAllNotes. 
        /// </summary>
        private List<AttachedNote> finalNotes = new List<AttachedNote>();


        private StringCollection showFootnotesCodes;

        private PXSqlMeta_23 mMeta;
        private PXSqlVariables mVariables;
        private PXSqlSubTables mSubTables;
        private Dictionary<string, PXSqlContent> mContents;
        private PXSqlVariable mTimeVariable;
        private readonly string mMainTableId;
        private readonly string contentsDimentionId;
        private readonly string timeVariableId;

        internal PXSqlNotes(PXSqlMeta_23 mMeta, string mMainTableId, bool inPresentationModus)
        {
            this.mMeta = mMeta;
            this.mVariables = mMeta.Variables;

            this.mSubTables = mMeta.SubTables;
            this.mContents = mMeta.Contents;
            this.mTimeVariable = mMeta.TimeVariable;
            this.mMainTableId = mMainTableId;

            this.contentsDimentionId = mMeta.ContensCode;
            this.timeVariableId = mMeta.TimeVariable.Name;

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


            //Creates the footnotes from the database tables and if their point of attachment is part of the return cube and Selection/Presentation value is ok,  adds them to a candidate list  
            enroll(inPresentationModus);


            //the candidates are either upgrades or put in the out list (finalNotes) 


            this.processCellNoteCandidates();

            this.processContentsValueNoteCandidates();

            this.processClassificationValueNoteCandidates();

            this.processClassificationVariableNoteCandidates();

            this.processCandidatesTableNoteCandidates();


            //boarding complete.  Ready for the call to ParseAllNotes
        }


        //it seems paxiom uses presCode to address values on the contents-dimention... why is it double-IDed ?
        private string recodeContents(string contents)
        {
            string contentsPresCode = mMeta.ContentsVariable.Values.GetValueByContentsCode(contents).ValueCode;
            return contentsPresCode;
        }



        #region enroll

        private void enroll(bool inPresentationModus)
        {
            if (this.mMeta.MainTable.ContainsOnlyMetaData)
            {
                this.addTableNoteCandidate(new TableAttachedNote(this.mMeta));
            }


            //rows from BIG sql.

            List<RelevantFootNotesRow> allRelevantFoonotes = mMeta.MetaQuery.GetRelevantFoonotes(this.mMainTableId);

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
                        enrollMainTableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.SubTable:
                        enrollSubTableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Contents:
                        enrollContentsNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Variable:
                        enrollVariableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Value:
                        enrollValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.MainTableValue:
                        enrollMaintValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsVbl:
                        enrollContVblNotes(footNoteRow);
                        break;
                    default:
                        throw new Exception("BUG unknown footNoteRow.FootNoteType case :" + footNoteRow.FootNoteType);
                }
            }

            //these footnote tables was included in the CNMM after the big SQL was written. They could (perhaps should) be included in the BIG one, but I dare not touch it.
            //It seems it big SQL did not cover ContValue And ContTime fully (I might be mistaken), so they where given separate treatment. 


            //Adding footnotes for grouping 
            this.enrollGroupingNotes(inPresentationModus);

            this.enrollValuesetValueNotes();


            this.enrollMaintTimeNotes();

            this.enrollContValueAndTimeNotes();
        }


        /// <summary>
        /// Enrolles notes on values in the time variable.
        /// </summary>
        private void enrollMaintTimeNotes()
        {

            foreach (FootnoteMaintTimeRow fmtr in this.mMeta.MetaQuery.GetFootnoteMaintTimeRows(this.mMainTableId, true).Values)
            {

                if (mTimeVariable.Values.ContainsKey(fmtr.TimePeriod))
                {
                    FootnoteRow fr = mMeta.MetaQuery.GetFootnoteRow(fmtr.FootnoteNo);
                    if (!this.showFootnotesCodes.Contains(fr.ShowFootnote))
                    {
                        continue;
                    }
                    addClassificationValueNoteCandidate(new ValueAttachedNote(fr, mTimeVariable.Name, fmtr.TimePeriod, this.mMeta));

                }
            }
        }


        /// <summary>
        /// Enrolls notes on Grouping for the variables
        /// </summary>
        /// <param name="inPresentationModus"></param>
        private void enrollGroupingNotes(bool inPresentationModus)
        {

            foreach (string selectedVarCode in mMeta.Variables.GetSelectedClassificationVarableIds())
            {
                PXSqlVariableClassification clVar = (PXSqlVariableClassification)mMeta.Variables[selectedVarCode];

                StringCollection groupingids = new StringCollection();
                if (inPresentationModus)
                {
                    if (!String.IsNullOrEmpty(clVar.CurrentGroupingId))
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
                        FootnoteRow fr = mMeta.MetaQuery.GetFootnoteRow(fgr.FootnoteNo);
                        if (this.showFootnotesCodes.Contains(fr.ShowFootnote))
                        {
                            addClassificationVariableNoteCandidate(new VariableAttachedNote(fr, clVar.Name, mMeta));
                        }
                    }
                }

            }
        }


        /// <summary>
        /// enrolls footnotes from FootnoteValuesetValue to ValueNote candidates .
        /// </summary>
        private void enrollValuesetValueNotes()
        {

            foreach (string selectedVarCode in mMeta.Variables.GetSelectedClassificationVarableIds())
            {


                PXSqlVariableClassification clVar = (PXSqlVariableClassification)mMeta.Variables[selectedVarCode];

                foreach (FootnoteValueSetValueRow fvsr in mMeta.MetaQuery.GetFootnoteValueSetValueRows(clVar.ValusetIds, true).Values)
                {
                    if (!clVar.Values.ContainsKey(fvsr.ValueCode))
                    {
                        continue;
                    }


                    FootnoteRow fr = mMeta.MetaQuery.GetFootnoteRow(fvsr.FootnoteNo);
                    if (!this.showFootnotesCodes.Contains(fr.ShowFootnote))
                    {
                        continue;
                    }
                    addClassificationValueNoteCandidate(new ValueAttachedNote(fr, clVar.Name, fvsr.ValueCode, this.mMeta));

                }
            }
        }





        private void enrollMainTableNotes(RelevantFootNotesRow footNoteRow)
        {
            this.addTableNoteCandidate(new TableAttachedNote(footNoteRow, this.mMeta));
        }

        private void enrollSubTableNotes(RelevantFootNotesRow footNoteRow)
        {
            if (mSubTables.ContainsKey(footNoteRow.SubTable))
            {
                if (mSubTables[footNoteRow.SubTable].IsSelected)
                {
                    this.addTableNoteCandidate(new TableAttachedNote(footNoteRow, this.mMeta));
                }

            }
        }

        private void enrollVariableNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    VariableAttachedNote mNote = new VariableAttachedNote(footNoteRow, this.mMeta);
                    addClassificationVariableNoteCandidate(mNote);
                }
            }
        }

        private void enrollValueNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                    {
                        if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                        {
                            addClassificationValueNoteCandidate(new ValueAttachedNote(footNoteRow, footNoteRow.Variable, footNoteRow.ValueCode, this.mMeta));
                        }
                    }
                }
            }
        }
        private void enrollMaintValueNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                    {
                        if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                        {
                            addClassificationValueNoteCandidate(new ValueAttachedNote(footNoteRow, footNoteRow.Variable, footNoteRow.ValueCode, this.mMeta));
                        }
                    }
                }
            }
        }


        private void enrollContentsNotes(RelevantFootNotesRow footNoteRow)
        {
            // Contents

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                //it seems paxiom uses presCode to address values on the contents-dimention...


                addContentsValueNoteCandidate(new ContentsValueAttachedNote(footNoteRow, mMeta.ContensCode, this.recodeContents(footNoteRow.Contents), footNoteRow.Contents, this.mMeta));



            }
        }

        private void enrollContVblNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {

                        if (mMeta.Contents.Count == 1)
                        {
                            //pure variable note
                            addClassificationVariableNoteCandidate(new VariableAttachedNote(footNoteRow, mMeta));
                        }
                        else
                        {
                            // if more than one contentsvariable is selected the note should be presented different,
                            // as is it not really a VariableNote but there are
                            // no way to parse it to PAXIOm and specify that the note is valid for a variable and  only one contents.
                            addClassificationVariableNoteCandidate(new VariableAttachedNote(footNoteRow, mMeta));
                        }
                    }
                }
            }
        }

        private void enrollContValueAndTimeNotes()
        {

            Dictionary<string, List<FootnoteContTimeRow>> contTimeRaw = mMeta.MetaQuery.GetFootnoteContTimeRows(mMainTableId, true);
            Dictionary<string, List<FootnoteContValueRow>> contValueRaw = mMeta.MetaQuery.GetFootnoteContValueRows(mMainTableId, true);

            ///all (time + value) noteNo 
            StringCollection allNoteNos = new StringCollection();

            foreach (string footnoteNo in contTimeRaw.Keys)
            {
                allNoteNos.Add(footnoteNo);
            }
            foreach (string footnoteNo in contValueRaw.Keys)
            {
                if (!allNoteNos.Contains(footnoteNo))
                {
                    allNoteNos.Add(footnoteNo);
                }
            }

            if (allNoteNos.Count == 0)
            {
                return;
                 //that's a good table! :-)
            }

            Dictionary<string, FootnoteRow> footnoteRows = mMeta.MetaQuery.GetFootnoteRows(allNoteNos, true);



            //the rows with Cellnote = YES are chained so if one is not selected all
            // rows with that noteno and Cellnote = YES should be ignored
            //this is the list of bad numbers:
            StringCollection noteNoOfNotSelectedCellnotes = new StringCollection();

            

            /// the rows in contValueRaw which has Cellnote = YES
            Dictionary<string, List<FootnoteContValueRow>> contValueCells = new Dictionary<string, List<FootnoteContValueRow>>();


            // footnoteNumber , timeperiod, list of contents (for cellnote = No, where we want to see if all contents is selected for given footnoteNumber and timeperiod in
            // which case an "upgrade" to valuenote an time is done.
            Dictionary<string, Dictionary<string, StringCollection>> contentslistByTimeperiodByNoteno = new Dictionary<string, Dictionary<string, StringCollection>>();

            /// the rows in contTimeRaw which has Cellnote = YES
            // footnoteNumber ,contents , list of  timeperiod  (for cellnote = yes a join on contents is neeed,
            Dictionary<string, Dictionary<string, StringCollection>> timeperiodlistByContentsByNoteno = new Dictionary<string, Dictionary<string, StringCollection>>();


            // footnoteNumber , variable, value, list of contents 
            Dictionary<string, Dictionary<string, Dictionary<string, StringCollection>>> contentslistByValueByVariableByNoteno = new Dictionary<string, Dictionary<string, Dictionary<string, StringCollection>>>();

            // footnoteNumber, contents , variable, list of value   ( is joined on for footnoteNumber, contents with timeperiodlistByContentsByNoteno)
            Dictionary<string, Dictionary<string, Dictionary<string, StringCollection>>> valuelistByVariableByContentsByNoteno = new Dictionary<string, Dictionary<string, Dictionary<string, StringCollection>>>();




            bool isSelected = false;
            bool cellnoteColumnIsYES = false;

            #region foreach (string noteno in contTimeRaw.Keys)
            foreach (string noteno in contTimeRaw.Keys)
            {
                if (!this.showFootnotesCodes.Contains(footnoteRows[noteno].ShowFootnote))
                {
                    continue;
                }

                foreach (FootnoteContTimeRow footNoteRow in contTimeRaw[noteno])
                {
                    isSelected = false;
                    
                    if (mContents.ContainsKey(footNoteRow.Contents) && mTimeVariable.Values.ContainsKey(footNoteRow.TimePeriod))
                    {
                        isSelected = true;
                    }

                    cellnoteColumnIsYES = footNoteRow.Cellnote.Equals(mMeta.Config.Codes.Yes);

                    if (!isSelected)
                    {
                        if (cellnoteColumnIsYES)
                        {
                            //Add it to the bad-list:
                            if (!noteNoOfNotSelectedCellnotes.Contains(noteno))
                            {
                                noteNoOfNotSelectedCellnotes.Add(noteno);
                            }
                        }
                        continue;
                    }


                    if (cellnoteColumnIsYES)
                    {
                        if (!timeperiodlistByContentsByNoteno.ContainsKey(noteno))
                        {
                            timeperiodlistByContentsByNoteno[noteno] = new Dictionary<string, StringCollection>();
                        }

                        if (!timeperiodlistByContentsByNoteno[noteno].ContainsKey(footNoteRow.Contents))
                        {
                            timeperiodlistByContentsByNoteno[noteno][footNoteRow.Contents] = new StringCollection();
                        }

                        timeperiodlistByContentsByNoteno[noteno][footNoteRow.Contents].Add(footNoteRow.TimePeriod);

                    }
                    else
                    {
                        if (!contentslistByTimeperiodByNoteno.ContainsKey(noteno))
                        {
                            contentslistByTimeperiodByNoteno[noteno] = new Dictionary<string, StringCollection>();
                        }

                        if (!contentslistByTimeperiodByNoteno[noteno].ContainsKey(footNoteRow.TimePeriod))
                        {
                            contentslistByTimeperiodByNoteno[noteno][footNoteRow.TimePeriod] = new StringCollection();
                        }
                        contentslistByTimeperiodByNoteno[noteno][footNoteRow.TimePeriod].Add(footNoteRow.Contents);
                    }
                }
            }
            #endregion foreach (string noteno in contTimeRaw.Keys)

            /////////////

            #region foreach (string noteno in contValueRaw.Keys)
            foreach (string noteno in contValueRaw.Keys)
            {
                if (!this.showFootnotesCodes.Contains(footnoteRows[noteno].ShowFootnote))
                {
                    continue;
                }
                foreach (FootnoteContValueRow footNoteRow in contValueRaw[noteno])
                {
                    isSelected = false;
                    
                    if (mContents.ContainsKey(footNoteRow.Contents))
                    {
                        if (mVariables.GetSelectedClassificationVarableIds().Contains(footNoteRow.Variable))
                        {

                            if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                            {
                                if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                                {
                                    isSelected = true;
                                }
                            }

                        }
                    }

                    cellnoteColumnIsYES = footNoteRow.Cellnote.Equals(mMeta.Config.Codes.Yes);


                    if (!isSelected)
                    {
                        if (cellnoteColumnIsYES)
                        {
                            if (!noteNoOfNotSelectedCellnotes.Contains(noteno))
                            {
                                noteNoOfNotSelectedCellnotes.Add(noteno);
                            }
                        }
                        continue;
                    }

                    //valuelistByVariableByContentsByNoteno 
                    if (cellnoteColumnIsYES)
                    {
                        if (!valuelistByVariableByContentsByNoteno.ContainsKey(noteno))
                        {
                            valuelistByVariableByContentsByNoteno[noteno] = new Dictionary<string, Dictionary<string, StringCollection>>();
                        }
                        if (!valuelistByVariableByContentsByNoteno[noteno].ContainsKey(footNoteRow.Contents))
                        {
                            valuelistByVariableByContentsByNoteno[noteno][footNoteRow.Contents] = new Dictionary<string, StringCollection>();
                        }
                        if (!valuelistByVariableByContentsByNoteno[noteno][footNoteRow.Contents].ContainsKey(footNoteRow.Variable))
                        {
                            valuelistByVariableByContentsByNoteno[noteno][footNoteRow.Contents][footNoteRow.Variable] = new StringCollection();
                        }

                        valuelistByVariableByContentsByNoteno[noteno][footNoteRow.Contents][footNoteRow.Variable].Add(footNoteRow.ValueCode);
                    }
                    else
                    {
                        if (!contentslistByValueByVariableByNoteno.ContainsKey(noteno))
                        {
                            contentslistByValueByVariableByNoteno[noteno] = new Dictionary<string, Dictionary<string, StringCollection>>();
                        }

                        if (!contentslistByValueByVariableByNoteno[noteno].ContainsKey(footNoteRow.Variable))
                        {
                            contentslistByValueByVariableByNoteno[noteno][footNoteRow.Variable] = new Dictionary<string, StringCollection>();
                        }
                        if (!contentslistByValueByVariableByNoteno[noteno][footNoteRow.Variable].ContainsKey(footNoteRow.ValueCode))
                        {
                            contentslistByValueByVariableByNoteno[noteno][footNoteRow.Variable][footNoteRow.ValueCode] = new StringCollection();
                        }
                        contentslistByValueByVariableByNoteno[noteno][footNoteRow.Variable][footNoteRow.ValueCode].Add(footNoteRow.Contents);
                    }
                }
            }
            #endregion foreach (string noteno in contValueRaw.Keys)


           

            #region Singlerow notes
            /// 
            foreach (string footno in contentslistByTimeperiodByNoteno.Keys)
            {
                foreach (string timeperiod in contentslistByTimeperiodByNoteno[footno].Keys)
                    if (contentslistByTimeperiodByNoteno[footno][timeperiod].Count == mContents.Count)
                    {
                        // all contents (all value in the content dimention) has this footnote, so it is upgraded to a note on the timedimention for the timeperiod
                        addClassificationValueNoteCandidate(new ValueAttachedNote(footnoteRows[footno], this.timeVariableId, timeperiod, this.mMeta));
                    }
                    else
                    {
                        foreach (string contents in contentslistByTimeperiodByNoteno[footno][timeperiod])
                        {
                            CellAddress tmpAddress = new CellAddress(mMeta);
                            tmpAddress.setValueCode(this.timeVariableId, timeperiod);
                            tmpAddress.setValueCode(this.contentsDimentionId, this.recodeContents(contents));

                            candidatesCellNotes.Add(new CellAttachedNote(footnoteRows[footno], tmpAddress, this.mMeta));
                        }

                    }

            }

            foreach (string footno in contentslistByValueByVariableByNoteno.Keys)
            {
                foreach (string variable in contentslistByValueByVariableByNoteno[footno].Keys)
                {
                    foreach (string valuecode in contentslistByValueByVariableByNoteno[footno][variable].Keys)
                    {
                        if (contentslistByValueByVariableByNoteno[footno][variable][valuecode].Count == mContents.Count)
                        {
                            // all contents (all value in the content dimention) has this footnote, so it is upgraded to a note on the valueCode for the valiable
                            addClassificationValueNoteCandidate(new ValueAttachedNote(footnoteRows[footno], variable, valuecode, mMeta));
                        }
                        else
                        {
                            foreach (string contents in contentslistByValueByVariableByNoteno[footno][variable][valuecode])
                            {
                                CellAddress tmpAddress = new CellAddress(mMeta);
                                tmpAddress.setValueCode(variable, valuecode);
                                tmpAddress.setValueCode(this.contentsDimentionId, this.recodeContents(contents));

                                candidatesCellNotes.Add(new CellAttachedNote(footnoteRows[footno], tmpAddress, this.mMeta));
                            }

                        }
                    }
                }

            }

            #endregion Singlerow notes


            #region Multirow notes

            ///  remove cells which is part of a (footnoteNo,Cellnote = YES) unit, where one (or more) is not selected
            foreach (string noteno in noteNoOfNotSelectedCellnotes)
            {
                timeperiodlistByContentsByNoteno.Remove(noteno);
                valuelistByVariableByContentsByNoteno.Remove(noteno);
            }
            
            // 
            Dictionary<string, StringCollection> timeperiodlistByContents = new Dictionary<string, StringCollection>();
            Dictionary<string, Dictionary<string, StringCollection>> valueByVariableByContents = new Dictionary<string, Dictionary<string, StringCollection>>();

            


            foreach (string noteno in allNoteNos)
            {
                timeperiodlistByContents.Clear();
                if (timeperiodlistByContentsByNoteno.ContainsKey(noteno))
                {
                    timeperiodlistByContents = timeperiodlistByContentsByNoteno[noteno];
                }

                valueByVariableByContents.Clear();
                if (valuelistByVariableByContentsByNoteno.ContainsKey(noteno))
                {
                    valueByVariableByContents = valuelistByVariableByContentsByNoteno[noteno];
                }


                /// valueByVariableByContents:
                //  Var   Val
                //  v1    v1w1  
                //  v1    v1w2
                //  v2    v2w1  
                //  v3    v3w1  
                //  v3    v3w2
                //
                //  is tranformed to CellAddress :
                //
                //  v1      v2     v3
                //  v1w1    v2w1   v3w1
                //  v1w1    v2w1   v3w2
                //  v1w2    v2w1   v3w1
                //  v1w2    v2w1   v3w2



                StringCollection joinedContents = new StringCollection();

                foreach (string contents in timeperiodlistByContents.Keys)
                {
                    joinedContents.Add(contents);
                }
                foreach (string contents in valueByVariableByContents.Keys)
                {
                    if (!joinedContents.Contains(contents))
                    {
                        joinedContents.Add(contents);
                    }
                }

                // cellListIn/Out : In/Out means ... sort of ... before/after a dimention is "added" ... in/out for the prossess of adding values for a dimention 
                List<CellAddress> cellListIn;
                List<CellAddress> cellListOut;
                CellAddress tempCellAddress;

                foreach (string contents in joinedContents)
                {

                    cellListIn = new List<CellAddress>();
                    cellListOut = new List<CellAddress>();
                    tempCellAddress = new CellAddress(mMeta);
                    tempCellAddress.setValueCode(contentsDimentionId, this.recodeContents(contents));
                    cellListIn.Add(tempCellAddress);

                    if (timeperiodlistByContents.ContainsKey(contents))
                    {
                        foreach (string timeperiod in timeperiodlistByContents[contents])
                        {
                            foreach (CellAddress aCell in cellListIn)
                            {
                                tempCellAddress = new CellAddress(aCell);
                                tempCellAddress.setValueCode(timeVariableId, timeperiod);
                                cellListOut.Add(tempCellAddress);
                            }
                        }

                        cellListIn = cellListOut;
                        cellListOut = new List<CellAddress>();
                    }


                    if (valueByVariableByContents.ContainsKey(contents))
                    {

                        foreach (string variable in valueByVariableByContents[contents].Keys)
                        {


                            foreach (string valueCode in valueByVariableByContents[contents][variable])
                            {
                                foreach (CellAddress aCell in cellListIn)
                                {
                                    tempCellAddress = new CellAddress(aCell);
                                    tempCellAddress.setValueCode(variable, valueCode);
                                    cellListOut.Add(tempCellAddress);
                                }
                            }

                            cellListIn = cellListOut;
                            cellListOut = new List<CellAddress>();
                        }
                    }

                    foreach (CellAddress address in cellListIn)
                    {
                        candidatesCellNotes.Add(new CellAttachedNote(footnoteRows[noteno], address, mMeta));
                    }
                }



            }
            #endregion Multirow notes

        }





        #endregion enroll



        #region processCandidates
        // Upgrade or add to finalNotes


        /// <summary>
        /// CellNotes are not upgraded, so they all go to finalNotes
        /// </summary>
        private void processCellNoteCandidates()
        {
            foreach (CellAttachedNote note in candidatesCellNotes)
            {

                finalNotes.Add(note);
            }
        }







        /// <summary>
        /// Upgrade or add to finalNotes
        /// </summary>
        private void processContentsValueNoteCandidates()
        {

            StringCollection outputContentsValues = new StringCollection();
            foreach (String contentsValue in this.mMeta.Contents.Keys)
            {
                outputContentsValues.Add(contentsValue);
            }

            foreach (String noteno in candidatesContentsValueNoteByNoteNo.Keys)
            {
                StringCollection usedValues = new StringCollection();
                foreach (ContentsValueAttachedNote anote in candidatesContentsValueNoteByNoteNo[noteno])
                {
                    if (!usedValues.Contains(anote.Contents))
                    {
                        usedValues.Add(anote.Contents);
                    }
                }




                if (PCAxis.Sql.Parser.PXSqlMeta.containsAll(outputContentsValues, usedValues))
                {
                    this.addTableNoteCandidate(new TableAttachedNote(this.mMeta.MetaQuery.GetFootnoteRow(noteno), this.mMeta));
                }
                else
                {
                    foreach (ContentsValueAttachedNote note in candidatesContentsValueNoteByNoteNo[noteno])
                    {
                        finalNotes.Add(note);
                    }

                }

            }


        }



        private void processClassificationValueNoteCandidates()
        {

            foreach (String noteNo in candidatesClassificationValueNoteByVariableByNoteNo.Keys)
            {
                foreach (String variable in candidatesClassificationValueNoteByVariableByNoteNo[noteNo].Keys)
                {
                    List<ValueAttachedNote> tmp = candidatesClassificationValueNoteByVariableByNoteNo[noteNo][variable];

                    StringCollection valuesWhichHasNotes = new StringCollection();
                    foreach (ValueAttachedNote note in tmp)
                    {
                        if (!valuesWhichHasNotes.Contains(note.ValueCode))
                        {
                            valuesWhichHasNotes.Add(note.ValueCode);
                        }
                    }

                    if (mVariables[variable].Values.Keys.Count == valuesWhichHasNotes.Count)
                    {
                        addClassificationVariableNoteCandidate(new VariableAttachedNote(tmp[0]));
                    }
                    else
                    {
                        foreach (ValueAttachedNote note in tmp)
                        {
                            finalNotes.Add(note);
                        }

                    }

                }
            }
        }


        private void processClassificationVariableNoteCandidates()
        {
            //Upgrade if all these are found
            StringCollection outputVariables = this.mVariables.GetSelectedClassificationVarableIds();

            foreach (String noteno in candidatesClassificationVariableNoteByNoteNo.Keys)
            {
                StringCollection varsWhichHasANote = new StringCollection();
                foreach (VariableAttachedNote note in candidatesClassificationVariableNoteByNoteNo[noteno])
                {
                    if (!varsWhichHasANote.Contains(note.Variable))
                    {
                        varsWhichHasANote.Add(note.Variable);
                    }
                }


                if (PCAxis.Sql.Parser.PXSqlMeta.containsAll(outputVariables, varsWhichHasANote))
                {
                    this.addTableNoteCandidate(new TableAttachedNote(this.mMeta.MetaQuery.GetFootnoteRow(noteno), this.mMeta));
                }
                else
                {
                    foreach (VariableAttachedNote note in candidatesClassificationVariableNoteByNoteNo[noteno])
                    {
                        this.finalNotes.Add(note);
                    }

                }
            }
        }

        //tablenotes are all just added to finalNotes
        private void processCandidatesTableNoteCandidates()
        {
            foreach (TableAttachedNote note in candidatesTableNote.Values)
            {
                finalNotes.Add(note);
            }
        }

        #endregion processCandidates

        #region add

        private void addClassificationValueNoteCandidate(ValueAttachedNote note)
        {


            if (!candidatesClassificationValueNoteByVariableByNoteNo.ContainsKey(note.FootNoteNo))
            {
                candidatesClassificationValueNoteByVariableByNoteNo[note.FootNoteNo] = new Dictionary<String, List<ValueAttachedNote>>();
            }

            if (!candidatesClassificationValueNoteByVariableByNoteNo[note.FootNoteNo].ContainsKey(note.Variable))
            {
                candidatesClassificationValueNoteByVariableByNoteNo[note.FootNoteNo][note.Variable] = new List<ValueAttachedNote>();
            }
            candidatesClassificationValueNoteByVariableByNoteNo[note.FootNoteNo][note.Variable].Add(note);

        }

        private void addContentsValueNoteCandidate(ContentsValueAttachedNote note)
        {
            if (!candidatesContentsValueNoteByNoteNo.ContainsKey(note.FootNoteNo))
            {
                candidatesContentsValueNoteByNoteNo[note.FootNoteNo] = new List<ContentsValueAttachedNote>();
            }
            candidatesContentsValueNoteByNoteNo[note.FootNoteNo].Add(note);
        }


        private void addClassificationVariableNoteCandidate(VariableAttachedNote note)
        {
            if (!candidatesClassificationVariableNoteByNoteNo.ContainsKey(note.FootNoteNo))
            {
                candidatesClassificationVariableNoteByNoteNo[note.FootNoteNo] = new List<VariableAttachedNote>();
            }
            candidatesClassificationVariableNoteByNoteNo[note.FootNoteNo].Add(note);
        }



        //adds tableNote unless it is already added
        private void addTableNoteCandidate(TableAttachedNote tableNote)
        {
            if (!candidatesTableNote.ContainsKey(tableNote.FootNoteNo))
            {
                candidatesTableNote.Add(tableNote.FootNoteNo, tableNote);
            }
        }


        #endregion add

        // last, but not least
        internal void ParseAllNotes(PCAxis.Paxiom.IPXModelParser.MetaHandler handler)
        {

            foreach (AttachedNote note in finalNotes)
            {
                note.ParseNote(mMeta, handler, mMeta.LanguageCodes);
            }

        }
    }




    //
    abstract internal class AttachedNote
    {
        private string mandOpt;
        private string codeForMandatory;


        private string footNoteNo;
        /// <summary>
        /// The number from the FOOTNOTE table. 
        /// </summary>
        public string FootNoteNo
        {
            get { return this.footNoteNo; }
        }

        protected string presCharacter = "";

        internal bool hasPresCharacter
        {
            get { return !String.IsNullOrEmpty(this.presCharacter); }
        }


        protected Dictionary<string, String> mNotePresTexts = new Dictionary<string, String>();

        protected const string FootNoteTableContainsOnlyMetadata = "This table contains only metadata";
        protected const string FootNoteTableContainsOnlyMetadataDummyNumber = "-999";


        internal AttachedNote(FootnoteRow footnoteRow, PXSqlMeta_23 mMeta)
        {
            this.codeForMandatory = mMeta.Config.Codes.FootnoteM;
            this.mandOpt = footnoteRow.MandOpt;

            this.presCharacter = footnoteRow.PresCharacter;

            this.footNoteNo = footnoteRow.FootnoteNo;

            foreach (string langCode in mMeta.LanguageCodes)
            {
                this.mNotePresTexts.Add(langCode, footnoteRow.texts[langCode].FootnoteText);
            }
        }

        internal AttachedNote(RelevantFootNotesRow relevant, PXSqlMeta_23 mMeta)
        {
            this.codeForMandatory = mMeta.Config.Codes.FootnoteM;
            this.mandOpt = relevant.MandOpt;

            this.presCharacter = relevant.PresCharacter;

            this.footNoteNo = relevant.FootNoteNo;

            foreach (string langCode in mMeta.LanguageCodes)
            {
                this.mNotePresTexts.Add(langCode, relevant.texts[langCode].FootNoteText);
            }
        }

        internal AttachedNote(PXSqlMeta_23 mMeta)
        {
            this.codeForMandatory = mMeta.Config.Codes.FootnoteM;
            this.mandOpt = mMeta.Config.Codes.FootnoteM;

            this.footNoteNo = FootNoteTableContainsOnlyMetadataDummyNumber;

            foreach (string langCode in mMeta.LanguageCodes)
            {
                this.mNotePresTexts.Add(langCode, FootNoteTableContainsOnlyMetadata);
            }
        }


        internal AttachedNote(AttachedNote otherOutNote)
        {
            this.mandOpt = otherOutNote.mandOpt;
            this.codeForMandatory = otherOutNote.codeForMandatory;
            this.footNoteNo = otherOutNote.footNoteNo;
            this.presCharacter = otherOutNote.presCharacter;
            this.mNotePresTexts = otherOutNote.mNotePresTexts;
        }

        abstract internal void ParseNote(PXSqlMeta_23 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection langCodes);

        internal string returnsXWhenNoteIsMandatory()
        {
            string myOut = "";
            if (this.mandOpt == this.codeForMandatory || this.hasPresCharacter)
            {
                myOut = "X";
            }
            return myOut;
        }

    }



    internal class TableAttachedNote : AttachedNote
    {
        internal TableAttachedNote(FootnoteRow footnoteRow, PXSqlMeta_23 mMeta)
            : base(footnoteRow, mMeta)
        {
        }

        internal TableAttachedNote(RelevantFootNotesRow relevant, PXSqlMeta_23 mMeta)
            : base(relevant, mMeta)
        {
        }

        internal TableAttachedNote(PXSqlMeta_23 mMeta)
            : base(mMeta)
        {
        }



        override internal void ParseNote(PXSqlMeta_23 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection langCodes)
        {
            string keyWord;
            string subKeyWord = null;
            StringCollection parseValue;
            if (this.hasPresCharacter)
            {
                keyWord = PXKeywords.DATANOTE; 
                parseValue = new StringCollection();
                parseValue.Add(this.presCharacter);

                handler(keyWord, null, subKeyWord, parseValue);

                //TODO; Does this work? 
            }

            foreach (string langCode in langCodes)
            {

                parseValue = new StringCollection();

                keyWord = PXKeywords.NOTE + this.returnsXWhenNoteIsMandatory();

                parseValue.Add(this.mNotePresTexts[langCode]);
                handler(keyWord, langCode, subKeyWord, parseValue);
            }
        }

    }


    internal class VariableAttachedNote : AttachedNote
    {
        private string variableCode;

        internal string Variable
        {
            get { return variableCode; }
        }


        internal VariableAttachedNote(FootnoteRow footnoteRow, string variableCode, PXSqlMeta_23 mMeta)
            : base(footnoteRow, mMeta)
        {
            this.variableCode = variableCode;
        }


        internal VariableAttachedNote(RelevantFootNotesRow relevant, PXSqlMeta_23 mMeta)
            : base(relevant, mMeta)
        {
            this.variableCode = relevant.Variable;
        }

        internal VariableAttachedNote(ValueAttachedNote outValueNote)
            : base(outValueNote)
        {

            this.variableCode = outValueNote.Variable;
        }



        override internal void ParseNote(PXSqlMeta_23 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection langCodes)
        {
            string keyWord;
            string subKeyWord = this.variableCode;
            StringCollection parseValue;

            if (this.hasPresCharacter)
            {
                keyWord = PXKeywords.DATANOTE;
                parseValue = new StringCollection();
                parseValue.Add(this.presCharacter);

                handler(keyWord, null, subKeyWord, parseValue);
            }


            keyWord = PXKeywords.NOTE + this.returnsXWhenNoteIsMandatory();

            foreach (string langCode in langCodes)
            {

                parseValue = new StringCollection();
                parseValue.Add(this.mNotePresTexts[langCode]);

                handler(keyWord, langCode, subKeyWord, parseValue);
            }
        }
    }




    /// <summary>
    /// Because contents.contents and contents.presCode
    /// </summary>
    internal class ContentsValueAttachedNote : ValueAttachedNote
    {
        private string mContents;

        internal string Contents
        {
            get { return mContents; }
        }

        internal ContentsValueAttachedNote(RelevantFootNotesRow relevant, string variableCode, string valueCode, string contentsCode, PXSqlMeta_23 mMeta)
            : base(relevant, variableCode, valueCode, mMeta)
        {
            this.mContents = contentsCode;
        }
    }


    internal class ValueAttachedNote : AttachedNote
    {
        private string variableCode;

        internal string Variable
        {
            get { return variableCode; }
        }


        private string valueCode;

        internal string ValueCode
        {
            get { return valueCode; }
        }


        internal ValueAttachedNote(FootnoteRow footnoteRow, string variableCode, string valueCode, PXSqlMeta_23 mMeta)
            : base(footnoteRow, mMeta)
        {
            this.variableCode = variableCode;
            this.valueCode = valueCode;
        }

        internal ValueAttachedNote(RelevantFootNotesRow relevant, string variableCode, string valueCode, PXSqlMeta_23 mMeta)
            : base(relevant, mMeta)
        {
            this.variableCode = variableCode;
            this.valueCode = valueCode;
        }



        override internal void ParseNote(PXSqlMeta_23 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection langCodes)
        {
            string keyWord;
            string subKeyWord = this.variableCode + "\",\"" + this.valueCode;
            StringCollection parseValue;

            if (this.hasPresCharacter)
            {
                keyWord = PXKeywords.DATANOTE;
                parseValue = new StringCollection();
                parseValue.Add(this.presCharacter);

                handler(keyWord, null, subKeyWord, parseValue);

                //TODO; for presChat <> "" fjerne tableattachment og legge inn direkte parsing 
                //throw new NotImplementedException("send as symbol note");
            }

            keyWord = PXKeywords.VALUENOTE + this.returnsXWhenNoteIsMandatory();
            foreach (string langCode in langCodes)
            {

                parseValue = new StringCollection();
                parseValue.Add(this.mNotePresTexts[langCode]);

                handler(keyWord, langCode, subKeyWord, parseValue);
            }
        }
    }


    /// <summary>
    /// Class representing a point ( or line or plane or subcube) in the output cube. 
    /// </summary>
    internal class CellAddress
    {
        private const string star = "*";

        private List<string> variableIDsInOutputOrder;

        private Dictionary<string, string> valueCodeByDimention = new Dictionary<string, string>();

        /// <summary>
        /// Creates a CellAddress with "*" in all dimentions
        /// </summary>
        /// <param name="mMeta"></param>
        internal CellAddress(PXSqlMeta_23 mMeta)
        {
            variableIDsInOutputOrder = mMeta.GetVariableIDsInOutputOrder();
            foreach (string dimention in variableIDsInOutputOrder)
            {

                valueCodeByDimention[dimention] = CellAddress.star;
            }
        }


        /// <summary>
        /// Creates a copy of the sourceCell
        /// </summary>
        /// <param name="sourceCell">the one to read data from</param>
        internal CellAddress(CellAddress sourceCell)
        {
            this.variableIDsInOutputOrder = sourceCell.variableIDsInOutputOrder;
            foreach (string dimention in this.variableIDsInOutputOrder)
            {
                this.valueCodeByDimention[dimention] = sourceCell.valueCodeByDimention[dimention];
            }
        }



        /// <summary>
        /// Sets a dimention to a value. Throws an exception if dimention is unknown. Value is not checked. 
        /// </summary>
        /// <param name="onVariable">The dimention which has the valuecode</param>
        /// <param name="valueCode">The valuecode for the dimention </param>
        internal void setValueCode(string onVariable, string valueCode)
        {
            if (!valueCodeByDimention.ContainsKey(onVariable))
            {
                throw new Exception("Bug in CellAddress, setValueCode: unknown variable: " + onVariable + " (valueCode is " + valueCode + ")");
            }
            valueCodeByDimention[onVariable] = valueCode;
        }


        /// <summary>
        /// Glues the valuecodes together in "paxiom format"
        /// </summary>
        /// <returns>the string</returns>
        internal string getAddressString()
        {
            string myOut = "";
            foreach (string var in variableIDsInOutputOrder)
            {
                myOut += "\"" + valueCodeByDimention[var] + "\",";
            }
            myOut = myOut.TrimStart('"');
            myOut = myOut.TrimEnd(',', '"');
            return myOut;
        }



    }


    internal class CellAttachedNote : AttachedNote
    {
        private const string star = "*";

        private List<PXSqlVariable> stubAndHead = new List<PXSqlVariable>();

        private StringCollection CellNoteDimensions = new StringCollection();

        private CellAddress cellAddress;
        //lag dimentionMap med * som verdi og lag set variable, verdi 

        internal CellAttachedNote(FootnoteRow footnoteRow, CellAddress cell, PXSqlMeta_23 mMeta)
            : base(footnoteRow, mMeta)
        {
            this.cellAddress = cell;
        }



        override internal void ParseNote(PXSqlMeta_23 mMeta, PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection langCodes)
        {
            string keyWord = PXKeywords.CELLNOTE + this.returnsXWhenNoteIsMandatory();
            string subKeyWord = cellAddress.getAddressString();
            StringCollection parseValue;



            if (this.hasPresCharacter)
            {
                keyWord = PXKeywords.DATANOTECELL;
                parseValue = new StringCollection();
                parseValue.Add(this.presCharacter);
                handler(keyWord, null, subKeyWord, parseValue);
                //TODO; Does this work? Test
            }

            keyWord = PXKeywords.CELLNOTE + this.returnsXWhenNoteIsMandatory();
            foreach (string langCode in langCodes)
            {
                parseValue = new StringCollection();
                parseValue.Add(this.mNotePresTexts[langCode]);
                handler(keyWord, langCode, subKeyWord, parseValue);
            }
        }

    }
}
