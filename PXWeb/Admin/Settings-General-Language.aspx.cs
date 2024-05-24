using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Settings_General_Language : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            var lang = from l in Settings.Current.General.Language.AllLanguages
                       select new
                       {
                           //Id = l,
                           //Name = new CultureInfo(l).EnglishName,
                           //Selected = Settings.Current.General.Language.SiteLanguages.Contains(l)
                           Id = l,
                           Name = new CultureInfo(l).EnglishName,
                           Selected = Settings.Current.General.Language.IsSiteLanguage(l),
                           DecimalSeparator = GetDecimalSeparator(l),
                           ThousandSeparator = GetThousandSeparator(l),
                           DateFormat = GetDateFormat(l)
                       };

            rptSiteLanguages.DataSource = lang;
            rptSiteLanguages.DataBind();

            cboDefaultLanguage.DataSource = lang;
            cboDefaultLanguage.DataBind();
            cboDefaultLanguage.SelectedValue = Settings.Current.General.Language.DefaultLanguage;
        }


        /// <summary>
        /// Save Site settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            HiddenField hidSetting;

            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    PXWeb.LanguagesSettings lang = (PXWeb.LanguagesSettings)PXWeb.Settings.NewSettings.General.Language;

                    lang.DefaultLanguage = cboDefaultLanguage.SelectedValue;
                    List<ILanguageSettings> langs = (List<ILanguageSettings>)lang.SiteLanguages;
                    langs.Clear();
                    foreach (RepeaterItem itm in rptSiteLanguages.Items)
                    {
                        SaveLanguageSettings(itm, langs, false);
                    }

                    // Add the default language if it is not selected
                    if (langs.Find(x => (x.Name == lang.DefaultLanguage)) == null)
                    {
                        foreach (RepeaterItem itm in rptSiteLanguages.Items)
                        {
                            hidSetting = (HiddenField)itm.FindControl("hidSetting");
                            if (hidSetting != null)
                            {
                                if (hidSetting.Value == lang.DefaultLanguage)
                                {
                                    SaveLanguageSettings(itm, langs, true);
                                }
                            }
                        }
                    }

                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }

            }
        }

        /// <summary>
        /// Save settings for one of the languages in the list
        /// </summary>
        /// <param name="itm">The repeater item representing the language</param>
        /// <param name="langs">List of languagesettings</param>
        /// <param name="defaultLanguage">If it is the default language or not</param>
        private void SaveLanguageSettings(RepeaterItem itm, List<ILanguageSettings> langs, bool defaultLanguage)
        {
            LanguageSettings ls;
            CheckBox cbx;
            HiddenField hidSetting;
            DropDownList cbo;
            TextBox txt;

            if ((itm.ItemType == ListItemType.Item) || (itm.ItemType == ListItemType.AlternatingItem))
            {
                cbx = (CheckBox)itm.FindControl("cbxLanguage");
                if (cbx.Checked || defaultLanguage)
                {
                    hidSetting = (HiddenField)itm.FindControl("hidSetting");

                    ls = new LanguageSettings();
                    ls.Name = hidSetting.Value;
                    ls.DefaultLanguage = ls.Name.Equals(cboDefaultLanguage.SelectedValue);

                    cbo = (DropDownList)itm.FindControl("cboDecimalSeparator");
                    ls.DecimalSeparator = LanguageSettings.DecodeDecimalSeparator(cbo.SelectedValue);

                    cbo = (DropDownList)itm.FindControl("cboThousandSeparator");
                    ls.ThousandSeparator = LanguageSettings.DecodeThousandSeparator(cbo.SelectedValue);

                    txt = (TextBox)itm.FindControl("txtDateFormat");
                    ls.DateFormat = txt.Text;

                    langs.Add(ls);
                }
            }
        }

        /// <summary>
        /// Get the decimal separator for the given language
        /// </summary>
        /// <param name="lang">Language</param>
        /// <returns>Decimal separator</returns>
        private string GetDecimalSeparator(string lang)
        {
            PCAxis.Paxiom.Settings.LocaleSettings locSettings = PCAxis.Paxiom.Settings.GetLocale(lang);

            return LanguageSettings.EncodeDecimalSeparator(locSettings.DecimalSeparator);
        }

        /// <summary>
        /// Get the thousand separator for the given language
        /// </summary>
        /// <param name="lang">Language</param>
        /// <returns>Thousand separator</returns>
        private string GetThousandSeparator(string lang)
        {
            PCAxis.Paxiom.Settings.LocaleSettings locSettings = PCAxis.Paxiom.Settings.GetLocale(lang);

            return LanguageSettings.EncodeThousandSeparator(locSettings.ThousandSeparator);
        }

        /// <summary>
        /// Get the date format for the given language
        /// </summary>
        /// <param name="lang">Language</param>
        /// <returns>Date format</returns>
        private string GetDateFormat(string lang)
        {
            PCAxis.Paxiom.Settings.LocaleSettings locSettings = PCAxis.Paxiom.Settings.GetLocale(lang);

            return locSettings.DateFormat;
        }

        protected void imgDefaultLanguage_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsLanguageDefaultLanguage", "PxWebAdminSettingsLanguageDefaultLanguageInfo");
        }

        protected void imgSiteLanguagesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsLanguageSiteLanguages", "PxWebAdminSettingsLanguageSiteLanguagesInfo");
        }
        protected void imgDecimalSeparatorInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsLanguageDecimalSeparator", "PxWebAdminSettingsLanguageDecimalSeparatorInfo");
        }
        protected void imgThousandSeparatorInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsLanguageThousandSeparator", "PxWebAdminSettingsLanguageThousandSeparatorInfo");
        }
        protected void imgDateFormatInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsLanguageDateFormat", "PxWebAdminSettingsLanguageDateFormatInfo");
        }

    }
}
