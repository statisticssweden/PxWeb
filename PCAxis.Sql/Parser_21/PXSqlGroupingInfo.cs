namespace PCAxis.Sql.Parser_21
{

    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public class PXSqlGroupingInfo
    {
        /// <summary>The logger to log to. (Stylecop wants documentation)</summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlGroupingInfos));
       
        // not used sorting is done in sql 
        public class SortGroupingInfoHelper : IComparer<PXSqlGroupingInfo>
        {
            private string lang;
            public SortGroupingInfoHelper(string lang)
            {
                this.lang = lang;
            }
            int IComparer<PXSqlGroupingInfo>.Compare(PXSqlGroupingInfo a, PXSqlGroupingInfo b)
            {
                PXSqlGroupingInfo valA = a;
                PXSqlGroupingInfo valB = b;
                return String.Compare(valA.sortCodeByLanguage[lang], valB.sortCodeByLanguage[lang]);
            }
        }
        
       
        /// <summary> Database unique ID</summary>
        private string groupingId;

        /// <summary>
        /// Kod som anger hur en gruppering ska presenteras, som ett aggregerat värde, ingående värden eller både och. Följande alternativ finns:
        /// A = Aggregerat värde skall visas
        /// I = Ingående värden skall visas
        /// B = Både aggregerat värde och ingående värden skall visas
        /// </summary>
        private string groupPres;
        
        /// <summary>The texts which may be displayed to users when selecting a grouping from a list of groupings</summary>
        private Dictionary<string, string> presTextByLanguage = new Dictionary<string,string>();

        /// <summary>Sort order in which the groupings are shown. If SortCode in grouping are set this will be used otherwise PresText</summary>
        private Dictionary<string, string> sortCodeByLanguage = new Dictionary<string, string>();

        internal PXSqlGroupingInfo(PCAxis.Sql.QueryLib_21.GroupingRow groupingRow, DbConfig.SqlDbConfig_21.Ccodes ccodes)
        {

            this.groupingId = groupingRow.Grouping;

            /*
             *A = Aggregerat värde skall visas
             *I = Ingående värden skall visas
             *B = Både aggregerat värde och ingående värden skall visas
            */

            if( groupingRow.GroupPres.Equals(ccodes.GroupPresA))
            {
                this.groupPres = "AggregatedValues";
            }
            else if(  groupingRow.GroupPres.Equals(ccodes.GroupPresI))
            {
                this.groupPres = "SingleValues";
            }
            else if(   groupingRow.GroupPres.Equals(ccodes.GroupPresB)){
                this.groupPres = "All";
            } else {
                log.Warn("Unknown GroupPres: \""+groupingRow.GroupPres+"\", so \"All\" all is used");
                log.WarnFormat("These are the GroupPres options: mother {0}, child {1} or all {2}", ccodes.GroupPresA, ccodes.GroupPresI, ccodes.GroupPresB);
                this.groupPres = "AggregatedValues";
            }

            
            
   

            foreach (KeyValuePair<string, PCAxis.Sql.QueryLib_21.GroupingTexts> text in groupingRow.texts)
            {
                this.presTextByLanguage[text.Key] = text.Value.PresText;
          //      if (text.Value.SortCode != "")
          //      {
          //          this.sortCodeByLanguage[text.Key] = text.Value.SortCode;
          //      }
          //      else
          //      {
          //          this.sortCodeByLanguage[text.Key] = text.Value.PresText;
          //      }
            }

        }

  


        /// <summary>Returns the (database) unique ID of the Grouping</summary>
        internal string GroupingId
        {
            get { return this.groupingId; }

        }

        /// <summary>Returns a char inducating how to present the Grouping</summary>
        internal string GroupPres
        {
            get { return this.groupPres; }
        }


        /// <summary>Returns the texts which may be displayed to users when selecting a grouping from a list of groupings</summary>
        internal IDictionary<string, string> PresTextByLanguage
        {
            get { return presTextByLanguage; }
        }
        //not used sorting is done in sql
        public static IComparer<PXSqlGroupingInfo> SortGroupingInfo(string lang)
        {
            return (IComparer<PXSqlGroupingInfo>)new SortGroupingInfoHelper(lang);
        }

    }
}
