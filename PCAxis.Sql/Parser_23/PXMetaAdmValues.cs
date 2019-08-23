using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_23;
using log4net;
using PCAxis.Sql.DbConfig;

namespace PCAxis.Sql.Parser_23 {

    /// <summary>
    /// Gets the values from the MetaAdm table or their defaults.
    /// </summary>
    public class PXMetaAdmValues {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXMetaAdmValues));

        #region fields & properties


        private string _PXCodepage = "iso-8859-1"; //default may be overwritten
        /// <summary>
        /// returns the PXCodepage from database (optional) or the default 
        /// </summary>
        internal string PXCodepage {
            get { return _PXCodepage; }
        }

        private string _PXCharset = "ANSI"; //default may be overwritten
        /// <summary>
        /// returns the PXCharset from database (optional) or the default 
        /// </summary>
        internal string PXCharset {
            get { return _PXCharset; }
        }

        private string _PXAxisVersion = "2000"; //default may be overwritten
        /// <summary>
        /// returns the PXAxisVersion from database (optional) or the default 
        /// </summary>
        internal string PXAxisVersion {
            get { return _PXAxisVersion; }

        }

        private bool _PXDescriptionDefault = false; //default may be overwritten
        /// <summary>
        /// returns the PXDescriptionDefault from database (optional) or the default 
        /// </summary>
        internal bool PXDescriptionDefault {
            get { return _PXDescriptionDefault; }
        }

        private bool _AllwaysUseMaintablePrestextSInDynamicTitle = false; //default may be overwritten
        /// <summary>
        /// returns the AllwaysUseMaintablePrestextSInDynamicTitle from database (optional) or the default 
        /// </summary>
        internal bool AllwaysUseMaintablePrestextSInDynamicTitle
        {
            get { return _AllwaysUseMaintablePrestextSInDynamicTitle; }
        }


        private string _DataNotAvailable;
        /// <summary>
        /// Gets the ID for DataNotAvailable
        /// </summary>
        internal string DataNotAvailable{
            get { return _DataNotAvailable; }
        }


        private string _DataNoteSum;
        /// <summary>
        /// Gets the ID for DataNoteSum
        /// </summary>
        internal string DataNoteSum
        {
            get { return _DataNoteSum; }
        }

        private string _DataSymbolNIL;
        /// <summary>
        /// Gets the ID for DataSymbolNIL
        /// </summary>
        internal string DataSymbolNIL
        {
            get { return _DataSymbolNIL; }
        }

        private string _DataSymbolSum;
        /// <summary>
        /// Gets the ID for DataSymbolSum
        /// </summary>
        internal string DataSymbolSum
        {
            get { return _DataSymbolSum; }
        }

        private string _DefaultCodeMissingLine;
        /// <summary>
        /// Gets the ID for DefaultCodeMissingLine
        /// </summary>
        internal string DefaultCodeMissingLine
        {
            get { return _DefaultCodeMissingLine; }
        }
       

        #endregion fields & properties

        /// <summary>
        /// Reads the values from the database.
        /// </summary>
        /// <param name="mMetaQuery">The handle of the database</param>
        public PXMetaAdmValues(MetaQuery mMetaQuery)
        {

            Dictionary<string, MetaAdmRow> altIBasen = mMetaQuery.GetMetaAdmAllRows();
            SqlDbConfig_23 dbConfig = mMetaQuery.DB;
            SqlDbConfig_23.DbKeywords Keywords = dbConfig.Keywords;


            if (altIBasen.ContainsKey(Keywords.DataNotAvailable))
            {
                _DataNotAvailable = altIBasen[Keywords.DataNotAvailable].Value;
             } 
             else 
             {
                 throw new ApplicationException("Keyword:" + Keywords.DataNotAvailable + " not found in table with modelname MetaAdm.");
             }

             if (altIBasen.ContainsKey(Keywords.DataNoteSum))
             {
                 _DataNoteSum = altIBasen[Keywords.DataNoteSum].Value;
             }
             else
             {
                 throw new ApplicationException("Keyword:" + Keywords.DataNoteSum + " not found in table with modelname MetaAdm.");
             }


             if (altIBasen.ContainsKey(Keywords.DataSymbolNIL))
             {
                 _DataSymbolNIL = altIBasen[Keywords.DataSymbolNIL].Value;
             }
             else
             {
                 throw new ApplicationException("Keyword:" + Keywords.DataSymbolNIL + " not found in table with modelname MetaAdm.");
             }

             if (altIBasen.ContainsKey(Keywords.DataSymbolSum))
             {
                 _DataSymbolSum = altIBasen[Keywords.DataSymbolSum].Value;
             }
             else
             {
                 throw new ApplicationException("Keyword:" + Keywords.DataSymbolSum + " not found in table with modelname MetaAdm.");
             }


             if (altIBasen.ContainsKey(Keywords.DefaultCodeMissingLine))
             {
                 _DefaultCodeMissingLine = altIBasen[Keywords.DefaultCodeMissingLine].Value;
             }
             else
             {
                 throw new ApplicationException("Keyword:" + Keywords.DefaultCodeMissingLine + " not found in table with modelname MetaAdm.");
             }


            //Optional


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


        }
        
        


 
    }
}
