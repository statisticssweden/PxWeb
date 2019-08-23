using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PCAxis.Paxiom;
using PCAxis.Sql.QueryLib_22;
using System.Globalization;
using System.Collections.Specialized;

namespace PCAxis.Sql.Parser_22
{

    /// <summary>
    /// Takes care of the special characters.
    /// </summary>
    internal class PXSqlNpm
    {
        private PXSqlMeta_22 mMeta;


        private Dictionary<string, NPMCharacter> myNPMCharacters;

        private Dictionary<string, string> SpecialCharacterIDbyMetaAdmProprety;

        //private int cat1Counter; you may have as many category1 SpecialCharacters as you like
        private int cat2Counter;  //only one of these. Absolute zero.
        private int cat3Counter;  // 6 of theseor 7 if dataSymbolSum is not one of the six    


        //internal const string dataNoteSumKeyWord = "DATANOTESUM";
        //mMeta.Config.Keywords.DataNoteSum
        //private const string dataSymbolNIL = "DATASYMBOLNIL";

        //private const string dataSymbolSum = "DATASYMBOLSUM";

        //private const string defaultCodeMissingLine = "DEFAULTCODEMISSINGLINE";



        private NPMCharacter dataNoteSumNPMCharacter;
        private NPMCharacter dataNotAvailableNPMCharacter;
        private NPMCharacter defaultCodeMissingLineNPMCharacter;
        private NPMCharacter dataSymbolNilNPMCharacter;
        private NPMCharacter dataSymbolSumNPMCharacter;

        private int maxDatasymbolN = 0;


        internal PXSqlNpm(PXSqlMeta_22 mMeta)
        {
            this.mMeta = mMeta;


            cat2Counter = 0;
            cat3Counter = 0;

            SpecialCharacterIDbyMetaAdmProprety = new Dictionary<string, string>();

            //DataNotAvailable
            SpecialCharacterIDbyMetaAdmProprety.Add(mMeta.Config.Keywords.DataNotAvailable, mMeta.PXMetaAdmValues.DataNotAvailable);
            //DATANOTESUM
            SpecialCharacterIDbyMetaAdmProprety.Add(mMeta.Config.Keywords.DataNoteSum, mMeta.PXMetaAdmValues.DataNoteSum);

            //DATASYMBOLNIL
            SpecialCharacterIDbyMetaAdmProprety.Add(mMeta.Config.Keywords.DataSymbolNIL, mMeta.PXMetaAdmValues.DataSymbolNIL);

            //DATASYMBOLSUM
            SpecialCharacterIDbyMetaAdmProprety.Add(mMeta.Config.Keywords.DataSymbolSum, mMeta.PXMetaAdmValues.DataSymbolSum);

            //DEFAULTCODEMISSINGLINE
            SpecialCharacterIDbyMetaAdmProprety.Add(mMeta.Config.Keywords.DefaultCodeMissingLine, mMeta.PXMetaAdmValues.DefaultCodeMissingLine);

            // assign properties for specialcharacters

            setNpmCharacters();
            setMetaAdm();
            maxDatasymbolN = cat3Counter;

        }


        /// <summary>
        /// Creates NPMCharacters from the SpecialCharacterRows and counts them by category when needes.
        /// </summary>
        private void setNpmCharacters()
        {
            Dictionary<string, SpecialCharacterRow> specialCharacterRows = mMeta.MetaQuery.GetSpecialCharacterAllRows();
            myNPMCharacters = new Dictionary<string, NPMCharacter>();


            foreach (KeyValuePair<string, SpecialCharacterRow> myRow in specialCharacterRows)
            {
                NPMCharacter myNPMCharacter = new NPMCharacter(myRow, mMeta);

                // Category 1 
                if (myNPMCharacter.category == 1)
                {
                    //cat1Counter++;
                }

                // category 2
                else if (myNPMCharacter.category == 2)
                {
                    myNPMCharacter.pcAxisCode = PcAxisCodes.DataSymbol_NIL;
                    cat2Counter++;
                    if (cat2Counter > 1)
                    {
                        throw new PCAxis.Sql.Exceptions.DbException(15);
                    }
                }
                // category 3
                else if (myNPMCharacter.category == 3)
                {
                    // check to se if any of the character is the same as the one specified for DataSymbolSum.
                    if (myNPMCharacter.characterType == SpecialCharacterIDbyMetaAdmProprety[mMeta.Config.Keywords.DataSymbolSum])
                    {
                        myNPMCharacter.pcAxisCode = PcAxisCodes.DataSymbol_Sum;
                        //could have done dataSymbolSumNPMCharacter = myNPMCharacter; , but 
                        // for the others it is done in setMetaAdm() so I put it there for this as well. 
                    }
                    else
                    {
                        cat3Counter++;
                        if (cat3Counter > 6)
                        {
                            throw new PCAxis.Sql.Exceptions.DbException(16);
                        }
                        else
                        {
                            myNPMCharacter.dataSymbolNr = cat3Counter;
                            myNPMCharacter.pcAxisCode = PcAxisCodes.getDataSymbolNo(cat3Counter);
                        }
                    }
                }
                else
                {
                    throw new PCAxis.Sql.Exceptions.DbException(17);
                }

                myNPMCharacters.Add(myNPMCharacter.characterType, myNPMCharacter);

            }
        }


        /// <summary>
        /// Checks, for the SpecialCharacters that are "extra Special, that is, that are (must be) given a meaning in MetaAdm
        /// that the category is acceptable for meaning, and assigns the correct NPMCharacter to the "meaningNPMCharacter" fields
        /// </summary>
        private void setMetaAdm()
        {
            string idToCheck;
            NPMCharacter npmChar;
            // DefaultCodeMissingLine
            //defaultCodeMissingLine The value for defaultCodeMissingLine should be specified in SpecialCharacter and 
            // should be of category 2 or 3:
            // check if variables was found in the NPM table.

            idToCheck = SpecialCharacterIDbyMetaAdmProprety[mMeta.Config.Keywords.DefaultCodeMissingLine];

            if (!myNPMCharacters.ContainsKey(idToCheck))
            {
                throw new PCAxis.Sql.Exceptions.DbException(18, mMeta.Config.Keywords.DefaultCodeMissingLine, idToCheck);
            }

            npmChar = myNPMCharacters[idToCheck];

            if ((npmChar.category == 2) || (npmChar.category == 3))
            {
                defaultCodeMissingLineNPMCharacter = npmChar;
            }
            else
            {
                throw new PCAxis.Sql.Exceptions.DbException(20, mMeta.Config.Keywords.DefaultCodeMissingLine, idToCheck, npmChar.category, "2 or 3");
            }



            //DataSymbolNil
            //DatasymbolNil should be of category 2
            idToCheck = SpecialCharacterIDbyMetaAdmProprety[mMeta.Config.Keywords.DataSymbolNIL];

            if (!myNPMCharacters.ContainsKey(idToCheck))
            {
                throw new PCAxis.Sql.Exceptions.DbException(18, mMeta.Config.Keywords.DataSymbolNIL, idToCheck);
            }

            npmChar = myNPMCharacters[idToCheck];

            if (npmChar.category == 2)
            {
                dataSymbolNilNPMCharacter = npmChar;
            }
            else
            {
                throw new PCAxis.Sql.Exceptions.DbException(20, mMeta.Config.Keywords.DataSymbolNIL, idToCheck, npmChar.category, "2");
            }


            // DataNotAvailable  sjekker at koden fra metaadm finnes i SpesChar med riktig category
            // DataNotAvailable should be of category 3 
            idToCheck = SpecialCharacterIDbyMetaAdmProprety[mMeta.Config.Keywords.DataNotAvailable];
            if (!myNPMCharacters.ContainsKey(idToCheck))
            {
                throw new PCAxis.Sql.Exceptions.DbException(18, mMeta.Config.Keywords.DataNotAvailable, idToCheck);
            }
            npmChar = myNPMCharacters[idToCheck];

            if (npmChar.category == 3)
            {
                dataNotAvailableNPMCharacter = npmChar;
            }
            else
            {
                throw new PCAxis.Sql.Exceptions.DbException(20, mMeta.Config.Keywords.DataNotAvailable, idToCheck, npmChar.category, "3");
            }

            // DataNoteSum sjekker at koden fra metaadm finnes i SpesChar med riktig category
            idToCheck = SpecialCharacterIDbyMetaAdmProprety[mMeta.Config.Keywords.DataNoteSum];
            if (!myNPMCharacters.ContainsKey(idToCheck))
            {
                throw new PCAxis.Sql.Exceptions.DbException(18, mMeta.Config.Keywords.DataNoteSum, idToCheck);
            }
            npmChar = myNPMCharacters[idToCheck];
            if (npmChar.category == 1)
            {
                dataNoteSumNPMCharacter = npmChar;
            }
            else
            {
                throw new PCAxis.Sql.Exceptions.DbException(20, mMeta.Config.Keywords.DataNoteSum, idToCheck, npmChar.category, "1");
            }

            //DataSymbolSum
            idToCheck = SpecialCharacterIDbyMetaAdmProprety[mMeta.Config.Keywords.DataSymbolSum];
            if (!myNPMCharacters.ContainsKey(idToCheck))
            {
                throw new PCAxis.Sql.Exceptions.DbException(18, mMeta.Config.Keywords.DataSymbolSum, idToCheck);
            }
            npmChar = myNPMCharacters[idToCheck];
            if (npmChar.category == 3)
            {
                dataSymbolSumNPMCharacter = npmChar;
            }
            else
            {
                throw new PCAxis.Sql.Exceptions.DbException(20, mMeta.Config.Keywords.DataSymbolSum, idToCheck, npmChar.category, "3");
            }
        }


        public class NPMCharacter
        {

            /// <summary>
            /// This is the primary key of the SpecialCharacter table. 
            /// </summary>
            public string characterType;
            public Dictionary<string, string> presCharacters;
            public Dictionary<string, string> presTexts;
            public int dataSymbolNr;
            public int category;
            public double pcAxisCode;


            public NPMCharacter(KeyValuePair<string, SpecialCharacterRow> myRow, PXSqlMeta_22 mMeta)
            {
                //this.id = npmId;
                this.characterType = myRow.Value.CharacterType;
                this.presCharacters = new Dictionary<string, string>();
                this.presTexts = new Dictionary<string, string>();
                foreach (string lang in mMeta.LanguageCodes)
                {
                    this.presCharacters[lang] = myRow.Value.texts[lang].PresCharacter;
                    this.presTexts[lang] = myRow.Value.texts[lang].PresText;
                }

                if (myRow.Value.DataCellPres == mMeta.Config.Codes.Yes && myRow.Value.AggregPossible == mMeta.Config.Codes.Yes)
                {
                    this.category = 1;
                }
                else if (myRow.Value.DataCellPres == mMeta.Config.Codes.No && myRow.Value.AggregPossible == mMeta.Config.Codes.Yes)
                {
                    this.category = 2;
                }
                else if (myRow.Value.DataCellPres == mMeta.Config.Codes.No && myRow.Value.AggregPossible == mMeta.Config.Codes.No)
                {
                    this.category = 3;
                }
            }


        }


        /// <summary>
        /// magic Double values for telling "this is a spesial character" to paxiom
        /// </summary>
        private struct PcAxisCodes
        {

            public static double getDataSymbolNo(int symbolNo)
            {
                switch (symbolNo)
                {
                    case 1:
                        return PcAxisCodes.DataSymbol_1;
                    case 2:
                        return PcAxisCodes.DataSymbol_2;
                    case 3:
                        return PcAxisCodes.DataSymbol_3;
                    case 4:
                        return PcAxisCodes.DataSymbol_4;
                    case 5:
                        return PcAxisCodes.DataSymbol_5;
                    case 6:
                        return PcAxisCodes.DataSymbol_6;
                    default:
                        throw new PCAxis.Sql.Exceptions.DbException(26);
                }
            }
            internal static double getDatasymbolNil()
            {
                return DataSymbol_NIL;
            }
            internal static double getDatasymbolSum()
            {
                return DataSymbol_Sum;
            }
            public const double DataSymbol_1 = -1.0E+19;
            public const double DataSymbol_2 = -1.0E+20;
            public const double DataSymbol_3 = -1.0E+21;
            public const double DataSymbol_4 = -1.0E+22;
            public const double DataSymbol_5 = -9.9999999991E+22;
            public const double DataSymbol_6 = -1.0E+24;
            public const double DataSymbol_NIL = -1.0E+25;
            public const double DataSymbol_Sum = -9999999999999.9;
        }

        internal NPMCharacter DefaultCodeMissingLineNPMCharacter
        {
            get
            {
                return defaultCodeMissingLineNPMCharacter;
            }
        }


        // "Public" properties
        internal double DefaultCodeMissingLineMagic
        {
            get
            {
                return defaultCodeMissingLineNPMCharacter.pcAxisCode;
            }
        }



        internal double DataNotAvailableMagic
        {
            get
            {
                return dataNotAvailableNPMCharacter.pcAxisCode;
            }
        }



        private string DataSymbolNilPresChar(string lang)
        {

            return dataSymbolNilNPMCharacter.presCharacters[lang];

        }

        internal double DataSymbolSumMagic
        {
            get
            {
                return PcAxisCodes.DataSymbol_Sum;
            }
        }

        private string DataSymbolSumPresChar(string lang)
        {

            return dataSymbolSumNPMCharacter.presCharacters[lang];
        }




        internal double DataSymbolNMagic(string characterType)
        {
            if (myNPMCharacters.ContainsKey(characterType))
            {
                NPMCharacter myChar = myNPMCharacters[characterType];

                return myChar.pcAxisCode;
            }
            throw new PCAxis.Sql.Exceptions.DbException(31, characterType);

        }

        private string DataSymbolNPresChar(int symbolNo, string lang)
        {
            foreach (NPMCharacter myChar in myNPMCharacters.Values)
            {
                if (myChar.dataSymbolNr == symbolNo)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(32, symbolNo);
        }

        internal string DataNoteSumCharacterType()
        {

            return dataNoteSumNPMCharacter.characterType;
        }


        internal int GetCategory(double theMagicNumber)
        {
            foreach (NPMCharacter myChar in myNPMCharacters.Values)
            {
                if (myChar.pcAxisCode == theMagicNumber)
                    return myChar.category;
            }
            throw new PCAxis.Sql.Exceptions.DbException(34, theMagicNumber.ToString());
        }


        public NPMCharacter GetNpmBySpeciaCharacterType(string characterType)
        {
            foreach (NPMCharacter myCharacter in myNPMCharacters.Values)
            {
                if (myCharacter.characterType == characterType)
                {
                    return myCharacter;
                }
            }
            throw new ApplicationException("Special character with charactertype= " + characterType + " does not exists");
        }



        public void ParseMeta(IPXModelParser.MetaHandler handler, PXSqlMeta_22 mPXSqlMeta)
        {

            string keyword;

            string noLanguage = null;
            string subkey = null;
            StringCollection values;

            if (mPXSqlMeta.inPresentationModus)
            {

                // NPM Characters
                //DataNoteSum
                {
                    subkey = null;
                    keyword = PXKeywords.DATANOTESUM;
                    foreach (string langCode in mPXSqlMeta.LanguageCodes)
                    {
                        values = new StringCollection();
                        values.Add(this.DataSymbolSumPresChar(langCode));
                        handler(keyword, langCode, subkey, values);
                    }
                    values = null;
                }
                //DataSymbolN
                {

                    for (int i = 1; i <= this.maxDatasymbolN; i++)
                    {
                        subkey = null;
                        values = new StringCollection();
                        keyword = "DATASYMBOL" + i.ToString();
                        values.Add(this.DataSymbolNPresChar(i, mPXSqlMeta.LanguageCodes[0]));
                        handler(keyword, noLanguage, subkey, values);
                        values = null;
                    }
                }
                //DataSymbolNil
                {
                    subkey = null;
                    values = new StringCollection();
                    keyword = PXKeywords.DATASYMBOLNIL;
                    values.Add(this.DataSymbolNilPresChar(mPXSqlMeta.LanguageCodes[0]));
                    handler(keyword, noLanguage, subkey, values);
                    values = null;
                }
                //DataSymbolSum
                {
                    subkey = null;
                    values = new StringCollection();
                    keyword = PXKeywords.DATASYMBOLSUM;
                    values.Add(this.DataSymbolSumPresChar(mPXSqlMeta.LanguageCodes[0]));
                    handler(keyword, noLanguage, subkey, values);
                    values = null;
                }
            }
        }
    }
}


