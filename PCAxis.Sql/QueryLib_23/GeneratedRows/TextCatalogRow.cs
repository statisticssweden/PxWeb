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
    /*This table is not suitable for generated extractions such as Get...*/

    /// <summary>
    /// Holds the attributes for TextCatalog. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on joint texts.
    /// </summary>
    public class TextCatalogRow
    {
        private String mTextCatalogNo;
        /// <summary>
        /// Identity of text.
        /// </summary>
        public String TextCatalogNo
        {
            get { return mTextCatalogNo; }
        }

        public Dictionary<string, TextCatalogTexts> texts = new Dictionary<string, TextCatalogTexts>();

        public TextCatalogRow(DataRow myRow, SqlDbConfig_23 dbconf, StringCollection languageCodes)
        {
            this.mTextCatalogNo = myRow[dbconf.TextCatalog.TextCatalogNoCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new TextCatalogTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for TextCatalog  for one language.
    /// The table contains information on joint texts.
    /// </summary>
    public class TextCatalogTexts
    {
        private String mTextType;
        /// <summary>
        /// Type of text. The texts should be fixed for use in PC-AXIS.
        /// 
        /// Alternatives:
        /// - ContentsVariable
        /// - GeoAreaNo
        /// - Language1, Language2 (and so on)
        /// </summary>
        public String TextType
        {
            get { return mTextType; }
        }
        private String mPresText;
        /// <summary>
        /// The text that should be shown. Can be the name of a map file etc., or a language. The language should be written in the language it refers to, e.g. svenska, English, Espanol. See also the description of the table MetaAdm.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mDescription;
        /// <summary>
        /// Description of text.
        /// 
        /// If a description is not available, the field should be NULL.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }


        internal TextCatalogTexts(DataRow myRow, SqlDbConfig_23 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mTextType = myRow[dbconf.TextCatalogLang2.TextTypeCol.Label(languageCode)].ToString();
                this.mPresText = myRow[dbconf.TextCatalogLang2.PresTextCol.Label(languageCode)].ToString();
                this.mDescription = myRow[dbconf.TextCatalogLang2.DescriptionCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mTextType = myRow[dbconf.TextCatalog.TextTypeCol.Label()].ToString();
                this.mPresText = myRow[dbconf.TextCatalog.PresTextCol.Label()].ToString();
                this.mDescription = myRow[dbconf.TextCatalog.DescriptionCol.Label()].ToString();
            }
        }
    }

}
