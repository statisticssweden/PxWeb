using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.DbConfig; // ReadSqlDbConfig;
using PCAxis.Sql.QueryLib_23;
using System.Collections.Specialized;
using PCAxis.Paxiom;
using log4net;

namespace PCAxis.Sql.Parser_23
{

    /// <summary>Keeps infomation corresponding to the PCAxis Contact Keyword.
    /// Documentation on the PCAxis Contact Keyword:
    /// States the person who can give information about the statistics. 
    /// Is written in the form name, organization, telephone, fax, e-mail. 
    /// Several persons can be stated in the same textstring and are then divided by the #-sign. 
    /// </summary>
    public class PXSqlContact
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlContact));

        private Dictionary<string, string> mBigFatContactStringsByLanguage = new Dictionary<string, string>();

        // to avoid multiple SQLs to lookup the same org. 
        private Dictionary<string, OrganizationRow> orgs = new Dictionary<string, OrganizationRow>();

        /// <summary>
        /// Builds the Contactstring for each language in meta.LanguageCodes. 
        /// </summary>
        public PXSqlContact(PXSqlMeta_23 meta, string mMainTableId)
        {
            foreach (string language in meta.LanguageCodes)
            {
                mBigFatContactStringsByLanguage.Add(language, "");
            }

            try
            {
                Dictionary<string, MainTablePersonRow> personIDs = new Dictionary<string, MainTablePersonRow>();
                try
                {
                    foreach (KeyValuePair<string, MainTablePersonRow> RoleMain in meta.MetaQuery.GetMainTablePersonRows(mMainTableId, meta.Config.Codes.RoleMain,false))
                    {
                        if (!personIDs.ContainsKey(RoleMain.Key))
                        {
                            personIDs.Add(RoleMain.Key, RoleMain.Value);
                        }
                    }
                } catch (Exceptions.PCAxisSqlException e)
                {
                    log.Error("Cant add RoleMain", e);
                }

                try
                {
                    foreach (KeyValuePair<string, MainTablePersonRow> RoleContact in meta.MetaQuery.GetMainTablePersonRows(mMainTableId, meta.Config.Codes.RoleContact,true))
                    {

                        if (!personIDs.ContainsKey(RoleContact.Key))
                        {
                            personIDs.Add(RoleContact.Key, RoleContact.Value);
                        }
                    }
                } catch (Exceptions.PCAxisSqlException e)
                {
                    log.Error("Cant add RoleContact", e);
                }

                


                bool firstPerson = true;
                foreach (string personId in personIDs.Keys)
                {
                    PersonRow person = meta.MetaQuery.GetPersonRow(personId);

                    if (!orgs.ContainsKey(person.OrganizationCode))
                    {
                        orgs.Add(person.OrganizationCode, meta.MetaQuery.GetOrganizationRow(person.OrganizationCode));
                    }
                    OrganizationRow org = orgs[person.OrganizationCode];

                    foreach (string language in meta.LanguageCodes)
                    {
                        if (!firstPerson)
                        {
                            //mBigFatContactStringsByLanguage[language] += "#";
                           // mBigFatContactStringsByLanguage[language] += "||";
                        }



                        //mBigFatContactStringsByLanguage[language] += person.Forename + " " + person.Surname + ", " + org.texts[language].OrganizationName +
                        //    "# " + PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString("PxcContactPhone", language) + ": " + person.PhonePrefix + " " + person.PhoneNo +
                        //    "#" + PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString("PxcContactFax", language) + ": " + person.PhonePrefix + " " + person.FaxNo +
                        //    "#" + PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString("PxcContactEMail", language) + ": " + person.Email;

                        mBigFatContactStringsByLanguage[language] +=
                            person.Forename +
                            "#" + person.Surname +
                            "# " + person.PhonePrefix +
                            "#" + person.PhoneNo +
                            "#" + person.Email +
                            "#" + org.texts[language].OrganizationName +
                            "#" + org.texts[language].Department +
                            "#" + org.texts[language].Unit +
                            "||";


                    }

                    firstPerson = false;

                }
            } catch (Exceptions.PCAxisSqlException e)
            {
                log.Error("Cant find contact info", e);
            }
        }

        /// <summary>Is written in the form name, organization, telephone, fax, e-mail. 
        /// Several persons can be stated in the same textstring and are then divided by the #-sign.
        /// </summary>
        /// <param name="LanguageCode">The code of the language influences only the name of the organization.</param>
        /// <returns></returns>
        public string GetBigFatContactString(string LanguageCode)
        {
            if (!mBigFatContactStringsByLanguage.ContainsKey(LanguageCode))
            {
                throw new ApplicationException("Bug");
            }
            return mBigFatContactStringsByLanguage[LanguageCode];
        }

    }
}
