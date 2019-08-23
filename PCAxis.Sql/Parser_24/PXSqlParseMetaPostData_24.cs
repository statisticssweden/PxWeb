using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

using PCAxis.Paxiom;
using PCAxis.Sql.QueryLib_24;

using log4net;
using System.Text;

namespace PCAxis.Sql.Parser_24
{
    /// <summary>
    /// Just ships a string, string dictionary to paxiom 
    /// </summary>
    public class PXSqlParseMetaPostData_24 : IDisposable, PCAxis.Paxiom.IPXModelParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParseMetaPostData_24));


        private Dictionary<string, string> theDataNoteCellEntries;
        private string defaultAttributes;
        private Dictionary<string, StringCollection> attributeEntries;
        private PXSqlAttributes attributes;
        private StringCollection languages;
        private IEnumerable<PXSqlNpm.NPMCharacter> usedNPMCharacters;

        private PXSqlMeta_24 mMeta;

        /// <summary>
        /// Receives the data and stores it.
        /// </summary>
        /// <param name="inDataNoteCellEntries">the data</param>
        internal PXSqlParseMetaPostData_24(Dictionary<string, string> inDataNoteCellEntries, Dictionary<string, StringCollection> attributeEntries, string defaultAttributes, PXSqlAttributes attributes, StringCollection languages, IEnumerable<PXSqlNpm.NPMCharacter> usedNPMCharacters, PXSqlMeta_24 meta)
        {
            theDataNoteCellEntries = inDataNoteCellEntries;
            this.defaultAttributes = defaultAttributes;
            this.attributeEntries = attributeEntries;
            this.attributes = attributes;
            this.languages = languages;
            this.usedNPMCharacters = usedNPMCharacters;

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

            string keyword;

            string noLanguage = null;

            StringCollection values;

            //Add not notes if NPM charavters has been used
            if (this.usedNPMCharacters.Count() > 0)
            {
                foreach (var language in languages)
                {
                    foreach (var usedNPMCharacter in this.usedNPMCharacters)
                    {
                        var presCharacterNPM = usedNPMCharacter.presCharacters[language];
                        var presTextNPM = usedNPMCharacter.presTexts[language];

                        if (!string.IsNullOrEmpty(presCharacterNPM) && !string.IsNullOrEmpty(presTextNPM))
                        {
                            string npmCharacterExplanation = presCharacterNPM + " = " + presTextNPM;

                            StringCollection npmValues = new StringCollection();
                            npmValues.Add(npmCharacterExplanation);

                            string subKeyWord = null; //In order to show it

                            handler(PXKeywords.NOTE, language, subKeyWord, npmValues);
                        }
                    }
                }
            }

            /*
            Code below is commented out because it did not work.   
            */

            /*
            foreach (KeyValuePair<string, string> dcn in theDataNoteCellEntries)
            {
                keyword = PXKeywords.DATANOTECELL;
                values = new StringCollection();
                values.Add(dcn.Value);
                String myKey = dcn.Key.Replace(",", "\",\"");//for PXModelBuilder.SplittString   A","B","C 
                // not "A","B","C" 
                handler(keyword, noLanguage, myKey, values);

            }
            */

            /*
                Code below for PXKeywords.DATANOTECELL is copied from 2.1 because it works
            */

            PXSqlNpm myNpms = mMeta.mPxsqlNpm;
            StringCollection datanoteDistictValues = new StringCollection();
            keyword = PXKeywords.DATANOTECELL;
            
            foreach (KeyValuePair<string, string> dcn in theDataNoteCellEntries)
            {

                PXSqlNpm.NPMCharacter myNpm = myNpms.GetNpmBySpeciaCharacterType(dcn.Value);
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
            foreach (string datanoteDistinctValue in datanoteDistictValues)
            {
                PXSqlNpm.NPMCharacter myNpm = myNpms.GetNpmBySpeciaCharacterType(datanoteDistinctValue);
                foreach (string lang in mMeta.LanguageCodes)
                {
                    string presText = myNpm.presCharacters[lang] + " = " + myNpm.presTexts[lang];
                    values = new StringCollection();
                    values.Add(presText);
                    handler(keyword, lang, null, values);
                }
            }

            if (attributes.HasAttributes)
            {
                //ATTRIBUTE-ID
                keyword = PXKeywords.ATTRIBUTE_ID;
                values = new StringCollection();
                foreach (AttributeRow attr in attributes.SortedAttributes.Values)
                {
                    values.Add(attr.Attribute);
                }
                handler(keyword, noLanguage, null, values);


                //ATTRIBUTE-TEXT
                keyword = PXKeywords.ATTRIBUTE_TEXT;
                values = new StringCollection();

                foreach (string lang in languages)
                {
                    values.Clear();
                    foreach (AttributeRow attr in attributes.SortedAttributes.Values)
                    {
                        values.Add(attr.texts[lang].PresText);
                    }
                    handler(keyword, lang, null, values);
                }

                // Default ATTRIBUTES

                noLanguage = null;
                keyword = PXKeywords.ATTRIBUTES;

                values = new StringCollection();
                string[] tmpStrings = defaultAttributes.Split(':');
                foreach (string tmpString in tmpStrings)
                {
                    values.Add(tmpString);
                }
                handler(keyword, noLanguage, null, values);
                //ATTRIBUTES
                foreach (KeyValuePair<string, StringCollection> att in attributeEntries)
                {
                    if (att.Key != defaultAttributes)
                    {
                        values = new StringCollection();
                        string myKey = "";
                        tmpStrings = att.Key.Split(':');
                        foreach (string tmpString in tmpStrings)
                        {
                            values.Add(tmpString);
                        }
                        foreach (string position in att.Value)
                        {
                            myKey = position.Replace(",", "\",\"");
                            handler(keyword, noLanguage, myKey, values);
                        }
                    }

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


