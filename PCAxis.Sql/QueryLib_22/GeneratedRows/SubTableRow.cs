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
    /// Holds the attributes for SubTable. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on the sub-tables, reflecting the stored data tables, which are in the subject databases.  The data tables are identified using the main table's name + sub-table's name.
    /// </summary>
    public class SubTableRow
    {
        private String mSubTable;
        /// <summary>
        /// Name of the sub-table. For material stored only in the sub-table, the field is left empty, i.e. a dash is written.\nName of main table + name of sub-table together make up the name of the data table where the data is stored, if SubTable is not empty, in which case the name of the data table is made up of the name of the main table only.\nAt statistics \n,\n For material that is divided into several sub-tables, the following rules for naming apply:\n- For material that is not divided regionally, a consecutive numbering for the sub-tables is used that is linked to a specific main table, i.e.01, 02, etc.\n- For material that is divided regionally, the sub-tables are given the name:\nM (for sub-tables divided by municipality)\nC (for sub-tables divided by county)\nR (for sub-tables containing the whole country only)\n...followed by a consecutive numbering within each region value set for the sub-tables, which are linked to the relevant main table, i.e. M1, M2, C1, C2.\nNB. Make sure that the numbering is always included even if there is only one sub-table divided by region among the sub-tables that are linked to the relevant main table.
        /// </summary>
        public String SubTable
        {
            get { return mSubTable; }
        }
        private String mMainTable;
        /// <summary>
        /// The name of the main table, to which the sub-table is linked. See further description in the table MainTable.
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mCleanTable;
        /// <summary>
        /// Shows whether the sub-tables values can be aggregated or not.\nAt statistics \n,\n No longer in use. From June 1998, CleanTable = X was set for all new sub-tables, which should be written in. For older tables, the following codes can appear:\n- Y = yes, i.e. the sub-table values can be aggregated.\n- N = no, i.e. the sub-tables values cannot be aggregated.
        /// </summary>
        public String CleanTable
        {
            get { return mCleanTable; }
        }

        public Dictionary<string, SubTableTexts> texts = new Dictionary<string, SubTableTexts>();

        public SubTableRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mSubTable = myRow[dbconf.SubTable.SubTableCol.Label()].ToString();
            this.mMainTable = myRow[dbconf.SubTable.MainTableCol.Label()].ToString();
            this.mCleanTable = myRow[dbconf.SubTable.CleanTableCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new SubTableTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for SubTable  for one language.
    /// The table contains information on the sub-tables, reflecting the stored data tables, which are in the subject databases.  The data tables are identified using the main table's name + sub-table's name.
    /// </summary>
    public class SubTableTexts
    {
        private String mPresText;
        /// <summary>
        /// Descriptive text that is used by the retrieval interface, i.e. when selecting a sub-level to a table or sub-table in the retrieval interface, if the main table has several sub-tables.\nAt statistics \n,\n If an established abbreviation for the statistical product exists, i.e. AKU or KPI, this should be included in the presentation text.\nThe text should be unique (there should not be two sub-tables with the same PresText) and should contain information on all the division variables, excluding totals. Information on timescale should be added at the end.\nFor data material that is only stored in a sub-table, the text should be the same as PresText in the table MainTable.\nFor data material that is divided up into different sub-tables, the main table’s presentation text should be used as a "model", which is supplemented with the information that differentiates the sub-tables.\nThe text should begin with a capital letter, should not end with a full stop, and should not include the % symbol.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }


        internal SubTableTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.SubTableLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.SubTable.PresTextCol.Label()].ToString();
            }
        }
    }

}
