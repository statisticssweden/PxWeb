using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PCAxis.Paxiom;
using PCAxis.Sql.QueryLib_21;
using System.Globalization;

namespace PCAxis.Sql.Parser_21
{
    public class PXSqlNpm
    {
        PXSqlMeta_21 mMeta;
        MetaAdmRow myMetaAdmRow;

        string metaVersion;
        //  Dictionary<int,NPMCharacter> myNPMCharacters;
        List<NPMCharacter> myNPMCharacters;
        Dictionary<string, MetaAdmVariable> myMetaAdmVariables;
        private int cat1Counter;
        private int cat2Counter;
        private int cat3Counter;

        private int npmId;
        private int dataSymbolSumNpmId;

        private const string dataNotAvailable = "DATANOTAVAILABLE";
        private const string dataNoteSum = "DATANOTESUM";
        private const string dataSymbolNIL = "DATASYMBOLNIL";
        private const string dataSymbolSum = "DATASYMBOLSUM";
        private const string defaultCodeMissingLine = "DEFAULTCODEMISSINGLINE";
        private bool dataSymbolNilSet = false;
        private bool dataNotAvailableSet = false;
        private bool dataSymbolSumSet = false;
        private bool dataNoteSumSet = false;
        private bool defaultCodeMissingLineSet = false;

        public int maxDatasymbolN = 0;

        
        public PXSqlNpm(PXSqlMeta_21 mMeta) {
            this.mMeta = mMeta;
            metaVersion = mMeta.MetaModelVersion;
            cat1Counter = 0;
            cat2Counter = 0;
            cat3Counter = 0;
            myMetaAdmVariables = new Dictionary<string, MetaAdmVariable>();
            if (metaVersion == "2.0") {
                NPMCharacter myNPMCharacter = new NPMCharacter();
                //myNPMCharacters = new Dictionary<int, NPMCharacter>();
                myNPMCharacters = new List<NPMCharacter>();
                myNPMCharacter.id = 0;
                myNPMCharacter.category = 3;
                //myNPMCharacter.pcAxisCode = PcAxisCodes.getDataSymbolNo(1); 
                myNPMCharacter.pcAxisCode = PcAxisCodes.getDataSymbolNo(2);
                //TODO; FIX   (tja 2.1)
                myNPMCharacter.presCharacters = new Dictionary<string, string>();
                myNPMCharacter.presTexts = new Dictionary<string, string>();
                foreach (string lang in mMeta.LanguageCodes) {
                    myNPMCharacter.presCharacters[lang] = "..";
                    myNPMCharacter.presTexts[lang] = "";
                }
                myNPMCharacters.Add(myNPMCharacter);

                // Set MetaAdmvariable for DataNotAvailable
                MetaAdmVariable myMetaVariable = new MetaAdmVariable();
                myMetaVariable.name = dataNotAvailable;
                myMetaVariable.value = "";
                myMetaVariable.npmRef = 0;
                myMetaAdmVariables.Add(myMetaVariable.name, myMetaVariable);
                //TODO; FIX  (tja 2.1)
                //myMetaVariable = new MetaAdmVariable();
                //myMetaVariable.name = dataSymbolSum;
                //myMetaVariable.value = "";
                //myMetaVariable.npmRef = 0;
                //myMetaAdmVariables.Add(myMetaVariable.name, myMetaVariable);

                //myMetaVariable = new MetaAdmVariable();
                //myMetaVariable.name = dataSymbolNIL;
                //myMetaVariable.value = "";
                //myMetaVariable.npmRef = 0;
                //myMetaAdmVariables.Add(myMetaVariable.name, myMetaVariable);

                //myMetaVariable = new MetaAdmVariable();
                //myMetaVariable.name = dataNoteSum;
                //myMetaVariable.value = "";
                //myMetaVariable.npmRef = 0;
                //myMetaAdmVariables.Add(myMetaVariable.name, myMetaVariable);
                //----------------
            }
            else if (String.Compare(metaVersion, "2.0", false, CultureInfo.InvariantCulture) > 0) {
                //if ((metaVersion == (decimal)2.1) || (metaVersion == (decimal)2.2))
                //DataNotAvailable
                myMetaAdmRow = mMeta.MetaQuery.GetMetaAdmRow(dataNotAvailable);
                setMetaAdmInfo1(myMetaAdmRow);
                //DATANOTESUM
                myMetaAdmRow = mMeta.MetaQuery.GetMetaAdmRow(dataNoteSum);
                setMetaAdmInfo1(myMetaAdmRow);
                //DATASYMBOLNIL
                myMetaAdmRow = mMeta.MetaQuery.GetMetaAdmRow(dataSymbolNIL);
                setMetaAdmInfo1(myMetaAdmRow);
                //DATASYMBOLSUM
                myMetaAdmRow = mMeta.MetaQuery.GetMetaAdmRow(dataSymbolSum);
                setMetaAdmInfo1(myMetaAdmRow);
                //DEFAULTCODEMISSINGLINE
                myMetaAdmRow = mMeta.MetaQuery.GetMetaAdmRow(defaultCodeMissingLine);
                setMetaAdmInfo1(myMetaAdmRow);
                // assign properties for specialcharacters

                setNpmCharacters();
                setMetaAdm();
                maxDatasymbolN = cat3Counter;
            } else {
                throw new PCAxis.Sql.Exceptions.DbPxsMismatchException(14,mMeta.MetaModelVersion);
                
            }
        }

        
        protected void setMetaAdmInfo1(MetaAdmRow myMetaAdmRow)
        {
            MetaAdmVariable myMetaVariable = new MetaAdmVariable();
            myMetaVariable.name = myMetaAdmRow.Property;
            myMetaVariable.value = myMetaAdmRow.Value;
            myMetaAdmVariables.Add(myMetaVariable.name.ToUpper(), myMetaVariable);
        }


        private void setNpmCharacters() {
            Dictionary<string, SpecialCharacterRow> specialCharacterRows = new Dictionary<string, SpecialCharacterRow>();
            specialCharacterRows = mMeta.MetaQuery.GetSpecialCharacterAllRows();
            NPMCharacter myNPMCharacter;
            npmId = 0;
            //myNPMCharacters = new Dictionary<int,NPMCharacter>();
            myNPMCharacters = new List<NPMCharacter>();
            foreach (KeyValuePair<string, SpecialCharacterRow> myRow in specialCharacterRows) {
                myNPMCharacter = new NPMCharacter();
                myNPMCharacter.id = npmId;
                myNPMCharacter.characterType = myRow.Value.CharacterType;
                myNPMCharacter.presCharacters = new Dictionary<string, string>();
                myNPMCharacter.presTexts = new Dictionary<string, string>();
                foreach (string lang in mMeta.LanguageCodes) {
                    myNPMCharacter.presCharacters[lang] = myRow.Value.texts[lang].PresCharacter;
                    myNPMCharacter.presTexts[lang] = myRow.Value.texts[lang].PresText;
                }

                if (myRow.Value.DataCellPres == mMeta.Config.Codes.Yes && myRow.Value.AggregPossible == mMeta.Config.Codes.Yes)
                {
                    myNPMCharacter.category = 1;
                }
                else if (myRow.Value.DataCellPres == mMeta.Config.Codes.No && myRow.Value.AggregPossible == mMeta.Config.Codes.Yes)
                {
                    myNPMCharacter.category = 2;
                }
                else if (myRow.Value.DataCellPres == mMeta.Config.Codes.No && myRow.Value.AggregPossible == mMeta.Config.Codes.No)
                {
                    myNPMCharacter.category = 3;
                }

                // Category 1 
                if (myNPMCharacter.category == 1) {
                    cat1Counter++;
                }

                // category 2
                else if (myNPMCharacter.category == 2) {
                    myNPMCharacter.pcAxisCode = PcAxisCodes.DataSymbol_NIL;
                    cat2Counter++;
                    if (cat2Counter > 1) {
                        throw new PCAxis.Sql.Exceptions.DbException(15);
                    }
                }
                // category 3
                else if (myNPMCharacter.category == 3) {
                    // check to se if any of the character is the same as the one specified for DataSymbolSum.
                    if ((metaVersion == "2.1" && (myNPMCharacter.presCharacters[mMeta.LanguageCodes[0]] == myMetaAdmVariables[dataSymbolSum].value)) || ((metaVersion == "2.2") && (myNPMCharacter.characterType == myMetaAdmVariables[dataSymbolSum].value))) {
                        myNPMCharacter.pcAxisCode = PcAxisCodes.DataSymbol_Sum;
                        dataSymbolSumNpmId = npmId;
                        dataNoteSumSet = true;
                    } else {
                        cat3Counter++;
                        if (cat3Counter > 6) {
                            throw new PCAxis.Sql.Exceptions.DbException(16);
                      } else {
                            myNPMCharacter.dataSymbolNr = cat3Counter;
                            myNPMCharacter.pcAxisCode = PcAxisCodes.getDataSymbolNo(cat3Counter);
                        }
                    }
                } else {
                    throw new PCAxis.Sql.Exceptions.DbException(17);
                }

                // myNPMCharacters.Add(npmId, myNPMCharacter);
                myNPMCharacters.Add(myNPMCharacter);
                npmId++;
            }
        }

        
        protected void setMetaAdm() {
            MetaAdmVariable varToCheck;

            // DefaultCodeMissingLine
            //defaultCodeMissingLine The value for defaultCodeMissingLine should be specified in SpecialCharacter and 
            // should be of category 2 or 3:
            // check if variables was found in the NPM table.
            varToCheck = myMetaAdmVariables[defaultCodeMissingLine];
            foreach (NPMCharacter npmChar in myNPMCharacters) {
                if ((npmChar.category == 2) || (npmChar.category == 3)) {
                    if (varToCheck.value == npmChar.characterType) {
                        varToCheck.npmRef = npmChar.id;
                        defaultCodeMissingLineSet = true;
                        break;
                    }
                }
            }
            if (!defaultCodeMissingLineSet) {
                throw new PCAxis.Sql.Exceptions.DbException(18);
            }

            //DataSymbolNil
            //DatasymbolNil should be of category 2
            varToCheck = myMetaAdmVariables[dataSymbolNIL];
            foreach (NPMCharacter npmChar in myNPMCharacters) {
                if (npmChar.category == 2) {
                    if (((metaVersion == "2.1") && (varToCheck.value == npmChar.presCharacters[mMeta.LanguageCodes[0]])) || ((metaVersion == "2.2") && (varToCheck.value == npmChar.characterType))) {
                        varToCheck.npmRef = npmChar.id;
                        dataSymbolNilSet = true;
                        break;
                    }
                }
            }
            if (!dataSymbolNilSet) {
                if (metaVersion == "2.1") {
                    if (cat2Counter < 1) {
                        NPMCharacter myNPMCharacter = new NPMCharacter();
                        myNPMCharacter.id = npmId;
                        myNPMCharacter.category = 2;
                        myNPMCharacter.pcAxisCode = PcAxisCodes.getDatasymbolNil();
                        Dictionary<string, string> myPresTexts = new Dictionary<string, string>();
                        Dictionary<string, string> myPresCharacters = new Dictionary<string, string>();
                        foreach (string lang in mMeta.LanguageCodes) {
                            myPresTexts.Add(lang, "");
                            myPresCharacters.Add(lang, varToCheck.value);
                        }
                        myNPMCharacter.presCharacters = myPresCharacters;
                        myNPMCharacter.presTexts = myPresTexts;
                        myNPMCharacters.Add(myNPMCharacter);
                        varToCheck.npmRef = myNPMCharacter.id;
                        npmId++;
                        cat2Counter++;
                    } else {
                        throw new PCAxis.Sql.Exceptions.DbException(19);
                    }
                } else {
                    throw new PCAxis.Sql.Exceptions.DbException(20, dataSymbolNIL);
                }
            }
            
            // DataNotAvailable
            // DataNotAvailable should be of category 3 
            varToCheck = myMetaAdmVariables[dataNotAvailable];
            foreach (NPMCharacter npmChar in myNPMCharacters)
            {
                if (npmChar.category == 3)
                {
                    if (((metaVersion == "2.1") && (varToCheck.value == npmChar.presCharacters[mMeta.LanguageCodes[0]])) || ((metaVersion == "2.2") && (varToCheck.value == npmChar.characterType)))
                    {
                        varToCheck.npmRef = npmChar.id;
                        dataNotAvailableSet = true;
                        break;
                    }
                }
            }

            if (!dataNotAvailableSet)
            {
                if (metaVersion == "2.1")
                {
                    if (cat3Counter <= 6)
                    {
                        NPMCharacter myNPMCharacter = new NPMCharacter();
                        myNPMCharacter.id = npmId;
                        myNPMCharacter.category = 3;
                        myNPMCharacter.dataSymbolNr = cat3Counter+1;
                        myNPMCharacter.pcAxisCode = PcAxisCodes.getDataSymbolNo(cat3Counter+1);
                        Dictionary<string, string> myPresTexts = new Dictionary<string, string>();
                        Dictionary<string, string> myPresCharacters = new Dictionary<string, string>();
                        foreach (string lang in mMeta.LanguageCodes)
                        {
                            myPresTexts.Add(lang, "");
                            myPresCharacters.Add(lang, varToCheck.value);
                        }
                        myNPMCharacter.presCharacters = myPresCharacters;
                        myNPMCharacter.presTexts = myPresTexts;
                        myNPMCharacters.Add(myNPMCharacter);
                        varToCheck.npmRef = myNPMCharacter.id;
                        cat3Counter++;
                        npmId++;
                    }
                    else
                    {
                        throw new PCAxis.Sql.Exceptions.DbException(21);
                    }
                }
                else
                {
                    throw new PCAxis.Sql.Exceptions.DbException(22, dataNotAvailable);
                }
            }

            // DataNoteSum
            varToCheck = myMetaAdmVariables[dataNoteSum];
            foreach (NPMCharacter npmChar in myNPMCharacters)
            {
                if (npmChar.category == 1)
                {
                    if (((metaVersion == "2.1") && (varToCheck.value == npmChar.presCharacters[mMeta.LanguageCodes[0]])) || ((metaVersion == "2.2") && (varToCheck.value == npmChar.characterType))) 
                    {
                        varToCheck.npmRef = npmChar.id;
                        dataNoteSumSet = true;
                        break;
                    }
                }
            }
            if (!dataNoteSumSet)
            {
                if (metaVersion == "2.1")
                {
                NPMCharacter myNPMCharacter = new NPMCharacter();
                myNPMCharacter.id = npmId;
                myNPMCharacter.category = 1;
                Dictionary<string, string> myPresTexts = new Dictionary<string,string>();
                Dictionary<string, string> myPresCharacters = new Dictionary<string,string>();
                foreach (string lang in mMeta.LanguageCodes)
                {
                    myPresTexts.Add(lang, "");
                    myPresCharacters.Add(lang, varToCheck.value);
                }
                myNPMCharacter.presCharacters = myPresCharacters;
                myNPMCharacter.presTexts = myPresTexts;
                myNPMCharacters.Add(myNPMCharacter);
                varToCheck.npmRef = myNPMCharacter.id;
                    cat1Counter++;
                    npmId++;
                }
                else
                {
                    throw new PCAxis.Sql.Exceptions.DbException(23, dataNoteSum);
                }
            }

            //DataSymbolSum
            varToCheck = myMetaAdmVariables[dataSymbolSum];
            if (dataSymbolSumSet) {
                varToCheck.npmRef = dataSymbolSumNpmId;
            } else {
                if (metaVersion == "2.1") {
                    if (cat3Counter < 6) {
                        NPMCharacter myNPMCharacter = new NPMCharacter();
                        myNPMCharacter.id = npmId;
                        myNPMCharacter.category = 3;
                        myNPMCharacter.dataSymbolNr = cat3Counter + 1;
                        myNPMCharacter.pcAxisCode = PcAxisCodes.getDataSymbolNo(cat3Counter + 1);
                        Dictionary<string, string> myPresTexts = new Dictionary<string, string>();
                        Dictionary<string, string> myPresCharacters = new Dictionary<string, string>();
                        foreach (string lang in mMeta.LanguageCodes) {
                            myPresTexts.Add(lang, "");
                            myPresCharacters.Add(lang, varToCheck.value);
                        }
                        myNPMCharacter.presCharacters = myPresCharacters;
                        myNPMCharacter.presTexts = myPresTexts;
                        myNPMCharacters.Add(myNPMCharacter);
                        varToCheck.npmRef = myNPMCharacter.id;
                        cat3Counter++;
                        npmId++;
                    } else {
                        throw new PCAxis.Sql.Exceptions.DbException(24);

                    }
                } else {
                  //  throw new PCAxis.Sql.Exceptions.DbException(25, dataSymbolSum);

                }
            }
        }

        public struct NPMCharacter
        {
            public int id;
            public string characterType;
            public Dictionary<string, string> presCharacters;
            public Dictionary<string, string> presTexts;
            public int dataSymbolNr;
            public int category;
            public double pcAxisCode;
        }

        protected class MetaAdmVariable
        {
            public string name;
            public string value;
            public int npmRef;
            //public Dictionary<string, string> presCharacter;
            //public int category;
        }

        internal struct PcAxisCodes
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


        // Public properties
        public double DefaultCodeMissingLineMagic
        {
            get
            {
                foreach (NPMCharacter myChar in myNPMCharacters)
                {
                    if (myChar.id == myMetaAdmVariables[defaultCodeMissingLine].npmRef)
                        
                        return myChar.pcAxisCode;
                }
                throw new PCAxis.Sql.Exceptions.DbException(27);
            }
        }

        public string DefaultCodeMissingLinePresChar(string lang)
        {

            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.id == myMetaAdmVariables[defaultCodeMissingLine].npmRef)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(27);
        }

        public double DataNotAvailableMagic
        {
            get
            {
                foreach (NPMCharacter myChar in myNPMCharacters)
                {
                    if (myChar.id == myMetaAdmVariables[dataNotAvailable].npmRef)
                        return myChar.pcAxisCode; 
                }
                throw new PCAxis.Sql.Exceptions.DbException(28);
             
            }
        }

        public string DataNotAvailablePresChar(string lang)
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.id == myMetaAdmVariables[dataNotAvailable].npmRef)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(28);
        }
        
        public double DataSymbolNilMagic
        {
            get
            {
                return PcAxisCodes.DataSymbol_NIL;
            }
        }

        public string DataSymbolNilPresChar(string lang)
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.id == myMetaAdmVariables[dataSymbolNIL].npmRef)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(29);
            
        }
        
        public double DataSymbolSumMagic
        {
            get
            {
                return PcAxisCodes.DataSymbol_Sum;
            }
        }

        public string DataSymbolSumPresChar(string lang)
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.id == myMetaAdmVariables[dataSymbolSum].npmRef)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(30);
            
        }

        // Public Method
        //public double DataSymbol1Magic(int symbolNo)
        //{

        //        return PcAxisCodes.getDataSymbolNo(symbolNo);
        //}
        // Public Method


        // 2. jan 2009 this had no references
        //public double getMagic(string characterType)
        //{
        //    foreach (NPMCharacter myChar in myNPMCharacters)
        //    {
        //        if (myChar.characterType == characterType)
        //            return myChar.pcAxisCode;
        //    }
        //    throw new ApplicationException("DataSymbolNumber for characterType" + characterType + "not set");
        //}

        public double DataSymbolNMagic(int symbolNo)
        {
            return PcAxisCodes.getDataSymbolNo(symbolNo);
        }

        public double DataSymbolNMagic(string characterType)
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.characterType == characterType)
                    return myChar.pcAxisCode;
            }
            throw new PCAxis.Sql.Exceptions.DbException(31, characterType);
            
        }
        
        public string DataSymbolNPresChar(int symbolNo,string lang)
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.dataSymbolNr== symbolNo)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(32, symbolNo);
        }
        
        public string DataNoteSumCharacterType()
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.id == myMetaAdmVariables[dataNoteSum].npmRef)
                    return myChar.characterType;
            }
             throw new PCAxis.Sql.Exceptions.DbException(33);
        }

        public string DataNoteSumPresChar(string lang)
        {
            foreach (NPMCharacter myChar in myNPMCharacters)
            {
                if (myChar.id == myMetaAdmVariables[dataNoteSum].npmRef)
                    return myChar.presCharacters[lang];
            }
            throw new PCAxis.Sql.Exceptions.DbException(33);
            
        }

        internal int GetCategory(double theMagicNumber) {
            foreach (NPMCharacter myChar in myNPMCharacters) {
                if (myChar.pcAxisCode == theMagicNumber)
                    return myChar.category;
            }
            throw new PCAxis.Sql.Exceptions.DbException(34, theMagicNumber.ToString() );
        }
        public NPMCharacter GetNpmBySpeciaCharacterType(string characterType)
        {
            foreach(NPMCharacter myCharacter in myNPMCharacters)
            {
                if (myCharacter.characterType==characterType)
                {
                    return myCharacter;
                }
            }
            throw new ApplicationException("Special character with charactertype= " + characterType + " does not exists");
        }
    }
}


