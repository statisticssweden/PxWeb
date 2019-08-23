namespace PCAxis.Sql.Parser_22
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;
    using PCAxis.Sql.QueryLib_22;
    using PCAxis.Paxiom;
    using log4net;

    public class PXSqlGrouping
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlGrouping));

        private StringCollection mValuesetIds;
        StringCollection tempValueList = new StringCollection();
        public string Hierarchies;


        private GroupingIncludesType mIncludeType;
        public GroupingIncludesType IncludeType
        {
            get { return mIncludeType; }
            //set { mIncludeType = value; }
        }

        private string mValuePoolId;
        public string ValuePoolId
        {
            get { return mValuePoolId; }
            set { mValuePoolId = value; }
        }

        private string mGroupingId;
        public string GroupingId
        {
            get { return mGroupingId; }
            set { mGroupingId = value; }
        }

        private Dictionary<string, string> mPresText;
        public Dictionary<string, string> PresText
        {
            get { return mPresText; }
            set { mPresText = value; }
        }
        private string mDescription;
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        private string mGroupPres;
        public string GroupPres
        {
            get { return mGroupPres; }
            set { mGroupPres = value; }
        }


        internal bool isHierarchy;

        private string mHierarchy;
        public string Hierarchy
        {
            get { return mHierarchy; }
            set { mHierarchy = value; }
        }
        private string mSortCode;
        public string SortCode
        {
            get { return mSortCode; }
            set { mSortCode = value; }
        }
        private string mIsSelected;
        public string IsSelected
        {
            get { return mIsSelected; }
            set { mIsSelected = value; }
        }

        private List<PXSqlGroup> mGroups;
        public List<PXSqlGroup> Groups
        {
            get { return mGroups; }
            set { mGroups = value; }
        }

        private string mHierarchyLevelsOpen;
        public string HierarchyLevelsOpen
        {
            get { return mHierarchyLevelsOpen; }
            set { mHierarchyLevelsOpen = value; }
        }

        private string mHierarchyLevels;
        public string HierarchyLevels
        {
            get { return mHierarchyLevels; }
            set { mHierarchyLevels = value; }
        }


        private Dictionary<string, List<string>> mHierarchyNames;
        public Dictionary<string, List<string>> HierarchyNames
        {
            get { return mHierarchyNames; }
            set { mHierarchyNames = value; }
        }

        private PXSqlMeta_22 meta;
        private PXSqlVariableClassification variable;
        private string valuePoolValueTextExists;
        private GroupingIncludesType mDefaultIncludeType;
        private string levelOne = "1";
        private PXSqlGroup rootGroup;
        //private string hierarchiesKeywordValue;
        private StringCollection hierarchiesKeywordValue;


        /// <summary>
        /// Constructor called when parser is in selectionmode and there are no grouping ids in pxs or no pxs at all.
        /// </summary>
        /// <returns></returns>
        public PXSqlGrouping(GroupingRow groupingRow, PXSqlMeta_22 meta, PXSqlVariableClassification var, GroupingIncludesType include)
        {

            Init(groupingRow, meta, var);
            //TODO overriding include from paxiom, have to discuss how paxion can override database default.
            this.mIncludeType = include;
            // DONE, is now passed to paxiom as part of GroupingInfo ... this.mIncludeType = this.mDefaultIncludeType;
            //TODO end
            tempValueList = new StringCollection();
            StringCollection tempParentList = new StringCollection();
            mGroups = new List<PXSqlGroup>();
            PXSqlGroup tmpGroup = null;
            this.isHierarchy = this.mHierarchy != meta.Config.Codes.HierarchyNon;
            if (!isHierarchy)
            // Not hierarchical groups
            {
                this.mIncludeType = include;
                List<ValueGroupRow> myValuegroupRows = meta.MetaQuery.GetValueGroupRowsSorted(mGroupingId, null, true, meta.MainLanguageCode);

                foreach (ValueGroupRow myRow in myValuegroupRows)
                {
                    if (!tempParentList.Contains(myRow.GroupCode))
                    {
                        tmpGroup = new PXSqlGroup(myRow.GroupCode);
                        mGroups.Add(tmpGroup);
                        tempParentList.Add(myRow.GroupCode);
                    }
                    else
                    {
                        foreach (PXSqlGroup group in mGroups)
                        {
                            if (group.ParentCode == myRow.GroupCode)
                            {
                                tmpGroup = group;
                                break;
                            }
                        }
                    }
                    tmpGroup.AddChildCode(myRow.ValueCode);
                }
                AddValues(mValuePoolId, tempParentList, valuePoolValueTextExists);
            }
            else
            //hierarchical groups
            {
                this.mIncludeType = GroupingIncludesType.All;
                createHierarchy(meta.MetaQuery.GetValueGroupRowsSorted(mGroupingId, levelOne, true, meta.MainLanguageCode));
                setHierarchyLevelsOpen();
                setHierarchyLevels();
                setHierarchyNames();
                variable.Values.Clear();
                AddValues(mValuePoolId, tempValueList, valuePoolValueTextExists);
            }
        }





        //for presentation without grouping id in from pxs
        public PXSqlGrouping(PXSqlMeta_22 meta, PXSqlVariableClassification var, List<PXSqlGroup> groupFromPxs)
        {

            this.meta = meta;
            this.variable = var;
            this.valuePoolValueTextExists = var.ValuePool.ValueTextExists;
            this.mValuePoolId = var.ValuePool.ValuePool;
            this.mGroups = groupFromPxs;

            StringCollection parentCodes = new StringCollection();
            foreach (PXSqlGroup group in groupFromPxs)
            {
                parentCodes.Add(group.ParentCode);

            }
            AddValues(mValuePoolId, parentCodes, valuePoolValueTextExists);
        }

        //for selection or presentation with grouping id in from pxs
        public PXSqlGrouping(GroupingRow groupingRow, PXSqlMeta_22 meta, PXSqlVariableClassification var, StringCollection outputCodes)
        {
            Init(groupingRow, meta, var);


            StringCollection tempParentList = new StringCollection();
            mGroups = new List<PXSqlGroup>();
            PXSqlGroup tmpGroup = null;
            this.isHierarchy = this.mHierarchy != meta.Config.Codes.HierarchyNon;
            foreach (string code in outputCodes)
            {
                tmpGroup = new PXSqlGroup(code);
                mGroups.Add(tmpGroup);
            }

            foreach (PXSqlGroup group in mGroups)
            {

                //TODO; 2.2 bytt VSGroupRow
                // throw new NotImplementedException("i PXSqlGrouping");

                Dictionary<string, ValueGroupRow> templist = meta.MetaQuery.GetValueGroupRowskeyValueCode(mGroupingId, group.ParentCode, true);
                if (templist.Count > 0)
                {
                    foreach (KeyValuePair<string, ValueGroupRow> row in templist)
                    {
                        group.AddChildCode(row.Key);
                    }
                }
                else
                {
                    group.AddChildCode(group.ParentCode);
                }

                //List<VSGroupRow> tempList = meta.MetaQuery.GetVSGroupRow(mValuePoolId, mValuesetIds, mGroupingId,group.ParentCode);
                //if (tempList.Count > 0)
                //{
                //    foreach(VSGroupRow row in tempList)
                //    {                
                //    group.AddChildCode(row.ValueCode);
                //    }
                //}
                //else
                //{
                //    group.AddChildCode(group.ParentCode);
                //}
            }
            // Add the values to valuecollection of this variable
            AddValues(mValuePoolId, outputCodes, valuePoolValueTextExists);
        }


        /// <summary>
        /// Returns a List of PXSqlGroup with one PXSqlGroup for each output "row" 
        /// ( if children is to be displayed, they must be included as their own parent)
        /// The Group must be "expanded" in the sense that the children codes must "point" to rows in the datadata-table.
        /// This is used only when a Sum is used to calculate values. 
        /// </summary>
        internal List<PXSqlGroup> GetGroupsForDataParsing()
        {
            if (!this.isOnNonstoredData())
            {
                throw new ApplicationException("Grouping BUG");
            }

            if (this.isHierarchy)
            {
                List<PXSqlGroup> myOut = new List<PXSqlGroup>();
                foreach (PXSqlGroup unExpanded in mGroups)
                {
                    PXSqlGroup tmpGroup = new PXSqlGroup(unExpanded.ParentCode);
                    this.addLeafChildIds(tmpGroup, unExpanded.ParentCode);
                    myOut.Add(tmpGroup);
                }
                return myOut;
            }
            else
            {
                //The non-hierarchy has no levels to expand
                return mGroups;
            }
        }


        /// <summary>
        /// Recursive method drilling down to the leafs 
        /// </summary>
        /// <param name="toGroup">Add the leaf IDs to this</param>
        /// <param name="parentCode">Check if this has children</param>
        private void addLeafChildIds(PXSqlGroup toGroup, String parentCode)
        {
            Dictionary<string, ValueGroupRow> templist = meta.MetaQuery.GetValueGroupRowskeyValueCode(mGroupingId, parentCode, true);
            if (templist.Count < 1)
            {
                //Has no children
                toGroup.AddChildCode(parentCode);
            }
            else
            {
                foreach (KeyValuePair<string, ValueGroupRow> childOfparent in templist)
                {

                    addLeafChildIds(toGroup, childOfparent.Key);
                }

            }

        }

        /// <summary>
        /// Creates Hierarchical groups
        /// </summary>
        /// <returns></returns>
        private void createHierarchy(List<ValueGroupRow> mGroupRows)
        {
            int counter;
            PXSqlGroup myGoup = null;
            if (mGroupRows.Count < 1)
            {
            }
            else
            {
                counter = 0;
                foreach (ValueGroupRow row in mGroupRows)
                {
                    if (counter == 0)
                    {
                        myGoup = new PXSqlGroup(row.GroupCode);
                        myGoup.GroupLevel = int.Parse(row.GroupLevel);
                        mGroups.Add(myGoup);
                        if (row.GroupLevel == "1")
                        {
                            tempValueList.Add(row.GroupCode);
                            rootGroup = myGoup;
                        }
                    }
                    myGoup.AddChildCode(row.ValueCode);
                    tempValueList.Add(row.ValueCode);
                    List<ValueGroupRow> templist = new List<ValueGroupRow>();
                    foreach (KeyValuePair<string, ValueGroupRow> tempdict in meta.MetaQuery.GetValueGroupRowskeyValueCode(mGroupingId, row.ValueCode, true))
                    {
                        templist.Add(tempdict.Value);
                    }
                    if (templist.Count > 0)
                    {
                        createHierarchy(templist);
                    }
                    counter++;

                }

            }

        }

        public StringCollection getHierarchyForParsing()
        {
            hierarchiesKeywordValue = new StringCollection();
            hierarchiesKeywordValue.Add("\"" + rootGroup.ParentCode + "\"");
            CreateHierarchyForParsing(rootGroup);
            return hierarchiesKeywordValue;
        }

        public void CreateHierarchyForParsing(PXSqlGroup group)
        {
            foreach (string childCode in group.ChildCodes)
            {
                //hierarchiesKeywordValue  += "\"" + group.ParentCode + "\"" + ":" + "\"" + childCode + "\"," ;
                hierarchiesKeywordValue.Add("\"" + group.ParentCode + "\"" + ":" + "\"" + childCode + "\"");
                foreach (PXSqlGroup childGroup in mGroups)
                {
                    if (childGroup.ParentCode == childCode)
                    {
                        CreateHierarchyForParsing(childGroup);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Recursive method to receive all values for all groups in a grouping.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="childrenlist"></param>
        /// <returns></returns>
        public List<PXSqlValue> GetHierarchicalValues(PXSqlGroup group, List<PXSqlValue> childrenlist)
        {
            if (group.GroupLevel == 1)
            {
                childrenlist.Add(variable.Values[group.ParentCode]);
            }
            foreach (string childCode in group.ChildCodes)
            {
                childrenlist.Add(variable.Values[childCode]);
                foreach (PXSqlGroup childGroup in mGroups)
                {
                    if (childGroup.ParentCode == childCode)
                    {
                        GetHierarchicalValues(childGroup, childrenlist);
                    }
                }
            }
            return childrenlist;
        }

        public List<PXSqlValue> GetValuesForParsing()
        {
            if (this.meta.inSelectionModus && !this.meta.ConstructedFromPxs)
            {
                return GetValuesForParsingWhenSelection();
            }
            else
            {
                return GetValuesForParsingWhenPresentation();
            }

        }

        private List<PXSqlValue> GetValuesForParsingWhenPresentation()
        {
            List<PXSqlValue> tempValuesList = new List<PXSqlValue>();
            foreach (PXSqlGroup group in this.mGroups)
            {
                tempValuesList.Add(this.variable.Values[group.ParentCode]);
            }
            return tempValuesList;
        }

        private List<PXSqlValue> GetValuesForParsingWhenSelection()
        {
            PXSqlVariable var = variable;
            List<PXSqlValue> tempValuesList = new List<PXSqlValue>();
            if (this.isHierarchy)
            {

                tempValuesList = GetHierarchicalValues(rootGroup, tempValuesList);
            }
            else
            {
                if (this.mIncludeType == GroupingIncludesType.AggregatedValues)
                {
                    foreach (PXSqlGroup group in this.mGroups)
                    {

                        tempValuesList.Add(var.Values[group.ParentCode]);   //todo; sortert etter gruppe sorteringskode

                    }
                }
                else if (this.mIncludeType == GroupingIncludesType.SingleValues)
                {
                    foreach (PXSqlGroup group in this.mGroups)
                    {
                        foreach (string childCode in group.ChildCodes)
                            tempValuesList.Add(var.Values[childCode]);   //todo; sortert etter gruppe sorteringskode
                    }
                }
                else if (this.IncludeType == GroupingIncludesType.All)
                {

                    foreach (PXSqlGroup group in this.mGroups)
                    {

                        tempValuesList.Add(var.Values[group.ParentCode]);   //todo; sortert etter gruppe sorteringskode
                        foreach (string childCode in group.ChildCodes)
                        {
                            tempValuesList.Add(var.Values[childCode]);   //todo; sortert etter gruppe sorteringskode
                        }
                    }

                }
                //tempValuesList.Sort(PXSqlValue.SortByVsValue());


                //TODO; Hvorfor hadde Linja under en   if ( this.meta.MetaQuery.metaVersionLE("2.1")) { rundt seg
                tempValuesList.Sort(PXSqlValue.SortByValue());
            }

            return tempValuesList;
        }


        private void Init(GroupingRow groupingRow, PXSqlMeta_22 meta, PXSqlVariableClassification var)
        {

            this.meta = meta;

            this.variable = var;
            this.valuePoolValueTextExists = var.ValuePool.ValueTextExists;
            this.mValuePoolId = groupingRow.ValuePool;
            this.mValuesetIds = var.ValusetIds;
            this.mGroupingId = groupingRow.Grouping;
            this.mGroupPres = groupingRow.GroupPres;
            // throw new NotImplementedException("I init i PXSqlGrouping");
            //TODO; tok bort this.mGeoAreaNo = groupingRow.GeoAreaNo;
            //           this.mGeoAreaNo = groupingRow.GeoAreaNo;
            this.mHierarchy = groupingRow.Hierarchy;
            this.Description = groupingRow.Description;
            this.PresText = new Dictionary<string, string>();
            foreach (string langCode in meta.LanguageCodes)
            {
                this.PresText[langCode] = groupingRow.texts[langCode].PresText;
            }
            this.SortCode = groupingRow.texts[meta.MainLanguageCode].SortCode;
            //TODO added databasedefault to override value from paxiom, should be possible to do both
            switch (this.mGroupPres.ToUpper())
            {
                case "I":
                    mDefaultIncludeType = GroupingIncludesType.SingleValues;
                    break;
                case "A":
                    mDefaultIncludeType = GroupingIncludesType.AggregatedValues;
                    break;
                case "B":
                    mDefaultIncludeType = GroupingIncludesType.All;
                    break;
                default:
                    mDefaultIncludeType = GroupingIncludesType.AggregatedValues;
                    break;
            }
        }



        /// <summary>
        /// Get values from valuetable in database an add them to the the variables valuecollection
        /// </summary>
        /// <param name="valuePoolId"></param>
        /// <param name="valueList"></param>
        /// <param name="valuePoolValueTextExists"></param>
        private void AddValues(string valuePoolId, StringCollection valueList, string valuePoolValueTextExists)
        {
            PXSqlValue tempValue;
            foreach (ValueRow row in this.meta.MetaQuery.GetValueRowsByValuePool(valuePoolId, valueList, valuePoolValueTextExists))
            {

                tempValue = new PXSqlValue(row, meta.LanguageCodes, meta.MainLanguageCode);
                if (this.variable.Values.ContainsKey(tempValue.ValueCode))
                {
                    log.Debug("Already contains code =" + tempValue.ValueCode);
                }
                else
                {
                    this.variable.Values.Add(tempValue.ValueCode, tempValue);
                }
            }
        }

        /// <summary>
        /// Prepares a (Paxiom) Grouping with id, and if GroupingIncludesType = All parent-child relations.
        /// </summary>
        /// <returns></returns>
        internal Grouping GetPaxiomGrouping()
        {
            //            if (this.isHierarchy)
            //            {
            //            }
            //            else
            //           {
            Grouping paxGrouping = new Grouping();
            paxGrouping.Name = this.GroupingId;
            paxGrouping.ID = this.GroupingId;
         //   if (this.mIncludeType.Equals(GroupingIncludesType.All))
            {
                foreach (PXSqlGroup group in this.mGroups)
                {
                    Group paxGroup = new Group();
                    paxGroup.GroupCode = group.ParentCode;//the Name is in the value list(leaves paxGroup.Name empty)
                    List<GroupChildValue> groupChildValueList = new List<GroupChildValue>();
                    foreach (String childCode in group.ChildCodes)
                    {
                        GroupChildValue paxGCV = new GroupChildValue();
                        paxGCV.Code = childCode;
                        groupChildValueList.Add(paxGCV);
                    }
                    paxGroup.ChildCodes = groupChildValueList;
                    paxGrouping.Groups.Add(paxGroup);
                }

            }
            //}
            return paxGrouping;
        }


        /// <summary>
        /// True if nonHierarchycal or allLevelsStored = N
        /// </summary>
        /// <returns>Returns TRUE if group is nonhierarchical or AllLevelsStored=N in MainTableVariableHierarchy. Else return False. </returns>
        internal bool isOnNonstoredData()
        {
            if (!isHierarchy)
            {
                return true;
            }
            else
            {
                return (((meta.MetaQuery.GetMainTableVariableHierarchyRow(meta.MainTable.MainTable, variable.Name, GroupingId).AllLevelsStored).ToUpper() == "N"));
            }
        }

        /// <summary>
        /// Sets number of hierarchylevels to be initially opened. 
        /// </summary>
        private void setHierarchyLevelsOpen()
        {
            mHierarchyLevelsOpen = meta.MetaQuery.GetMainTableVariableHierarchyRow(meta.MainTable.MainTable, variable.Name, GroupingId).ShowLevels;
        }

        /// <summary>
        /// Sets number of hierarchylevels in the grouping
        /// </summary>
        private void setHierarchyLevels()
        {
            mHierarchyLevels = meta.MetaQuery.GetValueGroupMaxValueLevel(GroupingId, true);
        }

        /// <summary>
        /// Sets the names of the hierarchical levels for each languages.
        /// </summary>
        private void setHierarchyNames()
        {
            HierarchyNames = new Dictionary<string, List<string>>();
            Dictionary<string, GroupingLevelRow> GetGroupingLevelRows = meta.MetaQuery.GetGroupingLevelRows_KeyIsLevel(GroupingId, true);

            foreach (KeyValuePair<string, GroupingLevelRow> myRow in GetGroupingLevelRows)
            {
                foreach (KeyValuePair<string, GroupingLevelTexts> levelText in myRow.Value.texts)
                {
                    if (!HierarchyNames.ContainsKey(levelText.Key))
                    {
                        HierarchyNames.Add(levelText.Key, new List<string>());
                    }
                    HierarchyNames[levelText.Key].Add(levelText.Value.LevelText);
                }
            }

        }
    }
}
