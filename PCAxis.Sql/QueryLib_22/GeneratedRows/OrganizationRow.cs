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
    /// Holds the attributes for Organization. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on the authorities and organisations that are responsible for or produce statistics, which are stored in the Macro or Micro databases.
    /// </summary>
    public class OrganizationRow
    {
        private String mOrganizationCode;
        /// <summary>
        /// Code for the organisation or authority that produces and/or is responsible for the statistics material. \nThe field is used both to show the organisation responsible for the statistics and the producer of the statistics in the table Contents.\nAt statistics sweden,\n For every authority/organisation that is responsible for some statistical material in the databases, there should be a "main item" in the table Organization, where the OrganizationCode consists of an abbreviation of the authority's/organisation's name, official or unofficial (i.e. SCB or KI). Used in the field StatAuthority in Contents.\nWhere Statistics Sweden is the producer of the statistics, there should be an item for every function within Statistics Sweden that produces statistics in the databases. In this case, OrganizationCode should be an abbreviation, which, in addition to "SCB", should also contain the department or unit name, divided by a forward slash, i.e.SCB/BV/BE. Used in the field Producer in Contents.\nWhere the statistics are produced by another statistical authority, the authority's/organisation's abbreviated name should be used to give the producer in the field Producer in Contents. In this case, the fields Department and Unit in Organization should be NULL.\nThe code is written in capital letters.
        /// </summary>
        public String OrganizationCode
        {
            get { return mOrganizationCode; }
        }
        private String mWebAddress;
        /// <summary>
        /// Internet address to the authority's/organisation's website. Written as, for example:\nwww.scb.se\nIf Internet address is not available, the field should be NULL.
        /// </summary>
        public String WebAddress
        {
            get { return mWebAddress; }
        }
        private String mInternalId;
        /// <summary>
        /// Identifying code for the Product database (PDB). \nAt statistics \n,\n Not yet implemented. Will later be obligatory to fill in at Statistics Sweden.
        /// </summary>
        public String InternalId
        {
            get { return mInternalId; }
        }

        public Dictionary<string, OrganizationTexts> texts = new Dictionary<string, OrganizationTexts>();

        public OrganizationRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mOrganizationCode = myRow[dbconf.Organization.OrganizationCodeCol.Label()].ToString();
            this.mWebAddress = myRow[dbconf.Organization.WebAddressCol.Label()].ToString();
            this.mInternalId = myRow[dbconf.Organization.InternalIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new OrganizationTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Organization  for one language.
    /// The table contains information on the authorities and organisations that are responsible for or produce statistics, which are stored in the Macro or Micro databases.
    /// </summary>
    public class OrganizationTexts
    {
        private String mOrganizationName;
        /// <summary>
        /// Name of authority/organisation in full text, including any official abbreviation in brackets, i.e. Statistics Sweden (SCB).\nText should begin with a capital letter.
        /// </summary>
        public String OrganizationName
        {
            get { return mOrganizationName; }
        }
        private String mDepartment;
        /// <summary>
        /// Name of the department or equivalent within Statistics Sweden that produces the statistics.\nOnly filled in for statistics producers within Statistics Sweden, otherwise the field should be NULL.
        /// </summary>
        public String Department
        {
            get { return mDepartment; }
        }
        private String mUnit;
        /// <summary>
        /// Name of unit or equivalent within Statistics Sweden that produces the statistics.\nOnly filled in for statistics producers within Statistics Sweden, otherwise the field should be NULL.
        /// </summary>
        public String Unit
        {
            get { return mUnit; }
        }


        internal OrganizationTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mOrganizationName = myRow[dbconf.OrganizationLang2.OrganizationNameCol.Label(languageCode)].ToString();
                this.mDepartment = myRow[dbconf.OrganizationLang2.DepartmentCol.Label(languageCode)].ToString();
                this.mUnit = myRow[dbconf.OrganizationLang2.UnitCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mOrganizationName = myRow[dbconf.Organization.OrganizationNameCol.Label()].ToString();
                this.mDepartment = myRow[dbconf.Organization.DepartmentCol.Label()].ToString();
                this.mUnit = myRow[dbconf.Organization.UnitCol.Label()].ToString();
            }
        }
    }

}
