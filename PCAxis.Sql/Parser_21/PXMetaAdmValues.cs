using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_21;
using log4net;
using PCAxis.Sql.DbConfig;

namespace PCAxis.Sql.Parser_21 {
    public class PXMetaAdmValues {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXMetaAdmValues));

        #region fields & properties

        private string _PXCodepage = "iso-8859-1"; //default may be overwritten
        public string PXCodepage {
            get { return _PXCodepage; }
        }

        private string _PXCharset = "ANSI"; //default may be overwritten
        public string PXCharset {
            get { return _PXCharset; }

        }

        private string _PXAxisVersion = "2000"; //default may be overwritten
        public string PXAxisVersion {
            get { return _PXAxisVersion; }

        }

        //private string _DefaultCodeMissingLine;
        //public string DefaultCodeMissingLine {
        //    get { return _DefaultCodeMissingLine; }
        //}


        private bool _PXDescriptionDefault = false; //default may be overwritten
        public bool PXDescriptionDefault {
            get { return _PXDescriptionDefault; }
        }

        private bool _AllwaysUseMaintablePrestextSInDynamicTitle = false; //default may be overwritten
        internal bool AllwaysUseMaintablePrestextSInDynamicTitle
        {
            get { return _AllwaysUseMaintablePrestextSInDynamicTitle; }
        }
       

        #endregion fields & properties

        public PXMetaAdmValues(Dictionary<string, MetaAdmRow> altIBasen, SqlDbConfig_21 dbConfig) {
            SqlDbConfig_21.DbKeywords Keywords = dbConfig.Keywords;




            //PXCodepage
            if (Keywords.Optional_PXCodepage != null) {
                if (altIBasen.ContainsKey(Keywords.Optional_PXCodepage)) {
                    _PXCodepage = altIBasen[Keywords.Optional_PXCodepage].Value;
                } else {
                    throw new ApplicationException("Keyword:" + Keywords.Optional_PXCodepage + " not found in table with modelname MetaAdm.");
                }
            }

            //PXDescriptionDefault
            if (Keywords.Optional_PXDescriptionDefault != null) {
                if (altIBasen.ContainsKey(Keywords.Optional_PXDescriptionDefault)) {
                    _PXDescriptionDefault = altIBasen[Keywords.Optional_PXDescriptionDefault].Value.Equals(dbConfig.Codes.Yes);
                } else {
                    throw new ApplicationException("Keyword:" + Keywords.Optional_PXDescriptionDefault + " not found in table with modelname MetaAdm.");
                }
            }
            //AllwaysUseMaintablePrestextSInDynamicTitle
            if (Keywords.Optional_AllwaysUseMaintablePrestextSInDynamicTitle != null)
            {
                if (altIBasen.ContainsKey(Keywords.Optional_AllwaysUseMaintablePrestextSInDynamicTitle))
                {
                    _AllwaysUseMaintablePrestextSInDynamicTitle = altIBasen[Keywords.Optional_AllwaysUseMaintablePrestextSInDynamicTitle].Value.Equals(dbConfig.Codes.Yes);
                }
                else
                {
                    throw new ApplicationException("Keyword:" + Keywords.Optional_PXDescriptionDefault + " not found in table with modelname MetaAdm.");
                }
            }

            //PXCharset
            if (Keywords.Optional_PXCharset != null) {
                if (altIBasen.ContainsKey(Keywords.Optional_PXCharset)) {
                    _PXCharset = altIBasen[Keywords.Optional_PXCharset].Value;
                } else {
                    throw new ApplicationException("Keyword:" + Keywords.Optional_PXCharset + " not found in table with modelname MetaAdm.");
                }
            }

            //PXAxisVersion
            if (Keywords.Optional_PXAxisVersion != null) {
                if (altIBasen.ContainsKey(Keywords.Optional_PXAxisVersion)) {
                    _PXAxisVersion = altIBasen[Keywords.Optional_PXAxisVersion].Value;
                } else {
                    throw new ApplicationException("Keyword:" + Keywords.Optional_PXAxisVersion + " not found in table with modelname MetaAdm.");
                }
            }

            //if(altIBasen.ContainsKey("DefaultCodeMissingLine")){
            //    _DefaultCodeMissingLine = altIBasen["DefaultCodeMissingLine"].Value;
            //} else {
            //    log.Error(" Keyword:\"DefaultCodeMissingLine\" not found in table with modelname MetaAdm.");
                
            //}
        }
    }
}
