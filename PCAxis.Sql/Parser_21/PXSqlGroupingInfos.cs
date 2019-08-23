namespace PCAxis.Sql.Parser_21
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using PCAxis.Paxiom;
    using log4net;

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

        /// <summary>Initializes a new instance of the PXSqlGroupingInfos class, and fills the list of PXSqlGroupingInfo</summary>
        /// <param name="meta">Holds the state of the builder</param>
        /// <param name="variableName">The variable which has these groupings.</param>
        /// <param name="valueSetIds">The valueSets that apply to this builder </param>
        internal PXSqlGroupingInfos(PXSqlMeta_21 meta, string variableName, StringCollection valueSetIds)
        {
            foreach ( PCAxis.Sql.QueryLib_21.GroupingRow gRow in meta.MetaQuery.GetRelevantGroupingRows(valueSetIds))
            {
                infos.Add(new PXSqlGroupingInfo(gRow,meta.Config.Codes));
            }
            this.variableName = variableName;
        }

       

      //  internal List<PXSqlGroupingInfo> GetGroupsSorted(List<PXSqlGroupingInfo> groupingInfoList,string langCode)
       // {
           // List<PXSqlGroupingInfo> sortedGrouposList = new List<PXSqlGroupingInfo>();
           // foreach (PXSqlGroupingInfo group in groups)
           // {
           //     sortedGrouposList.Add(group);
           // }
           //return groupingInfoList.Sort(PXSqlGroupingInfo.SortGroupingInfo(langCode));
            //sortedGrouposList.Sort(PXSqlGroupingInfo.SortGroupingInfo(langCode));
            //return sortedGrouposList;

       // }


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
