namespace PCAxis.Sql.Parser_24
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using PCAxis.Paxiom;
    using log4net;
    using PCAxis.Sql.QueryLib_24;

    /// <summary>
    /// Holds the information on which groupings may be used with this variable. This applies to selected variables when build4selection.
    /// </summary>
    public class PXSqlGroupingInfos
    {


        /// <summary>The logger to log to. (Stylecop wants documentation)</summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlGroupingInfos));

        /// <summary>List of groupings</summary>
        private List<PXSqlGroupingInfo> infos = new List<PXSqlGroupingInfo>();

        /// <summary>
        /// The name = id of the variable which has these groupings.  
        /// </summary>
        private string variableName;
        public List<PXSqlGroupingInfo> Infos
        {
            get { return infos; }
        }

        
        private StringCollection groupingIDs;

        
        /// <summary>
        /// the IDs of the groupings which may be used for the variable. 
        /// </summary>
        internal StringCollection GroupingIDs
        {
            get { return groupingIDs; }
        } 

        /// <summary>Initializes a new instance of the PXSqlGroupingInfos class, and fills the list of PXSqlGroupingInfo</summary>
        /// <param name="meta">Holds the state of the builder</param>
        /// <param name="variableName">The variable which has these groupings.</param>
        /// <param name="valueSetIds">The valueSets that apply to this builder </param>
        internal PXSqlGroupingInfos(PXSqlMeta_24 meta, string variableName, StringCollection valueSetIds)
        {
            this.variableName = variableName;
            // finding the IDs of the grouping which may be used for this 
            var valueSetIdsByGroupingId = new Dictionary<string, List<string>>();
            groupingIDs = new StringCollection();

            foreach (string valueSetId in valueSetIds)
            {
                foreach (string groupingId in meta.MetaQuery.GetValueSetGroupingRowskeyGrouping(valueSetId, true).Keys) {
                    groupingIDs.Add(groupingId);

                    if (!valueSetIdsByGroupingId.ContainsKey(groupingId))
                    {
                        valueSetIdsByGroupingId[groupingId] = new List<string>();
                    }

                    valueSetIdsByGroupingId[groupingId].Add(valueSetId);
                }
            }

            // finding the ID's of hierarchical groups
            foreach (string groupingId in meta.MetaQuery.GetMainTableVariableHierarchyRows_KeyIsGroupingID(meta.MainTable.MainTable,variableName,true).Keys) {
                       groupingIDs.Add(groupingId);
            }

            
            //Må sortere groupingIDs 
            List<GroupingRow> sortedGroupingRows = meta.MetaQuery.GetGroupingRowsSorted(groupingIDs, false, meta.MainLanguageCode);

            // creating Info for them;
            foreach (GroupingRow groupingRow in sortedGroupingRows)
            {
                infos.Add(new PXSqlGroupingInfo(groupingRow, meta.Config.Codes, valueSetIdsByGroupingId[groupingRow.Grouping]));
            }
        }

    


        /// <summary>Sends the list of Grouping Info to paxiom</summary>
        /// <param name="handler">The paxiom feeder</param>
        /// <PXKeyword name="GROUPING_ID (incerted directly in paxiom)">
        ///   <rule>
        ///     <description>(incerted directly in paxiom, not via the BuilderAdapter)</description>
        ///     <table modelName ="Grouping">
        ///     <column modelName="Grouping"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="GROUPING_NAME">
        ///   <rule>
        ///     <description>(incerted directly in paxiom, not via the BuilderAdapter)</description>
        ///     <table modelName ="Grouping">
        ///     <column modelName="PresText"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        internal void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler)
        {
            if (this.infos.Count > 0)
            {
                string subkey = this.variableName;
                StringCollection values = new StringCollection();

                //tried with sorting on each language, but skipped it. Use sort in sql where added sort on prestext first language in addition to sortcode
                //List<PXSqlGroupingInfo> sortedGroupingInfoList;
                foreach (string langCode in this.infos[0].PresTextByLanguage.Keys)
                {
                //    sortedGroupingInfoList = new List<PXSqlGroupingInfo>();
                //    foreach (PXSqlGroupingInfo info in this.infos)
                //    {
                //        sortedGroupingInfoList.Add(info);
                //    }
                //    sortedGroupingInfoList.Sort(PXSqlGroupingInfo.SortGroupingInfo(langCode));

                    
                    values.Clear();
                    //foreach (PXSqlGroupingInfo info in sortedGroupingInfoList)
                    foreach (PXSqlGroupingInfo info in this.infos)
                    {
                        values.Add(info.GroupingId);
                        log.Debug("Sending groupingID: " + info.GroupingId);
                    }

                    handler(PXKeywords.GROUPING_ID, langCode, subkey, values);
                    values.Clear();

                    //groupPres
                    foreach (PXSqlGroupingInfo info in this.infos)
                    {
                        values.Add(info.GroupPres);
                    }

                    handler(PXKeywords.GROUPING_GROUPPRES, langCode, subkey, values);

                    values.Clear();

                    //this.infos.Sort(PXSqlGroupingInfo.SortGroupingInfo(langCode));

                    //foreach (PXSqlGroupingInfo info in sortedGroupingInfoList)
                    foreach (PXSqlGroupingInfo info in this.infos)
                    {
                        values.Add(info.PresTextByLanguage[langCode]);
                    }

                    

                    handler(PXKeywords.GROUPING_NAME, langCode, subkey, values);
                }
            }
        }
    }
}
