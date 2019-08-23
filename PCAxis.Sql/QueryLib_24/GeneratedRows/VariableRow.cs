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
    /// Holds the attributes for Variable. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains the distributed statistical variables in the database.
    /// </summary>
    public class VariableRow
    {
        private String mVariable;
        /// <summary>
        /// Name of distributed statistical variable. Name of metadata column for the variable in the data table.
        /// 
        /// The variable name must be unique within a main table.
        /// 
        /// The name should be descriptive, i.e. have an obvious link to the presentation text, consist of a maximum of 20 characters, begin with a capital letter and should only contains letters (a-z) and numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
        /// </summary>
        public String Variable
        {
            get { return mVariable; }
        }
        private String mVariableInfo;
        /// <summary>
        /// Descriptive information on variables, primarily for internal use, to facilitate the selection of a variable when drawing up new tables.
        /// 
        /// If there is no text, the field should be NULL.
        /// </summary>
        public String VariableInfo
        {
            get { return mVariableInfo; }
        }
        private String mMetaId;
        /// <summary>
        /// MetaId can be used to link the information in this table to an external system.
        /// </summary>
        public String MetaId
        {
            get { return mMetaId; }
        }
        private String mFootnote;
        /// <summary>
        /// Shows whether there is a footnote linked to the variable (FootnoteType 5). There are the following alternatives:
        /// 
        /// B = Both obligatory and optional footnotes exist
        /// V = One or several optional footnotes exist.
        /// O = One or several obligatory footnotes exist
        /// N = There are no footnotes
        /// </summary>
        public String Footnote
        {
            get { return mFootnote; }
        }

        public Dictionary<string, VariableTexts> texts = new Dictionary<string, VariableTexts>();

        public VariableRow(DataRow myRow, SqlDbConfig_24 dbconf, StringCollection languageCodes)
        {
            this.mVariable = myRow[dbconf.Variable.VariableCol.Label()].ToString();
            this.mVariableInfo = myRow[dbconf.Variable.VariableInfoCol.Label()].ToString();
            this.mMetaId = myRow[dbconf.Variable.MetaIdCol.Label()].ToString();
            this.mFootnote = myRow[dbconf.Variable.FootnoteCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new VariableTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Variable  for one language.
    /// The table contains the distributed statistical variables in the database.
    /// </summary>
    public class VariableTexts
    {
        private String mPresText;
        /// <summary>
        /// Presentation text for a variable. Used in the retrieval interface when selecting variables or values and in the heading text when the table is presented after retrieval.
        /// 
        /// The entire text should be written in lower case letters, with the exception of abbreviations, etc.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }


        internal VariableTexts(DataRow myRow, SqlDbConfig_24 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.VariableLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.Variable.PresTextCol.Label()].ToString();
            }
        }
    }

}
