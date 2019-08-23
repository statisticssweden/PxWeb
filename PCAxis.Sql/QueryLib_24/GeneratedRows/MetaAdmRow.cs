using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_24
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
        /// Name of system variable. There are the following alternatives:
        /// - LastFootnoteNo
        /// - MenuLevels
        /// - SpecCharSum
        /// - NoOfLanguage
        /// - Language1, Language2 and so on
        /// - Codepage1
        /// - DataNotAvailable
        /// - DataNoteSum
        /// - DataSymbolSum
        /// - DataSymbolNil
        /// - PxDataFormat
        /// - KeysUpperLimit
        /// - DefaultCodeMissingLine
        /// 
        /// NoOfLanguage indicates the number of languages that exists in the model for the meta database presentation texts, and that can be used by the retrieval interfaces. For each language there should be a line like: Language1, Language2 and so on. Language1 should always include the main language. See also the description of the column Value in this table.
        /// 
        /// For each language there should also be a row in the table TextCatalog. For further information, see this table.
        /// 
        /// Regarding DefaultCodeMissingLine see also descriptions in: PresCellsZero och PresMissingLine in Contents, and CharacterType and PresCharacter in SpecialCharacter.
        /// </summary>
        public String Property
        {
            get { return mProperty; }
        }
        private String mValue;
        /// <summary>
        /// Value of system variable. Contains one value per property.
        /// 
        /// For the property LastFootnoteNo, Value should contain the last used footnote number in the table Footnote.
        /// 
        /// For the property MenuLevels, Value should contains the highest used level number in the table MenuSelection.
        /// 
        /// For the property SpecCharSum, Value should contain the highest acceptable value for character type in the table SpecCharacter.
        /// 
        /// For the property NoOfLanguage Value should contain the number of languages that exists in the metadata model.
        /// 
        /// For the property Language1 Value should contain the main language of the model. The code is written in three capital letters, i.e. SVE.
        /// 
        /// For the property  Language2 , Language3 etc., Value should contain the other languages of the model. The code is written in three capital letters, i.e. ENG, ESP. The code is used as a suffix in the extra tables that should exist in the meta database, i.e. SubTable_ENG, SubTable_ESP.
        /// 
        /// For the property Codepage1: The characters that can be used and  how they should be presented. Is used at creating the keyword Codepage in the px file and at converting to XML.
        /// Three different examples: iso-8859-1, windows-1251, big5.
        /// 
        /// For the property DataNotAvailable: The value that should be presented, if  the data cell contains NULL and NPM-character is missing. If the value exists in the table SpecialCharacter, it is used, otherwise the character in the table DataNotAvailable is used.  The value is a reference to CharacterType in the SpecialCharacter table.
        /// Example: .. (two dots).
        /// 
        /// For the property DataNoteSum: The value that should be presented after the sum, if data cells with different NPM marking is summarized. The value is a reference to CharacterType in the SpecialCharacter table.
        /// Example:  *
        /// 1A + 2B = 3*
        /// 
        /// For the property DataSymbolSum: The value that should be presented if data cells with different NPM character are summarized and no sum can be created. The value is a reference to CharacterType in the SpecialCharacter table.
        /// Example: N.A.
        /// . + .. = N.A.
        /// 
        /// For the property DataSymbolNil: the value that should be presented at absolute 0 (zero) in the table SpecialCharacter. The value is a reference to CharacterType in the SpecialCharacter table.
        /// Example: -
        /// 
        /// For the property PxDataFormat: Matrix = all retreivals should be stored in matrix format.
        /// Keysnn = retreivals with keys are remade
        /// nn &gt; read data cells *100 / presented number of data cells
        /// Example: 40
        /// (Default is Matrix.)
        /// 
        /// For the property KeysUpperLimit: Maximum number of data cells that the presented matrix may contain, if the retreival should be possible to do with Keys. If greater, the retreival is made in matrix format.
        /// Example: 1000000
        /// 
        /// For the property DefaultCodeMissingLine: The value that should be presented in data cells that are not stored. Is used if neither presentation with 0 or special character have been specified. The value is a reference to CharacterType in the SpecialCharacter table.
        /// 
        /// See also the description of the column Property in this table, and also the table TextCatalog.
        /// </summary>
        public String Value
        {
            get { return mValue; }
        }
        private String mDescription;
        /// <summary>
        /// Must contain a description of the property.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }

        public MetaAdmRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mProperty = myRow[dbconf.MetaAdm.PropertyCol.Label()].ToString();
            this.mValue = myRow[dbconf.MetaAdm.ValueCol.Label()].ToString();
            this.mDescription = myRow[dbconf.MetaAdm.DescriptionCol.Label()].ToString();
        }
    }
}
