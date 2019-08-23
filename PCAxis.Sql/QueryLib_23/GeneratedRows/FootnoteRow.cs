using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_23
{

    /// <summary>
    /// Holds the attributes for Footnote. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains footnote texts and information on footnotes.
    /// </summary>
    public class FootnoteRow
    {
        private String mFootnoteNo;
        /// <summary>
        /// Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm.
        /// 
        /// See further in the description of the table MetaAdm.
        /// </summary>
        public String FootnoteNo
        {
            get { return mFootnoteNo; }
        }
        private String mFootnoteType;
        /// <summary>
        /// Code for the type of footnote. There are the following alternatives:
        /// 
        /// 1 = footnote on subject area
        /// 2 = footnote on content column
        /// 3 = footnote on variable + content column
        /// 4 = footnote on value/time + content column
        /// 5 = footnote on variable
        /// 6 = footnote on value
        /// 7 = footnote on main table
        /// 8 = footnote on sub-table
        /// 9 = footnote on value + main table
        /// A = footnote on statistical area (level 2)
        /// B = footnote on product (level 3)
        /// C = footnote on table group (level 4)
        /// Q = footnote on grouping
        /// </summary>
        public String FootnoteType
        {
            get { return mFootnoteType; }
        }
        private String mShowFootnote;
        /// <summary>
        /// Contains information on when the footnote should be shown in the outdata program, i.e. when content is selected for a table, when the table is presented or both.
        /// There are the following alternatives:
        /// 
        /// B = show both at selection and presentation
        /// P = show at presentation
        /// S = shown upon selection
        /// </summary>
        public String ShowFootnote
        {
            get { return mShowFootnote; }
        }
        private String mMandOpt;
        /// <summary>
        /// Code for whether the footnote is classified as "optional" or "mandatory".
        /// Alternatives:
        /// 
        /// O = optional
        /// M = mandatory
        /// </summary>
        public String MandOpt
        {
            get { return mMandOpt; }
        }
        private String mPresCharacter;
        /// <summary>
        /// Special character or special characters to be associated with the footnote
        /// </summary>
        public String PresCharacter
        {
            get { return mPresCharacter; }
        }

        public Dictionary<string, FootnoteTexts> texts = new Dictionary<string, FootnoteTexts>();

        public FootnoteRow(DataRow myRow, SqlDbConfig_23 dbconf, StringCollection languageCodes)
        {
            this.mFootnoteNo = myRow[dbconf.Footnote.FootnoteNoCol.Label()].ToString();
            this.mFootnoteType = myRow[dbconf.Footnote.FootnoteTypeCol.Label()].ToString();
            this.mShowFootnote = myRow[dbconf.Footnote.ShowFootnoteCol.Label()].ToString();
            this.mMandOpt = myRow[dbconf.Footnote.MandOptCol.Label()].ToString();
            this.mPresCharacter = myRow[dbconf.Footnote.PresCharacterCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new FootnoteTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Footnote  for one language.
    /// The table contains footnote texts and information on footnotes.
    /// </summary>
    public class FootnoteTexts
    {
        private String mFootnoteText;
        /// <summary>
        /// Text in the footnote. Written as consecutive text, starting with a capital letter.
        /// 
        /// NB! Double quotation marks should not be used as this causes problems in PC-AXIS.
        /// </summary>
        public String FootnoteText
        {
            get { return mFootnoteText; }
        }


        internal FootnoteTexts(DataRow myRow, SqlDbConfig_23 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mFootnoteText = myRow[dbconf.FootnoteLang2.FootnoteTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mFootnoteText = myRow[dbconf.Footnote.FootnoteTextCol.Label()].ToString();
            }
        }
    }

}
