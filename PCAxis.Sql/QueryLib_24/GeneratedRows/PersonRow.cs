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
    /// Holds the attributes for Person. (This entity is language independent.)
    /// 
    /// The table contains information on all persons (or alternatively groups) which are contact persons for content and/or responsible for updating statistics in the database.
    /// </summary>
    public class PersonRow
    {
        private String mPersonCode;
        /// <summary>
        /// Identifying code for person (or group) responsible.
        /// </summary>
        public String PersonCode
        {
            get { return mPersonCode; }
        }
        private String mOrganizationCode;
        /// <summary>
        /// Code for the organization or authority that produces and/or is responsible for the statistical material.
        /// 
        /// See further description of the table Organization.
        /// </summary>
        public String OrganizationCode
        {
            get { return mOrganizationCode; }
        }
        private String mForename;
        /// <summary>
        /// Responsible person's first name
        /// For groups, this data is not available and should therefore be NULL.
        /// </summary>
        public String Forename
        {
            get { return mForename; }
        }
        private String mSurname;
        /// <summary>
        /// Surname of the person responsible or name of the group responsible.
        /// </summary>
        public String Surname
        {
            get { return mSurname; }
        }
        private String mPhonePrefix;
        /// <summary>
        /// Prefix for telephone number, so that the number is valid internationally.
        /// 
        /// E.g. for Swedish telephone numbers the prefix is +46.
        /// </summary>
        public String PhonePrefix
        {
            get { return mPhonePrefix; }
        }
        private String mPhoneNo;
        /// <summary>
        /// Complete national telephone numbers, i.e. without international prefix.
        /// 
        /// Should be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.
        /// </summary>
        public String PhoneNo
        {
            get { return mPhoneNo; }
        }
        private String mFaxNo;
        /// <summary>
        /// Complete national fax machine numbers, i.e. without international prefix.
        /// 
        /// Should be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.
        /// If there is no fax number, this field should be NULL.
        /// </summary>
        public String FaxNo
        {
            get { return mFaxNo; }
        }
        private String mEmail;
        /// <summary>
        /// E-mail address for person or group responsible, if available.
        /// If an e-mail address is not available, the field should be NULL.
        /// 
        /// Written with lower case letters.
        /// </summary>
        public String Email
        {
            get { return mEmail; }
        }

        public PersonRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mPersonCode = myRow[dbconf.Person.PersonCodeCol.Label()].ToString();
            this.mOrganizationCode = myRow[dbconf.Person.OrganizationCodeCol.Label()].ToString();
            this.mForename = myRow[dbconf.Person.ForenameCol.Label()].ToString();
            this.mSurname = myRow[dbconf.Person.SurnameCol.Label()].ToString();
            this.mPhonePrefix = myRow[dbconf.Person.PhonePrefixCol.Label()].ToString();
            this.mPhoneNo = myRow[dbconf.Person.PhoneNoCol.Label()].ToString();
            this.mFaxNo = myRow[dbconf.Person.FaxNoCol.Label()].ToString();
            this.mEmail = myRow[dbconf.Person.EmailCol.Label()].ToString();
        }
    }
}
