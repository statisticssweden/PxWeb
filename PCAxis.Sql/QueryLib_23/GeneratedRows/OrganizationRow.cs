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
    /// Holds the attributes for Organization. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on the authorities and organizations that are responsible for or produce statistics.
    /// </summary>
    public class OrganizationRow
    {
        private String mOrganizationCode;
        /// <summary>
        /// Identification for the organization
        /// </summary>
        public String OrganizationCode
        {
            get { return mOrganizationCode; }
        }
        private String mMetaId;
        /// <summary>
        /// MetaId can be used to link the information in this table to an external system.
        /// </summary>
        public String MetaId
        {
            get { return mMetaId; }
        }

        public Dictionary<string, OrganizationTexts> texts = new Dictionary<string, OrganizationTexts>();

        public OrganizationRow(DataRow myRow, SqlDbConfig_23 dbconf, StringCollection languageCodes)
        {
            this.mOrganizationCode = myRow[dbconf.Organization.OrganizationCodeCol.Label()].ToString();
            this.mMetaId = myRow[dbconf.Organization.MetaIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new OrganizationTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Organization  for one language.
    /// The table contains information on the authorities and organizations that are responsible for or produce statistics.
    /// </summary>
    public class OrganizationTexts
    {
        private String mOrganizationName;
        /// <summary>
        /// Name of authority/organisation in full text, including any official abbreviation in brackets, e.g. Statistics Sweden (SCB).
        /// 
        /// Text should begin with a capital letter.
        /// </summary>
        public String OrganizationName
        {
            get { return mOrganizationName; }
        }
        private String mDepartment;
        /// <summary>
        /// Name of the department or equivalent that produces the statistics.
        /// </summary>
        public String Department
        {
            get { return mDepartment; }
        }
        private String mUnit;
        /// <summary>
        /// Name of unit or equivalent.
        /// </summary>
        public String Unit
        {
            get { return mUnit; }
        }
        private String mWebAddress;
        /// <summary>
        /// Internet address to the authority's/organisation's website. Written as, for example:
        /// www.scb.se
        /// 
        /// If Internet address is not available, the field should be NULL.
        /// </summary>
        public String WebAddress
        {
            get { return mWebAddress; }
        }


        internal OrganizationTexts(DataRow myRow, SqlDbConfig_23 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mOrganizationName = myRow[dbconf.OrganizationLang2.OrganizationNameCol.Label(languageCode)].ToString();
                this.mDepartment = myRow[dbconf.OrganizationLang2.DepartmentCol.Label(languageCode)].ToString();
                this.mUnit = myRow[dbconf.OrganizationLang2.UnitCol.Label(languageCode)].ToString();
                this.mWebAddress = myRow[dbconf.OrganizationLang2.WebAddressCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mOrganizationName = myRow[dbconf.Organization.OrganizationNameCol.Label()].ToString();
                this.mDepartment = myRow[dbconf.Organization.DepartmentCol.Label()].ToString();
                this.mUnit = myRow[dbconf.Organization.UnitCol.Label()].ToString();
                this.mWebAddress = myRow[dbconf.Organization.WebAddressCol.Label()].ToString();
            }
        }
    }

}
