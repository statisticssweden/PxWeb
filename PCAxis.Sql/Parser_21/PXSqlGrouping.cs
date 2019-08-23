namespace PCAxis.Sql.Parser_21
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;
    using PCAxis.Sql.QueryLib_21;
    using PCAxis.Paxiom;
    using log4net;

    public class PXSqlGrouping
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlGrouping));

        private StringCollection mValuesetIds;

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

        private Dictionary<string,string> mPresText;
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
        private string mGeoAreaNo;
        public string GeoAreaNo
        {
            get { return mGeoAreaNo; }
            set { mGeoAreaNo = value; }
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
        private PXSqlMeta_21 meta;
        private PXSqlVariableClassification variable;
        private string valuePoolValueTextExists;

        
        private GroupingIncludesType mDefaultIncludeType;



        //for Selection without grouping id in from pxs
        public PXSqlGrouping(GroupingRow groupingRow, PXSqlMeta_21 meta,PXSqlVariableClassification var,GroupingIncludesType include)
        {

            Init(groupingRow, meta, var);
            //TODO overriding include from paxiom, have to discuss how paxion can override database default.
            this.mIncludeType = include;
          // DONE, is now passed to paxiom as part of GroupingInfo ...  this.mIncludeType = this.mDefaultIncludeType;  
            //TODO end
            StringCollection tempParentList = new StringCollection();
            mGroups = new List<PXSqlGroup>();
            PXSqlGroup tmpGroup = null;
            foreach (VSGroupRow myRow in meta.MetaQuery.GetVSGroupRowsSorted(mValuePoolId, mValuesetIds, mGroupingId,meta.MainLanguageCode))
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

            // Add the values to valuecollection of this variable
            AddValues(mValuePoolId, tempParentList, valuePoolValueTextExists);

            //PXSqlValue tempValue;
            //    foreach (ValueRow row in meta.MetaQuery.GetValueRowsByValuePool(mValuePoolId, tempParentList, valuePoolValueTextExists))
            //{
            //    tempValue = new PXSqlValue(row, meta.LanguageCodes, meta.MainLanguageCode);
            //    var.Values.Add(tempValue.ValueCode,tempValue);
            //}
        }


        //for presentation without grouping id in from pxs
        public PXSqlGrouping(PXSqlMeta_21 meta, PXSqlVariableClassification var, List<PXSqlGroup> groupFromPxs)
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
        public PXSqlGrouping(GroupingRow groupingRow, PXSqlMeta_21 meta,PXSqlVariableClassification var, StringCollection outputCodes ) 
        {
            Init(groupingRow, meta, var);
            StringCollection tempParentList = new StringCollection();
            mGroups = new List<PXSqlGroup>();
            PXSqlGroup tmpGroup = null;
            foreach (string code in outputCodes)
            {
                tmpGroup = new PXSqlGroup(code);
                mGroups.Add(tmpGroup);
            }

            foreach (PXSqlGroup group in mGroups)
            {
                List<VSGroupRow> tempList = meta.MetaQuery.GetVSGroupRow(mValuePoolId, mValuesetIds, mGroupingId,group.ParentCode);
                if (tempList.Count > 0)
                {
                    foreach(VSGroupRow row in tempList)
                    {                
                    group.AddChildCode(row.ValueCode);
                    }
                }
                else
                {
                    group.AddChildCode(group.ParentCode);
                }
            }
            // Add the values to valuecollection of this variable
            AddValues(mValuePoolId,outputCodes, valuePoolValueTextExists);
        }


        /// <summary>
        /// Returns a List of PXSqlGroup with one PXSqlGroup for each output "row" 
        /// ( if children is to be displayed, they must be included as their own parent)
        /// </summary>
        internal List<PXSqlGroup> GetGroupsForParsing()
        {
            return mGroups;
        }

        public List<PXSqlValue> GetValuesForParsing()
        {
            if (this.meta.inSelectionModus && ! this.meta.ConstructedFromPxs)
            {
                return GetValuesForParsingWhenSelection();
            } else
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
            if (this.mIncludeType == GroupingIncludesType.AggregatedValues)
            {
                foreach (PXSqlGroup group in this.mGroups)
                {
                    
                    tempValuesList.Add(var.Values[group.ParentCode]);   //todo; sortert etter gruppe sorteringskode
                }
            } else if (this.mIncludeType == GroupingIncludesType.SingleValues)
            {
                foreach (PXSqlGroup group in this.mGroups)
                {
                    foreach (string childCode in group.ChildCodes)
                        tempValuesList.Add(var.Values[childCode]);   //todo; sortert etter gruppe sorteringskode
                }
            } else if (this.IncludeType == GroupingIncludesType.All)
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

       //     if ( this.meta.MetaQuery.metaVersionLE("2.0")) {
     if ( this.meta.MetaQuery.metaVersionLE("2.1")) {

                tempValuesList.Sort(PXSqlValue.SortByValue());
            }

            return tempValuesList;
        }


        private void Init(GroupingRow groupingRow, PXSqlMeta_21 meta, PXSqlVariableClassification var)
        {

            this.meta = meta;
            this.variable = var;
            this.valuePoolValueTextExists = var.ValuePool.ValueTextExists;
            this.mValuePoolId = groupingRow.ValuePool;
            this.mValuesetIds = var.ValusetIds;
            this.mGroupingId = groupingRow.Grouping;
            this.mGroupPres = groupingRow.GroupPres;
            this.mGeoAreaNo = groupingRow.GeoAreaNo;
            this.PresText = new Dictionary<string, string>();
            this.Description = groupingRow.Description; 
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
        /// Get values from valuetabel in database an add them to the the variables valuecollection
        /// </summary>
        /// <param name="valuePoolId"></param>
        /// <param name="valueList"></param>
        /// <param name="valuePoolValueTextExists"></param>
        private void AddValues(string valuePoolId, StringCollection valueList, string valuePoolValueTextExists)
        {
            PXSqlValue tempValue;
            foreach (ValueRow row in this.meta.MetaQuery.GetValueRowsByValuePool(valuePoolId, valueList, valuePoolValueTextExists))
            {
                
                tempValue = new PXSqlValue(row,meta.LanguageCodes, meta.MainLanguageCode);
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
            Grouping paxGrouping = new Grouping();
            paxGrouping.Name = this.GroupingId;
            paxGrouping.ID = this.GroupingId;
         //  if(this.mIncludeType.Equals(GroupingIncludesType.All))  // removed for test advanced grouping px-web
            {
                foreach (PXSqlGroup group in this.mGroups)
                {
                    Group paxGroup = new Group();
                    paxGroup.GroupCode = group.ParentCode;//the Name is in the value list(leaves paxGroup.Name empty)
                   //paxGroup.Name = this.variable.Values[group.ParentCode].ValueTextL[meta.MainLanguageCode]; //todo piv test
                    List<GroupChildValue> groupChildValueList = new List<GroupChildValue>();
                    foreach (String childCode in group.ChildCodes)
                    {
                        GroupChildValue paxGCV = new GroupChildValue();
                        paxGCV.Code = childCode;
                      //  paxGCV.Name = this.variable.Values[childCode].ValueTextL[meta.MainLanguageCode]; // todo test piv
                        groupChildValueList.Add(paxGCV);
                    }
                    paxGroup.ChildCodes = groupChildValueList;
                    paxGrouping.Groups.Add(paxGroup);
                }

            }
            return paxGrouping;
        }


    }
}
