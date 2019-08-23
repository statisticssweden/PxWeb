using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_22
{

    /// <summary>
    /// Holds the attributes for MetaAdm. (This entity is language independent.) 
    /// 
    /// The table contains system variables and their values.
    /// </summary>
    public class MetaAdmRow
    {
        private String mProperty;
        /// <summary>
        /// At statistics \n,\n Name of system variable. There are the following alternatives:\n- LastFootnoteNo\n- MenuLevels\n- SpecCharSum\n- NoOfLanguage \n- Language1, Language2 osv.  \n- Codepage1\n- DataNotAvailable \n- DataNoteSum \n- DataSymbolSum \n- DataSymbolNil \n- PxDataFormat \n- KeysUpperLimit \n- DefaultCodeMissingLine \nNoOfLanguage indicates the number of languages that exists in the model for the meta database presentation texts, and that can be used by the retrieval interfaces. For each language there should be a line like:  Language1, Language2 and so on. Language1 should always include the main language. See also the description of the column Value in this table.\nFor each language there should also be a row in the table TextCatalog. For further information, see this table. \nRegarding DefaultCodeMissingLine see also descriptions in: PresCellsZero och PresMissingLine in Contents, and CharacterType and PresCharacter in SpecialCharacter.
        /// </summary>
        public String Property
        {
            get { return mProperty; }
        }
        private String mValue;
        /// <summary>
        /// Value of system variable. Contains one value per property.\nFor the property LastFootnoteNo, Value should contain the last used footnote number in the table Footnote.\nFor the property MenuLevels, Value should contains the highest used level number in the table MenuSelection.\nFor the property SpecCharSum, Value should contain the highest acceptable value for character type in the table SpecCharacter.\nFor the property NoOfLanguage Value should contain the number of languages that exists in the metadata model. \nFor the property Language1 Value should contain the main language of the model. The code is written in three capital letters, i.e. SVE.  \nFor the property  Language2 , Language3 etc., Value should contain the other languages of the model. The code is written in three capital letters, i.e. ENG, ESP. The code is used as a suffix in the extra tables that should exist in the meta database, i.e. SubTable_ENG, SubTable_ESP. \nFor the property Codepage1: The characters that can be used and  how they should be presented. Is used at creating the keyword Codepage in the px file and at converting to XML. \nThree different examples: iso-8859-1, windows-1251, big5.\nFor the property DataNotAvailable: The value that should be presented, if  the data cell contains NULL and NPM-character is missing. If the value exists in the table SpecialCharacter, it is used, otherwise the character in the table DataNotAvailable is used. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample: .. (two dots).  \nFor the property DataNoteSum: The value that should be presented after the sum, if data cells with different NPM marking is summarized. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample:  * \n1A + 2B = 3* \nFor the property DataSymbolSum: The value that should be presented if data cells with different NPM character are summarized and no sum can be created. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample: N.A. \n. + .. = N.A.\nFor the property DataSymbolNil: the value that should be presented at absolute 0 (zero) in the table SpecialCharacter. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample: -\nFor the property PxDataFormat: Matrix = all retreivals should be stored in matrix format.\nKeysnn = retreivals with keys are remade \nnn > read data cells *100 / presented number of data cells\nExample: 40\n(Default is Matrix.)\nFor the property KeysUpperLimit: Maximum number of data cells that the presented matrix may contain, if the retreival should be possible to do with Keys. If greater, the retreival is made in matrix format.\nExample: 1000000\nFor the property DefaultCodeMissingLine: The value that should be presented in data cells that are not stored. Is used if neither presentation with 0 or special character have been specified. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nSee also the description of the column Property in this table, and also the table TextCatalog.
        /// </summary>
        public String Value
        {
            get { return mValue; }
        }
        private String mDescription;
        /// <summary>
        /// Description of the property for internal use
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }

        public MetaAdmRow(DataRow myRow, SqlDbConfig_22 dbconf)
        {
            this.mProperty = myRow[dbconf.MetaAdm.PropertyCol.Label()].ToString();
            this.mValue = myRow[dbconf.MetaAdm.ValueCol.Label()].ToString();
            this.mDescription = myRow[dbconf.MetaAdm.DescriptionCol.Label()].ToString();
        }
    }
}
