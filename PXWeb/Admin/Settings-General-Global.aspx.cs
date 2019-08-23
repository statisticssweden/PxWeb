using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;

namespace PXWeb.Admin
{
    public partial class Settings_General_Global : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
            }
        }

        /// <summary>
        /// Read and display General.Global settings  
        /// </summary>
        private void ReadSettings()
        {
            cboSecrecyOption.SelectedValue = PXWeb.Settings.Current.General.Global.SecrecyOption.ToString();
            cboRoundingRule.SelectedValue = PXWeb.Settings.Current.General.Global.RoundingRule.ToString();
            txtSymbol1.Text = PXWeb.Settings.Current.General.Global.Symbol1;
            txtSymbol2.Text = PXWeb.Settings.Current.General.Global.Symbol2;
            txtSymbol3.Text = PXWeb.Settings.Current.General.Global.Symbol3;
            txtSymbol4.Text = PXWeb.Settings.Current.General.Global.Symbol4;
            txtSymbol5.Text = PXWeb.Settings.Current.General.Global.Symbol5;
            txtSymbol6.Text = PXWeb.Settings.Current.General.Global.Symbol6;
            txtDataSymbolNil.Text = PXWeb.Settings.Current.General.Global.DataSymbolNil;
            txtDataSymbolSum.Text = PXWeb.Settings.Current.General.Global.DataSymbolSum;

            //cboDecimalSeparator.SelectedValue = PXWeb.Settings.Current.General.Global.DecimalSeparator;
            //cboThousandSeparator.SelectedValue = PXWeb.Settings.Current.General.Global.ThousandSeparator;
            cboShowSourceDescription.SelectedValue = PXWeb.Settings.Current.General.Global.ShowSourceDescription.ToString();
            cboTableInformationLevel.SelectedValue = PXWeb.Settings.Current.General.Global.TableInformationLevel.ToString();
            cboUpperCase.SelectedValue = PXWeb.Settings.Current.General.Global.Uppercase.ToString();
            cboDataNotePlacement.SelectedValue = PXWeb.Settings.Current.General.Global.DataNotePlacement.ToString();
            cboRemoveSingleContent.SelectedValue = PXWeb.Settings.Current.General.Global.RemoveSingleContent.ToString();
            cboStrictAggregationCheck.SelectedValue = PXWeb.Settings.Current.General.Global.StrictAggregationCheck.ToString();
            chkInfoFile.Checked = PXWeb.Settings.Current.General.Global.ShowInfoFile;

            List<PCAxis.Enums.InformationType> lst = new List<PCAxis.Enums.InformationType>();
            Array enumValues = System.Enum.GetValues(typeof(PCAxis.Enums.InformationType));

            foreach (PCAxis.Enums.InformationType enumValue in enumValues)
            {
                lst.Add(enumValue);
            }

            var infoTypes = from it in lst
                           select new 
                           {
                               Id = it.ToString(),
                               Name = Master.GetLocalizedString("CtrlInformation" + it.ToString() + "Label"),
                               Selected = CheckIfInformationTypeIsSelected(it.ToString())
                           };

            rptInformationTypes.DataSource = infoTypes;
            rptInformationTypes.DataBind();
        }

        /// <summary>
        /// Checks if the information type is selected in the settings
        /// </summary>
        /// <param name="id">Id of the InformationType</param>
        /// <returns>True if selected, else false</returns>
        private bool CheckIfInformationTypeIsSelected(string id)
        {
            switch (id)
            {
                case "OfficialStatistics":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.OfficialStatistics;
                case "Unit":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Unit;
                case "Contact":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Contact;
                case "LastUpdated":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.LastUpdated;
                case "RefPeriod":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.RefPeriod;
                case "StockFa":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.StockFa;
                case "CFPrices":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.CFPrices;
                case "DayAdj":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.DayAdj;
                case "SeasAdj":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.SeasAdj;
                case "BasePeriod":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.BasePeriod;
                case "UpdateFrequency":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.UpdateFrequency;
                case "NextUpdate":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.NextUpdate;
                case "Survey":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Survey;
                case "Link":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Link;
                case "CreationDate":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.CreationDate;
                case "Copyright":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Copyright;
                case "Source":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Source;
                case "Matrix":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Matrix;
                case "Database":
                    return PXWeb.Settings.Current.General.Global.ShowInformationTypes.Database;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Save General.Global settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PXWeb.Settings.BeginUpdate())
                {
                    try
                    {
                        PXWeb.GlobalSettings glob = (PXWeb.GlobalSettings)PXWeb.Settings.NewSettings.General.Global;

                        glob.SecrecyOption = (PCAxis.Paxiom.SecrecyOptionType)Enum.Parse(typeof(PCAxis.Paxiom.SecrecyOptionType), cboSecrecyOption.SelectedValue);
                        glob.RoundingRule = (PCAxis.Paxiom.RoundingType)Enum.Parse(typeof(PCAxis.Paxiom.RoundingType), cboRoundingRule.SelectedValue);
                        glob.Symbol1 = txtSymbol1.Text;
                        glob.Symbol2 = txtSymbol2.Text;
                        glob.Symbol3 = txtSymbol3.Text;
                        glob.Symbol4 = txtSymbol4.Text;
                        glob.Symbol5 = txtSymbol5.Text;
                        glob.Symbol6 = txtSymbol6.Text;
                        glob.DataSymbolNil = txtDataSymbolNil.Text;
                        glob.DataSymbolSum = txtDataSymbolSum.Text;
                        //glob.DecimalSeparator = cboDecimalSeparator.SelectedValue;
                        //glob.ThousandSeparator = cboThousandSeparator.SelectedValue;
                        glob.ShowSourceDescription = bool.Parse(cboShowSourceDescription.SelectedValue);
                        glob.TableInformationLevel = (PCAxis.Paxiom.InformationLevelType)Enum.Parse(typeof(PCAxis.Paxiom.InformationLevelType), cboTableInformationLevel.SelectedValue);
                        glob.Uppercase = bool.Parse(cboUpperCase.SelectedValue);
                        glob.DataNotePlacement = (PCAxis.Paxiom.DataNotePlacementType)Enum.Parse(typeof(PCAxis.Paxiom.DataNotePlacementType), cboDataNotePlacement.SelectedValue);
                        glob.RemoveSingleContent = bool.Parse(cboRemoveSingleContent.SelectedValue);
                        glob.StrictAggregationCheck = bool.Parse(cboStrictAggregationCheck.SelectedValue);
                        glob.ShowInfoFile = chkInfoFile.Checked;


                        foreach (RepeaterItem itm in rptInformationTypes.Items)
                        {
                            if ((itm.ItemType == ListItemType.Item) || (itm.ItemType == ListItemType.AlternatingItem))
                            {
                                CheckBox chk = (CheckBox)itm.FindControl("chkInfoType");
                                HiddenField hidSetting = (HiddenField)itm.FindControl("hidSetting");
                                SaveInfoType(chk.Checked, hidSetting.Value);
                            }
                        }
                        
                        PXWeb.Settings.Save();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                        PCAxis.Paxiom.GroupRegistry.GetRegistry().Strict = PXWeb.Settings.Current.General.Global.StrictAggregationCheck;
                        PCAxis.Paxiom.GroupRegistry.GetRegistry().ReloadGroupingsAsync();
                    }
                }
            }
        }

        private void SaveInfoType(bool selected, string id)
        {
            PXWeb.ShowInformationTypesSettings infoTypes = (PXWeb.ShowInformationTypesSettings)PXWeb.Settings.NewSettings.General.Global.ShowInformationTypes;
            switch (id)
            {
                case "OfficialStatistics":
                    infoTypes.OfficialStatistics = selected;
                    break;
                case "Unit":
                    infoTypes.Unit = selected;
                    break;
                case "Contact":
                    infoTypes.Contact = selected;
                    break;
                case "LastUpdated":
                    infoTypes.LastUpdated = selected;
                    break;
                case "RefPeriod":
                    infoTypes.RefPeriod = selected;
                    break;
                case "StockFa":
                    infoTypes.StockFa = selected;
                    break;
                case "CFPrices":
                    infoTypes.CFPrices = selected;
                    break;
                case "DayAdj":
                    infoTypes.DayAdj = selected;
                    break;
                case "SeasAdj":
                    infoTypes.SeasAdj = selected;
                    break;
                case "BasePeriod":
                    infoTypes.BasePeriod = selected;
                    break;
                case "UpdateFrequency":
                    infoTypes.UpdateFrequency = selected;
                    break;
                case "NextUpdate":
                    infoTypes.NextUpdate = selected;
                    break;
                case "Survey":
                    infoTypes.Survey = selected;
                    break;
                case "Link":
                    infoTypes.Link = selected;
                    break;
                case "CreationDate":
                    infoTypes.CreationDate = selected;
                    break;
                case "Copyright":
                    infoTypes.Copyright = selected;
                    break;
                case "Source":
                    infoTypes.Source = selected;
                    break;
                case "Matrix":
                    infoTypes.Matrix = selected;
                    break;
                case "Database":
                    infoTypes.Database = selected;
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Validate the Symbol x settings
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateSymbol(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            args.IsValid = true;
        }

        protected void SecrecyOptionInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSecrecyOption", "PxWebAdminSettingsGlobalSecrecyOptionInfo");
        }
        protected void RoundingRuleInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalRoundingRule", "PxWebAdminSettingsGlobalRoundingRuleInfo");
        }
        protected void Symbol1Info(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSymbol1", "PxWebAdminSettingsGlobalSymbol1Info");
        }
        protected void Symbol2Info(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSymbol2", "PxWebAdminSettingsGlobalSymbol2Info");
        }
        protected void Symbol3Info(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSymbol3", "PxWebAdminSettingsGlobalSymbol3Info");
        }
        protected void Symbol4Info(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSymbol4", "PxWebAdminSettingsGlobalSymbol4Info");
        }
        protected void Symbol5Info(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSymbol5", "PxWebAdminSettingsGlobalSymbol5Info");
        }
        protected void Symbol6Info(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalSymbol6", "PxWebAdminSettingsGlobalSymbol6Info");
        }
        protected void DataSymbolNilInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalDataSymbolNil", "PxWebAdminSettingsGlobalDataSymbolNilInfo");
        }
        protected void DataSymbolSumInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalDataSymbolSum", "PxWebAdminSettingsGlobalDataSymbolSumInfo");
        }
        protected void DecimalSeparatorInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalDecimalSeparator", "PxWebAdminSettingsGlobalDecimalSeparatorInfo");
        }
        protected void ThousandSeparatorInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalThousandSeparator", "PxWebAdminSettingsGlobalThousandSeparatorInfo");
        }
        protected void ShowSourceDescriptionInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalShowSourceDescription", "PxWebAdminSettingsGlobalShowSourceDescriptionInfo");
        }
        protected void TableInformationLevelInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalTableInformationLevel", "PxWebAdminSettingsGlobalTableInformationLevelInfo");
        }
        protected void DataNotePlacementInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalDataNotePlacement", "PxWebAdminSettingsGlobalDataNotePlacementInfo");
        }
        protected void RemoveSingleContentInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalRemoveSingleContent", "PxWebAdminSettingsGlobalRemoveSingleContentInfo");
        }
        protected void ShowInformationTypesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalShowInformationTypes", "PxWebAdminSettingsGlobalShowInformationTypesInfo");
        }
        protected void UpperCaseInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalUpperCase", "PxWebAdminSettingsGlobalUpperCaseInfo");
        }
        protected void StrictAggregationCheckInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGlobalStrictAggregationCheck", "PxWebAdminSettingsGlobalStrictAggregationCheckInfo");
        }
        protected void ShowInformationInfoFile(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebDetailedInformation", "PxWebAdminSettingsGlobalDetailedInformationInfo");
        }
        
    }
}
