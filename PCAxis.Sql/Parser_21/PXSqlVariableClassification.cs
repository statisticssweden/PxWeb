using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using PCAxis.Sql.QueryLib_21;
using PCAxis.Paxiom;
using PCAxis.Sql.Pxs;
using PCAxis.PlugIn.Sql;
using System.Data;
using log4net;
namespace PCAxis.Sql.Parser_21
{
    public class PXSqlVariableClassification : PXSqlVariable
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlVariableClassification));

        #region properties

        private PXSqlGroupingInfos groupingInfos;

        //added 8.2.2010 No keyword to handle parsing og groupingInfos. Metode applied in PXSqlBuilder for directly settting properties of variable in Paxiom
        public PXSqlGroupingInfos GroupingInfos
        {
            get { return groupingInfos; }
        }

        /// <summary>Is null more often than not</summary>
        private PXSqlGrouping currentGrouping;
        // for PxsQuery construction
        public string CurrentGroupingId
        {
            get
            {
                if (currentGrouping == null)
                {
                    return null;
                }
                else
                {

                    return currentGrouping.GroupingId;
                }
            }
        }

        /// <summary>
        /// Contains the ids of the real valueset ( never the "magic All")
        /// </summary>
        private StringCollection valusetIds;
        /// <summary>
        /// Contains the ids of the real valueset ( never the "magic All")
        /// </summary>
        internal StringCollection ValusetIds
        {
            get { return valusetIds; }
        }


        /// <summary> The variable in pxs with the same code(=id) if  such exists null otherwise (!meta.constructedFromPxs or variable eliminated. </summary>
        private readonly PQVariable pxsQueryVariable;


        private string aggregationType = "N";
        private string aggregatingStructureId;

        #endregion

        #region constructors
        public PXSqlVariableClassification() { }


        public PXSqlVariableClassification(MainTableVariableRow aTVRow, PXSqlMeta_21 meta)
            : base(aTVRow.Variable, meta, false, false, true)
        {


            if (this.meta.ConstructedFromPxs)
            {
                foreach (PQVariable tmpVar in this.meta.PxsFile.Query.Variables)
                {
                    if (this.Name == tmpVar.code)
                        this.pxsQueryVariable = tmpVar;
                }
            }
            SetSelected();

            if (this.isSelected)
            {
                this.mStoreColumnNo = int.Parse(aTVRow.StoreColumnNo);
                if (!this.meta.ConstructedFromPxs)
                {
                    this.mIndex = this.mStoreColumnNo;
                }

                SetValueset();
                SetValuePool();
                SetPresText();
                SetDefaultPresTextOption(); // would be overwritten if options set in pxs
                if (this.meta.ConstructedFromPxs)
                    SetOptionsFromPxs();

                if (this.meta.inSelectionModus)
                {
                    this.groupingInfos = new PXSqlGroupingInfos(this.meta, this.Name, valusetIds);
                }
            }
            else
            {
                SetValueset();
            }
            //  Elimination must be done after SetValues moved down.
            //   if (this.meta.InstanceModus == Instancemodus.selection)
            //       SetElimForSelection();
            //   else
            //       SetElimForPresentation();



            if (this.aggregationType.Equals("G"))
            {
                this.UsesGrouping = true;
                if (String.IsNullOrEmpty(this.aggregatingStructureId))
                {
                    throw new ApplicationException("Not implemented yet");
                }
                else if (this.aggregatingStructureId.Equals("UNKNOWNSTRUCTUREID"))
                {
                    List<PXSqlGroup> groupFromFile = GetListOfGroupFromPxs();

                    currentGrouping = new PXSqlGrouping(this.meta, this, groupFromFile);

                }
                else
                {
                    currentGrouping = new PXSqlGrouping(this.metaQuery.GetGroupingRow(this.ValuePool.ValuePool, this.aggregatingStructureId), this.meta, this, this.pxsQueryVariable.GetCodesNoWildcards());
                }
            }
            else
            {

                SetValues();
                SetCodelists();
            }


            if (this.meta.inSelectionModus)
                SetElimForSelection();
            else
                SetElimForPresentation();


        }

        #endregion constructors



        private void SetCodelists()
        {
            if (this.isSelected)
            {

                if (meta.inSelectionModus)
                {

                    foreach (KeyValuePair<string, PXSqlValueSet> tmpValueSet in this.ValueSets)
                    {
                        if (tmpValueSet.Key == PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS)
                        {
                            foreach (PXSqlValue value in mValues.GetValuesSortedByValue(mValues.GetValuesForSelectedValueset(tmpValueSet.Key)))
                            {
                                tmpValueSet.Value.SortedListOfCodes.Add(value.ValueCode);
                            }
                        }
                        else
                        {
                            if (tmpValueSet.Value.SortCodeExists == metaQuery.DB.Codes.Yes)
                                foreach (PXSqlValue value in mValues.GetValuesSortedByVSValue(mValues.GetValuesForSelectedValueset(tmpValueSet.Key)))
                                {
                                    tmpValueSet.Value.SortedListOfCodes.Add(value.ValueCode);
                                }
                            else
                                foreach (PXSqlValue value in mValues.GetValuesSortedByValue(mValues.GetValuesForSelectedValueset(tmpValueSet.Key)))
                                {
                                    tmpValueSet.Value.SortedListOfCodes.Add(value.ValueCode);
                                }
                        }
                    }

                }

            }
        }


        private void SetOptionsFromPxs()
        {
            if (this.pxsQueryVariable != null)
            {
                if (this.pxsQueryVariable.PresTextOption != null) // added because PresTextOption not set anymore in PxsQuery_Paxiom_partial. see comment there
                {
                    this.mPresTextOption = ConvertToDbPresTextOption(this.pxsQueryVariable.PresTextOption);
                }
                this.aggregationType = this.pxsQueryVariable.Aggregation.ToString();
                this.aggregatingStructureId = this.pxsQueryVariable.StructureId;
                log.Debug("aggregationType:" + aggregationType);

            }
        }



        private void SetValues()
        {
            StringCollection selSubTables = new StringCollection();
            foreach (PXSqlSubTable subTable in meta.SubTables.Values)
            {
                selSubTables.Add(subTable.SubTable);
            }

            if (meta.ConstructedFromPxs)
            {
                if (pxsQueryVariable != null)
                {
                    this.SetValues(selSubTables, pxsQueryVariable);
                }
            }
            else
            {
                this.SetValues(selSubTables);
            }
        }

        // when no pxs
        internal void SetValues(StringCollection selSubTables)
        {
            List<PXSqlValue> mSortedValues = new List<PXSqlValue>();

            ValueRowDictionary mValueRowDictionary = meta.MetaQuery.GetValueRowDictionary(meta.MainTable.MainTable, selSubTables, this.Name, this.ValuePool.ValueTextExists);
            Dictionary<string, ValueRow2> mValueRows = mValueRowDictionary.ValueRows;

            foreach (ValueRow2 myValueRow in mValueRows.Values)
            {
                PXSqlValue mValue = new PXSqlValue(myValueRow, meta.LanguageCodes, meta.MainLanguageCode);
                mSortedValues.Add(mValue);
            }

            PxSqlValues mValues = new PxSqlValues();

            foreach (PXSqlValue sortedValue in mSortedValues)
            {
                mValues.Add(sortedValue.ValueCode, sortedValue);
            }
            this.Values = mValues;

        }


        //when pxs
        internal void SetValues(StringCollection selSubTables, PQVariable var)
        {
            log.Debug("PQVariable code = " + var.code);
            StringCollection mSelectedValues = new StringCollection();
            // Defines a dictionary to hold all the sortorders. Necessary because of the wildcards
            Dictionary<string, int> mDefinedSortorder = new Dictionary<string, int>();
            bool usesGrouping = false;
            
            
            string mPxsSubTableId = meta.PxsFile.Query.SubTable;


            #region foreach var.Values.Items
            if (var.Values.Items.Length > 0)
            {
                int documentOrder = 0;

                foreach (PCAxis.Sql.Pxs.ValueTypeWithGroup val in var.Values.Items)
                {
                    if (val.Group != null)
                    {
                        usesGrouping = true;
                        //TODO;
                        throw new ApplicationException("PXSqlVariableClassification, SetValues: group not implemented yet");

                    }
                    if (val.code.Contains("*") || val.code.Contains("?"))
                    {

                        DataSet mValueInfoTbl = meta.MetaQuery.GetValueWildCardBySubTable(meta.MainTable.MainTable, var.code, mPxsSubTableId, val.code);
                        DataRowCollection mValueInfo = mValueInfoTbl.Tables[0].Rows;
                        
                        foreach (DataRow row in mValueInfo)
                        {
                            string mTempCode = row[meta.MetaQuery.DB.Value.ValueCode].ToString();
                            mSelectedValues.Add(mTempCode);
                            if (!mDefinedSortorder.ContainsKey(mTempCode))
                            {
                                mDefinedSortorder.Add(mTempCode, documentOrder);
                            }
                            documentOrder++;
                        }
                    }
                    else
                    {
                        mSelectedValues.Add(val.code);
                        if (!mDefinedSortorder.ContainsKey(val.code))
                        {
                            mDefinedSortorder.Add(val.code, documentOrder);
                        }
                    }

                    documentOrder++;
                }
            #endregion foreach var.Values.Items


                // mSelectedValues now contains all the selected values, including those defined by wildcards

                Dictionary<string, PXSqlValue> mTempPXSqlValues = new Dictionary<string, PXSqlValue>();
                List<PXSqlValue> mSortedValues = new List<PXSqlValue>();

                ValueRowDictionary mValueRowDictionary = meta.MetaQuery.GetValueRowDictionary(meta.MainTable.MainTable, selSubTables, var.code, mSelectedValues, this.ValuePool.ValueTextExists);
                

                // todo; fortsette her
                Dictionary<string, ValueRow2> mValueRows = mValueRowDictionary.ValueRows;

                #region foreach mValueRows
                foreach (ValueRow2 myValueRow in mValueRows.Values)
                {

                    PXSqlValue mValue = new PXSqlValue(myValueRow, meta.LanguageCodes, meta.MainLanguageCode);

                    // jfi: kommentaren sto i en kodeblock som forsvant inn i PXSqlValue
                    // todo; legge til sjekk om koden finnes blandt valgte i basen.
                    mValue.SortCodePxs = mDefinedSortorder[myValueRow.ValueCode];
                    mSortedValues.Add(mValue);
                }
                #endregion foreach mValueRows

                mValues = new PxSqlValues();

                foreach (PXSqlValue sortedValue in mSortedValues)
                {
                    mValues.Add(sortedValue.ValueCode, sortedValue);
                }

                this.Values = mValues;
                this.UsesGrouping = usesGrouping;
            }
        }


        private List<PXSqlGroup> GetListOfGroupFromPxs()
        {
            if (this.pxsQueryVariable.Values.Items.Length == 0)
            {
                throw new ApplicationException("GetListOfGroupFromPxs(): No entries found");
            }
            List<PXSqlGroup> myOut = new List<PXSqlGroup>();

            foreach (PCAxis.Sql.Pxs.ValueTypeWithGroup val in this.pxsQueryVariable.Values.Items)
            {
                if (val.code.Contains("*") || val.code.Contains("?"))
                {
                    throw new ApplicationException("GetListOfGroupFromPxs(): Groups cannot contain wildcards");
                }
                PXSqlGroup group = new PXSqlGroup(val.code);

                if (val.Group == null || val.Group.GroupValue == null || val.Group.GroupValue.Length < 1)
                {
                    throw new ApplicationException("GetListOfGroupFromPxs(): Expected group children");
                }
                foreach (GroupValueType child in val.Group.GroupValue)
                {
                    group.AddChildCode(child.code);
                }
                myOut.Add(group);
            }
            return myOut;

        }


        private void SetSelected()
        {
            this.mIsSelected = false;
            if (meta.ConstructedFromPxs)
            {
                if ((this.pxsQueryVariable != null) && (this.pxsQueryVariable.Values.Items.Length > 0))
                {
                    this.mIsSelected = true;
                }
            }
            else
            {
                this.isSelected = true;
            }
        }



        /// <summary>Started from GUI via builder, a grouping is applied.</summary>
        /// <param name="paxiomVariable">the paxiom Variable </param>
        /// <param name="groupingId">The id of the grouping</param>
        /// <param name="include">Emun value inducating what the new codelist should include:parents,childern or both </param>
        internal void ApplyGrouping(Variable paxiomVariable, string groupingId, GroupingIncludesType include)
        {
            paxiomVariable.RecreateValues();// per inge values for variable must be deleted before created for new valueset.

            this.currentGrouping = new PXSqlGrouping(this.metaQuery.GetGroupingRow(ValuePool.ValuePool, groupingId), meta, this, include);
            this.selectedValueset = PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS; //todo; or should it be valuset for vsgroup?

            //was   SetElimForSelection(); //TODO How should elimination for groups be?
            //            SetDefaultPresTextOption();


            this.PaxiomElimination = PXConstant.NO;
            this.PresTextOption = this.ValuePool.ValuePres;


            //send new state to paxiom:

            paxiomVariable.CurrentGrouping = this.currentGrouping.GetPaxiomGrouping();


        }

        internal void ApplyValueSet(string valueSetId)
        {
            this.currentGrouping = null;
            this.selectedValueset = valueSetId;
            SetElimForSelection();
            SetDefaultPresTextOption();
            //TODO; 

        }

        private void SetValueset()
        {
            valusetIds = new StringCollection();

            List<ValueSetRow> tmpList = new List<ValueSetRow>();
            //if (meta.HasSubTable)
            //{
            //    tmpList.Add(meta.MetaQuery.GetValueSetRow2(meta.MainTable.MainTable, meta.SubTables.GetSelectedSubTable().SubTable, this.Name));
            //}
            //else
            //{
            if (this.pxsQueryVariable == null || this.pxsQueryVariable.SelectedValueset == PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS)
            {
                tmpList = meta.MetaQuery.GetValueSetRows2(meta.MainTable.MainTable, this.Name);
            }
            else
            //{
            //if (this.pxsQueryVariable.SelectedValueset == PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS)
            //{
            //    tmpList = meta.MetaQuery.GetValueSetRows2(meta.MainTable.MainTable, this.Name);
            //}
            //else
            {
                tmpList.Add(meta.MetaQuery.GetValueSetRow(this.pxsQueryVariable.SelectedValueset)); // for selected valueset without subtable stored in pxs.
            }
            // }
            // }

            int NumberOfSelectedValueSets = tmpList.Count;
            mValueSets = new Dictionary<string, PXSqlValueSet>();
            int totalNumberOfValues = 0;

            List<string> elimValues = new List<string>(); // For the magicAll valueSet 

            StringCollection tmpValuePres = new StringCollection(); // For the magicAll valueSet 

            foreach (ValueSetRow vsr in tmpList)
            {
                mValueSet = new PXSqlValueSet(vsr);
                mValueSet.NumberOfValues = meta.MetaQuery.GetNumberOfValuesInValueSetById(mValueSet.ValueSet);

                totalNumberOfValues += mValueSet.NumberOfValues;

                mValueSets.Add(mValueSet.ValueSet, mValueSet);
                valusetIds.Add(mValueSet.ValueSet);

                // helpers for the magicAll 
                elimValues.Add(mValueSet.Elimination);
                if (!tmpValuePres.Contains(mValueSet.ValuePres))
                    tmpValuePres.Add(mValueSet.ValuePres);

            }

            // Add the collection to the variable.
            this.ValueSets = mValueSets;
            this.TotalNumberOfValuesInDB = totalNumberOfValues;

            if (NumberOfSelectedValueSets == 1)
            {
                selectedValueset = mValueSet.ValueSet;
            }
            else
            {
                string allValuePres;
                if (tmpValuePres.Count == 1)
                    allValuePres = tmpValuePres[0];
                else
                    allValuePres = "V"; //For valuepool TODO her må det endres slik at codes V legges i config fila 

                PXSqlValueSet magicAll = new PXSqlValueSet(this.PresText, tmpList[0].ValuePool, CheckMultValuesetElim(elimValues), metaQuery.DB.Codes.No, allValuePres);
                magicAll.NumberOfValues = totalNumberOfValues;
                this.ValueSets.Add(magicAll.ValueSet, magicAll);
            }


        }



        private void SetDefaultPresTextOption()
        {
            // if prestextoption not set e.g from Pxs then apply PresTextOption from db
            if (this.ValueSets[this.selectedValueset].ValuePres == "V" || this.ValueSets[this.selectedValueset].ValuePres == "")
            {
                this.PresTextOption = this.ValuePool.ValuePres;
            }
            else
            {
                this.PresTextOption = this.ValueSets[this.selectedValueset].ValuePres;
            }
        }

        internal string GetOneValuePoolId()
        {

            Dictionary<string, PXSqlValueSet>.Enumerator vSEnum;
            vSEnum = this.ValueSets.GetEnumerator();
            vSEnum.MoveNext();
            return vSEnum.Current.Value.ValuePoolId;
        }

        private void SetValuePool()
        {
            this.ValuePool = new PXSqlValuepool(metaQuery.GetValuePoolRow(this.GetOneValuePoolId()), meta);
        }
        /// <summary>
        /// 
        /// </summary>
        /// 

        protected void SetElimForSelection()
        {
            string tmpElim;
            this.IsEliminatedByValue = false;

            PXSqlValueSet vs = this.ValueSets[selectedValueset];

            tmpElim = vs.Elimination;

            if (tmpElim.Equals(meta.Config.Codes.EliminationN))
            {
                this.PaxiomElimination = PXConstant.NO;
            }
            else
            {
                this.PaxiomElimination = PXConstant.YES;
            }

        }
        protected override void SetElimForPresentation()
        {
            string tmpElim;
            PXSqlValue mValue;
            this.IsEliminatedByValue = false;

            List<decimal> NumberOfValuesInValuesets = new List<decimal>();


            if (pxsQueryVariable != null)
            {
                if (!string.IsNullOrEmpty(this.pxsQueryVariable.StructureId))
                {
                    tmpElim = meta.Config.Codes.EliminationN;
                }
                else
                {
                    PXSqlValueSet vs = this.ValueSets[selectedValueset];
                    NumberOfValuesInValuesets.Add(vs.NumberOfValues);
                    tmpElim = vs.Elimination;
                }
            }
            else
            {
                PXSqlValueSet vs = this.ValueSets[selectedValueset];
                NumberOfValuesInValuesets.Add(vs.NumberOfValues);
                tmpElim = vs.Elimination;
            }





            if (tmpElim == meta.Config.Codes.EliminationN || tmpElim.Length == 0)
            {

                if (!this.isSelected)
                {

                    throw new PCAxis.Sql.Exceptions.PxsException(11, this.Name);

                }
                else
                {

                    this.PaxiomElimination = PXConstant.NO;
                }
            }
            else if (tmpElim == meta.Config.Codes.EliminationA)
            {
                if (this.isSelected)
                {
                    // We have to compare values in the valuepool(s) with the values selected in the PxsFile

                    if (this.Values.Count == NumberOfValuesInValuesets[0])
                    {

                        this.PaxiomElimination = PXConstant.YES;
                    }
                    else
                    {

                        this.PaxiomElimination = PXConstant.NO;
                    }

                }
            }
            else
            { // An elimination value exist for the variable.
                if (this.isSelected)
                {
                    if (this.Values.TryGetValue(tmpElim, out mValue))
                    { // the elimination value is selected

                        this.PaxiomElimination = mValue.ValueCode;
                    }



                    else
                    { // The Elimination value is not selected.  Elimination in Paxiom should be NO.

                        this.PaxiomElimination = PXConstant.NO;
                    }

                }
                // If an elimiantion value exists and no values are selected for the variable, the elimination
                // value should be used when selecting data, and metadata should be marked as eliminated by value.
                else
                {
                    mValue = new PXSqlValue();
                    mValue.ValueCode = tmpElim;

                    this.Values.Add(mValue.ValueCode, mValue);
                    this.IsEliminatedByValue = true;
                }
            }

        }
        private string CheckMultValuesetElim(List<string> ElimValues)
        {
            int numberOfElimA = 0;
            int numberOfElimN = 0;
            int numberOfElimOtherCode = 0;
            string elimOtherCode = "";
            if (ElimValues.Count == 1)
            {
                return ElimValues[0];
            }

            foreach (string elimval in ElimValues)
            {
                if (elimval == meta.Config.Codes.EliminationA)
                {
                    numberOfElimA++;
                }
                else if (elimval == meta.Config.Codes.EliminationN || String.IsNullOrEmpty(elimval))
                {
                    numberOfElimN++;
                }
                else
                {
                    numberOfElimOtherCode++;
                    elimOtherCode = elimval;
                }

            }

            //jfi sep 2015. This:
            //if ((numberOfElimA == ElimValues.Count - 1) && (numberOfElimOtherCode == 1))
            //was found to be too strict in VariablerOgVerdimengder-4.doc
          
            if ((numberOfElimOtherCode == 1))
            {
                return elimOtherCode;
            }
            else
            {
                return meta.Config.Codes.EliminationN;
            }
        }





        internal override List<PXSqlValue> GetValuesForParsing()
        {
            if (currentGrouping != null)
            {
                return currentGrouping.GetValuesForParsing();
            }

            if ((meta.inPresentationModus) && meta.ConstructedFromPxs)
            {
                return mValues.GetValuesSortedByPxs(mValues.GetValuesForSelectedValueset(selectedValueset));
                //return GetValuesSortedDefault(GetValuesForSelectedValueset()); // old sorting Thomas say its how Old Pcaxis does
            }
            else
            {
                PXSqlValueSet tmpValueSet = mValueSets[selectedValueset];
                List<PXSqlValue> myOut = new List<PXSqlValue>(tmpValueSet.NumberOfValues);
                foreach (string code in tmpValueSet.SortedListOfCodes)
                    myOut.Add(mValues[code]);
                return myOut;

            }
        }


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

        internal List<PXSqlGroup> GetGroupsForParsing()
        {
            if (currentGrouping == null)
                throw new ApplicationException("BUG!");

            return currentGrouping.GetGroupsForParsing();
        }



        internal override void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage)
        {
            base.ParseMeta(handler, LanguageCodes, preferredLanguage);
            if (this.isSelected)
            {

                // DOMAIN
                ParseDomain(handler, LanguageCodes);

                //MAP
                ParseMap(handler);

                //  VALUESET_X
                ParseValueSetKeywords(handler, LanguageCodes);

            }

            // ELIMINATION
            ParseElimination(handler, preferredLanguage);

            //GROUPING  (only for selected and selectionMode)
            if (this.groupingInfos != null)
            {
                this.groupingInfos.ParseMeta(handler);
            }

        }

        /// <PXKeyword name="DOMAIN">
        ///   <rule>
        ///     <description>Deviates from the standard languagehandeling which would be to read the ValuePool column of secondary language table. Doamin is read 
        ///     from column ValuePoolEng(2.0) or ValuePoolAlias (later).  </description>
        ///     <table modelName ="ValuePool">
        ///     <column modelName="ValuePool"/>
        ///     </table>
        ///     <table modelName ="ValuePool(secondary language)">
        ///     <column modelName="ValuePoolEng(2.0)"/>
        ///     <column modelName="ValuePoolAlias(later)"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        private void ParseDomain(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes)
        {
            StringCollection values = new StringCollection();
            string subkey = this.Name;

            foreach (string langCode in LanguageCodes)
            {
                values.Clear();
                values.Add(this.ValuePool.Domain[langCode]);
                handler(PXKeywords.DOMAIN, langCode, subkey, values);
            }

        }


        /// <PXKeyword name="MAP">
        ///   <rule>
        ///     <description> </description>
        ///     <table modelName ="ValueSet">
        ///       <column modelName="GeoAreaNo"/>
        ///     </table>     
        ///     <table modelName ="Grouping">
        ///       <column modelName="GeoAreaNo"/>
        ///     </table>
        ///     <table modelName ="GroupingLevel">
        ///       <column modelName="GeoAreaNo"/>
        ///     </table>
        ///     <table modelName ="TextCatalogt">
        ///       <column modelName="PresText (of main language)"/>
        ///     </table>     
        ///   </rule>
        /// </PXKeyword>
        private void ParseMap(PCAxis.Paxiom.IPXModelParser.MetaHandler handler)
        {
            StringCollection values = new StringCollection();
            string subkey = this.Name;
            string noLanguage = null;

            if (this.PaxiomMap != null)
            {
                values.Clear();
                values.Add(this.PaxiomMap);
                handler(PXKeywords.MAP, noLanguage, subkey, values);
            }
        }




        /// <PXKeyword name="VALUESET_ID">
        ///   <rule>
        ///     <description> </description>
        ///     <table modelName ="ValueSet">
        ///     <column modelName="ValueSet"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="VALUESET_NAME">
        ///   <rule>
        ///     <description> </description>
        ///     <table modelName ="ValueSet">
        ///     <column modelName="PresText"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        private void ParseValueSetKeywords(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes)
        {
            StringCollection values = new StringCollection();
            string subkey = this.Name;
            string noLanguage = null;
            bool parseValueSet;
            if (meta.inPresentationModus)
            {
                parseValueSet = true;
            }else
            {
                if ((this.ValueSets.Values.Count > 1) || (this.groupingInfos.Infos.Count > 0))
                {
                    parseValueSet = true;
                }else
                {
                    parseValueSet = false;
                }

            }


            if (parseValueSet)
            {
                foreach (PXSqlValueSet valueSet in this.ValueSets.Values)
                {
                    values.Add(valueSet.ValueSet);
                }
                handler(PXKeywords.VALUESET_ID, noLanguage, subkey, values);

                foreach (string langCode in LanguageCodes)
                {
                    values.Clear();

                    foreach (PXSqlValueSet valueSet in this.ValueSets.Values)
                    {
                        values.Add(valueSet.PresText[langCode]);
                    }
                    handler(PXKeywords.VALUESET_NAME, langCode, subkey, values);
                }

            }
        }

        /// <PXKeyword name="ELIMINATION">
        ///   <rule>
        ///     <description>Is set directly in paxiom.</description>
        ///     <table modelName ="ValueSet">
        ///     <column modelName="Elimination"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        internal void ParseElimination(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string preferredLanguage)
        {
            string subkey = this.Name;
            string noLanguage = null;
            StringCollection values = new StringCollection();
            if (this.isSelected) //29.6.2010 This keyword should only be sent if the variable is selected.
            {
                if (this.PaxiomElimination == PXConstant.YES)
                {
                    if (this.ValueSets[selectedValueset].Elimination == meta.Config.Codes.EliminationA)
                    {
                        foreach (PXSqlContent pxsqlCont in meta.Contents.Values)
                        {
                            if (!pxsqlCont.AggregPossible)
                            {
                                this.PaxiomElimination = PXConstant.NO;
                                break;
                            }
                        }
                    }
                }

                values.Clear();
                values.Add(this.PaxiomElimination);
                handler(PXKeywords.ELIMINATION, noLanguage, subkey, values);
            }

        }


        internal void ParseForApplyValueSet(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage)
        {
            string subkey = this.Name;

            StringCollection values = new StringCollection();
            // ELIMINATION
            ParseElimination(handler, preferredLanguage);
            //PresText
            base.ParsePresTextOption(handler, LanguageCodes, preferredLanguage);
            //Codes and values
            base.ParseCodeAndValues(handler, LanguageCodes, preferredLanguage);

        }
    }

}