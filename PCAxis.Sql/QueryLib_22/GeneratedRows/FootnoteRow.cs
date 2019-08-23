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
    /// Holds the attributes for Footnote. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains footnote texts and information on footnotes.  (In the future, it could also contain links to documents, publications, etc. on Statistics Sweden's website or the website of another statistical authority).
    /// </summary>
    public class FootnoteRow
    {
        private String mFootnoteNo;
        /// <summary>
        /// Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm.\nSee further in the description of the table MetaAdm.
        /// </summary>
        public String FootnoteNo
        {
            get { return mFootnoteNo; }
        }
        private String mFootnoteType;
        /// <summary>
        /// Code for the type of footnote. There are the following alternatives:\n1 = footnote on subject area\n2 = footnote on content column\n3 = footnote on variable + content column\n4 = footnote on value/time + content column\n5 = footnote on variable\n6 = footnote on value\n7 = footnote on main table\n8 = footnote on sub-table\n9 = footnote on value + main table\nA = footnote on statistical area (level 2)\nB = footnote on product (level 3)\nC = footnote on table group (level 4)\nQ = footnote on grouping
        /// </summary>
        public String FootnoteType
        {
            get { return mFootnoteType; }
        }
        private String mShowFootnote;
        /// <summary>
        /// Contains information on when the footnote should be shown in the outdata program, i.e. when content is selected for a table, when the table is presented or both.\nThere are the following alternatives:\nB = shown upon both selection and presentation\nP = shown upon presentation\nS = shown upon selection
        /// </summary>
        public String ShowFootnote
        {
            get { return mShowFootnote; }
        }
        private String mMandOpt;
        /// <summary>
        /// Code for whether the footnote is classified as "voluntary" or "obligatory".\nAlternatives:\nO = optional\nM = mandatory
        /// </summary>
        public String MandOpt
        {
            get { return mMandOpt; }
        }

        public Dictionary<string, FootnoteTexts> texts = new Dictionary<string, FootnoteTexts>();

        public FootnoteRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mFootnoteNo = myRow[dbconf.Footnote.FootnoteNoCol.Label()].ToString();
            this.mFootnoteType = myRow[dbconf.Footnote.FootnoteTypeCol.Label()].ToString();
            this.mShowFootnote = myRow[dbconf.Footnote.ShowFootnoteCol.Label()].ToString();
            this.mMandOpt = myRow[dbconf.Footnote.MandOptCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new FootnoteTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Footnote  for one language.
    /// The table contains footnote texts and information on footnotes.  (In the future, it could also contain links to documents, publications, etc. on Statistics Sweden's website or the website of another statistical authority).
    /// </summary>
    public class FootnoteTexts
    {
        private String mFootnoteText;
        /// <summary>
        /// Text in the footnote. Written as consecutive text, starting with a capital letter.\nAt statistics \n,\n MacroMeta currently allows footnote texts of a maximum of 700 characters long.\nNOT YET IMPLEMENTED.\nThe footnote texts can be edited using HTML tags to insert a new row, bold or italic text. Only the following is allowed:\n<b> Bold </b> Bold text\n<I> Italic </I> Italic text\n<BR> New row\nThe text can also contain links to documents, publications, etc. on Statistics Sweden's website or the website of another statistical authority. The link is written in HTML format. Example: \n <a href=http://www.scb.se> </a>See Statistics Sweden's website for more information!</a>\nNB! Double quotation marks should not be used as this causes problems in PC-AXIS.
        /// </summary>
        public String FootnoteText
        {
            get { return mFootnoteText; }
        }


        internal FootnoteTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
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
