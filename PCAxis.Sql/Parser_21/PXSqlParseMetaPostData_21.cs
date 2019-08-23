using System;
using System.Collections.Specialized;
using System.Collections.Generic;

using PCAxis.Paxiom;

using log4net;

namespace PCAxis.Sql.Parser_21
{
    /// <summary>
    /// Just ships a string, string dictionary to paxiom 
    /// </summary>
    public class PXSqlParseMetaPostData_21 : IDisposable, PCAxis.Paxiom.IPXModelParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParseMetaPostData_21));


        private Dictionary<string, string> theDataNoteCellEntries;
        private PXSqlMeta_21 mMeta;

        /// <summary>
        /// Receives the data and stores it.
        /// </summary>
        /// <param name="inDataNoteCellEntries">the data</param>
        public PXSqlParseMetaPostData_21(Dictionary<string, string> inDataNoteCellEntries,PCAxis.Sql.Parser_21.PXSqlMeta_21 meta)
        {
            theDataNoteCellEntries = inDataNoteCellEntries;
            mMeta = meta;
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
            PXSqlNpm myNpms = mMeta.mPxsqlNpm;
            StringCollection datanoteDistictValues = new StringCollection();
            string keyword = PXKeywords.DATANOTECELL;
            StringCollection values;
            foreach (KeyValuePair<string, string> dcn in theDataNoteCellEntries)
            {

               PXSqlNpm.NPMCharacter  myNpm = myNpms.GetNpmBySpeciaCharacterType(dcn.Value);
             foreach (string lang in mMeta.LanguageCodes)
             {
                 string presCharacter = myNpm.presCharacters[lang];
                   values = new StringCollection();
                   values.Add(presCharacter);
                   String myKey = dcn.Key.Replace(",", "\",\"");//for PXModelBuilder.SplittString   A","B","C 
                   // not "A","B","C" 
                   handler(keyword, lang, myKey, values);
               }
                // Keep distinct values of special character to get Presetext.
             if (!datanoteDistictValues.Contains(dcn.Value))
             {
                 datanoteDistictValues.Add(dcn.Value);
             }
            }

            keyword = PXKeywords.NOTE;
            foreach(string datanoteDistinctValue in datanoteDistictValues)
            {
                PXSqlNpm.NPMCharacter myNpm = myNpms.GetNpmBySpeciaCharacterType(datanoteDistinctValue);
                foreach (string lang in mMeta.LanguageCodes)
                {
                    string presText = myNpm.presCharacters[lang] + "=" + myNpm.presTexts[lang];
                    values = new StringCollection();
                    values.Add(presText);
                    handler(keyword, lang,null, values);                    
                }
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


