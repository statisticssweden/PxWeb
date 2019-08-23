using System;
using System.Collections.Specialized;
using System.Collections.Generic;

using PCAxis.Paxiom;

using log4net;

namespace PCAxis.PlugIn.Sql
{
    /// <summary>
    /// Just ships a string, string dictionary to paxiom 
    /// </summary>
    public class PXSqlParserForDataCellNote : IDisposable, PCAxis.Paxiom.IPXModelParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParserForDataCellNote));


        private Dictionary<string, string> theDataNoteCellEntries;

        /// <summary>
        /// Receives the data and stores it.
        /// </summary>
        /// <param name="inDataNoteCellEntries">the data</param>
        public PXSqlParserForDataCellNote(Dictionary<string, string> inDataNoteCellEntries)
        {
            theDataNoteCellEntries = inDataNoteCellEntries;
        }



        #region ParseMeta
        ///
        /// <PXKeyword name="DATANOTECELL">
        ///   <rule>
        ///     <description>Sends codes from SpecialCharacter when found in datatables.</description>
        ///     <table modelName ="Maintable">
        ///     <column modelName="SpecCharExists"/>
        ///     </table>
        ///     <table modelName ="SpecialCharacter">
        ///     <column modelName="all"/>
        ///     </table>
        ///     <table modelName ="The datatables">
        ///       <column modelName="NPM columns and missing rows"/>
        ///     </table>
        ///   </rule>   
        /// </PXKeyword>
        /// 
        public void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string preferredLanguage)
        {

            string keyword;

            string noLanguage = null;

            StringCollection values;

            foreach (KeyValuePair<string, string> dcn in theDataNoteCellEntries)
            {
                keyword = PXKeywords.DATANOTECELL;
                values = new StringCollection();
                values.Add(dcn.Value);
                String myKey = dcn.Key.Replace(",", "\",\"");//for PXModelBuilder.SplittString   A","B","C 
                // not "A","B","C" 
                handler(keyword, noLanguage, myKey, values);

            }

        }

        #endregion

        #region ParseData
        public void ParseData(IPXModelParser.DataHandler handler, int preferredBufferSize)
        {
            throw new ApplicationException("BUG");
        }


        #endregion

        #region IDisposable implemenatation
        public void Dispose()
        {
            theDataNoteCellEntries = null;
        }
        #endregion
    }
}


