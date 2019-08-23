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
using PXWeb.Misc;
using System.Collections.Generic;
using PCAxis.Chart;

namespace PXWeb.Admin
{
    public partial class Features_Charts_General : System.Web.UI.Page
    {
        private string _guidelinesColor = "#BBBBBB";
        public string GuidelinesColor
        {
            get
            {
                return _guidelinesColor;
            }
        }
        private string _backgroundColorGraphs = "#FFFFFF";
        public string BackgroundColorGraphs
        {
            get
            {
                return _backgroundColorGraphs;
            }
        }
        private string _lineColorPhrame = "#AAAAAA";
        public string LineColorPhrame 
        {
            get
            {
                return _lineColorPhrame;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Very ugly fix for weird javascript error that only occur in IE11.
            // If browser version is IE11 use IE9 instead for this page 
            // Only IE11 uses Trident version 7 so when you use the regular expression below only IE11 should be found...
            if (System.Text.RegularExpressions.Regex.IsMatch(this.Request.UserAgent, @"Trident/7.*rv:11"))
            {
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "X-UA-Compatible";
                meta.Content = "IE=9";
                this.Header.Controls.AddAt(0, meta);
            }

            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                InitalizeSelectionControls();
                ReadSettings();
                ShowHidePathImgLogo();
                Page.DataBind();
            }
            else
            {
               AddColor(false);               
               //_guidelinesColor = txtGuidelinesColor.Text;
               //_lineColorPhrame = txtLineColorPhrame.Text;
               
               //_backgroundColorGraphs = txtBackgroundColorGraphs.Text;               
            }
            
        }

        /// <summary>
        /// Initialize the selection controls
        /// </summary>
        private void InitalizeSelectionControls()
        {
            cboFontName.DataSource = System.Drawing.FontFamily.Families;
            cboFontName.DataTextField = "Name";
            cboFontName.DataValueField = "Name";
            cboFontName.DataBind();
        }

        /// <summary>
        /// Read and display Charts settings  
        /// </summary>
        private void ReadSettings()
        {
            cboFontName.SelectedValue = PXWeb.Settings.Current.Features.Charts.Font.Name;
            txtFontSizeTitle.Text = PXWeb.Settings.Current.Features.Charts.Font.TitleSize.ToString();
            txtFontSizeAxis.Text = PXWeb.Settings.Current.Features.Charts.Font.AxisSize.ToString();
            txtHeight.Text = PXWeb.Settings.Current.Features.Charts.Height.ToString();
            txtMaxHeight.Text = PXWeb.Settings.Current.Features.Charts.MaxHeight.ToString();
            txtWidth.Text = PXWeb.Settings.Current.Features.Charts.Width.ToString();
            txtMaxWidth.Text = PXWeb.Settings.Current.Features.Charts.MaxWidth.ToString();
            txtLineThickness.Text = PXWeb.Settings.Current.Features.Charts.LineThickness.ToString();
            txtMaxLineThickness.Text = PXWeb.Settings.Current.Features.Charts.MaxLineThickness.ToString();
            cboLabelOrientation.SelectedValue = PXWeb.Settings.Current.Features.Charts.LabelOrientation.ToString();
            cboTimeSortOrder.SelectedValue = PXWeb.Settings.Current.Features.Charts.TimeSortOrder.ToString();
            cboShowLegend.SelectedValue = PXWeb.Settings.Current.Features.Charts.Legend.Visible.ToString();
            txtLegendHeight.Text = PXWeb.Settings.Current.Features.Charts.Legend.Height.ToString();
            txtLegendFontSize.Text = PXWeb.Settings.Current.Features.Charts.Legend.FontSize.ToString();
            txtGuidelinesColor.Text = PXWeb.Settings.Current.Features.Charts.Guidelines.Color;
            _guidelinesColor = PXWeb.Settings.Current.Features.Charts.Guidelines.Color;
            cboGuidelinesHorizontal.SelectedValue = PXWeb.Settings.Current.Features.Charts.Guidelines.Horizontal.ToString();
            cboGuidelinesVertical.SelectedValue = PXWeb.Settings.Current.Features.Charts.Guidelines.Vertical.ToString();
            txtMaxValues.Text = PXWeb.Settings.Current.Features.Charts.MaxValues.ToString();
            cboShowLogo.SelectedValue = PXWeb.Settings.Current.Features.Charts.ShowLogo.ToString();    
            txtPathImgLogo.Text = PXWeb.Settings.Current.Features.Charts.Logotype.ToString();
            cboShowSourse.SelectedValue = PXWeb.Settings.Current.Features.Charts.ShowSource.ToString();
            txtLineThicknessPhrame.Text = PXWeb.Settings.Current.Features.Charts.LineThicknessPhrame.ToString();
            _lineColorPhrame = PXWeb.Settings.Current.Features.Charts.LineColorPhrame.ToString();
            txtLineColorPhrame.Text = PXWeb.Settings.Current.Features.Charts.LineColorPhrame.ToString();
            _backgroundColorGraphs = PXWeb.Settings.Current.Features.Charts.BackgroundColorGraphs.ToString();
            txtBackgroundColorGraphs.Text = PXWeb.Settings.Current.Features.Charts.BackgroundColorGraphs.ToString();

            rptColors.DataSource = PXWeb.Settings.Current.Features.Charts.Colors;
            rptColors.DataBind();
        }

        /// <summary>
        /// Save Features settings
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
                        PXWeb.ChartsSettings charts = (PXWeb.ChartsSettings)PXWeb.Settings.NewSettings.Features.Charts;

                        charts.Height = int.Parse(txtHeight.Text);
                        charts.MaxHeight = int.Parse(txtMaxHeight.Text);
                        charts.Width = int.Parse(txtWidth.Text);
                        charts.MaxWidth = int.Parse(txtMaxWidth.Text);
                        charts.LineThickness = int.Parse(txtLineThickness.Text);
                        charts.MaxLineThickness = int.Parse(txtMaxLineThickness.Text);
                        charts.MaxValues = int.Parse(txtMaxValues.Text);
                        charts.ShowLogo = bool.Parse(cboShowLogo.SelectedValue);
                        charts.Logotype = this.txtPathImgLogo.Text.Trim();                        
                        charts.ShowSource = bool.Parse(cboShowSourse.SelectedValue); 
                        
                        switch (cboLabelOrientation.SelectedValue)
                        {
                            case "Vertical":
                                charts.LabelOrientation = ChartSettings.OrientationType.Vertical;
                                break;
                            default:
                                charts.LabelOrientation = ChartSettings.OrientationType.Horizontal;
                                break;
                        }
                        switch (cboTimeSortOrder.SelectedValue)
                        {
                            case "Ascending":
                                charts.TimeSortOrder = ChartSettings.SortType.Ascending;
                                break;
                            case "Descending":
                                charts.TimeSortOrder = ChartSettings.SortType.Descending;
                                break;
                            default:
                                charts.TimeSortOrder = ChartSettings.SortType.None;
                                break;
                        }

                        charts.LineThicknessPhrame = int.Parse(txtLineThicknessPhrame.Text);
                        _lineColorPhrame = txtLineColorPhrame.Text;
                        charts.BackgroundColorGraphs = txtBackgroundColorGraphs.Text;
                        _backgroundColorGraphs = txtBackgroundColorGraphs.Text;

                        SetColors(rptColors, (List<string>)charts.Colors);

                        PXWeb.ChartFontSettings fonts = (PXWeb.ChartFontSettings)charts.Font;

                        fonts.Name = cboFontName.SelectedValue;
                        fonts.TitleSize = int.Parse(txtFontSizeTitle.Text);
                        fonts.AxisSize = int.Parse(txtFontSizeAxis.Text);

                        PXWeb.ChartLegendSettings legend = (PXWeb.ChartLegendSettings)charts.Legend;

                        legend.Visible = bool.Parse(cboShowLegend.SelectedValue);
                        legend.Height = int.Parse(txtLegendHeight.Text);
                        legend.FontSize = int.Parse(txtLegendFontSize.Text);

                        PXWeb.ChartGuidelinesSettings guidelines = (PXWeb.ChartGuidelinesSettings)charts.Guidelines;

                        guidelines.Color = txtGuidelinesColor.Text;
                        guidelines.Horizontal = bool.Parse(cboGuidelinesHorizontal.SelectedValue);
                        guidelines.Vertical = bool.Parse(cboGuidelinesVertical.SelectedValue);
                        _guidelinesColor = txtGuidelinesColor.Text;                        
                        
                        Page.DataBind();
                        PXWeb.Settings.Save();
                        //Set the txtAddSelectedColor field to blank after saving so
                        //that the color picker and textbox has the same value after saving
                        this.txtAddSelectedColor.Text = "";
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        private void SetColors(Repeater rpt, List<string> lst)
        {
            lst.Clear();

            foreach (RepeaterItem itm in rpt.Items)
            {
                TextBox tb = (TextBox)itm.FindControl("tbSelectedColor");
                lst.Add(tb.Text);
            }
        }
        #region Region Validators
        /// <summary>
        /// Validates font size value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateFontSize(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (int.Parse(args.Value) > 100)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationMaximumValue", "100");
                return;
            }
        }

        /// <summary>
        /// Validates the height value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateHeight(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (txtMaxHeight.Text.Length == 0)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxHeight"));
                return;
            }

            int value;
            if (int.TryParse(txtMaxHeight.Text, out value) == false)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxHeight"));
                return;
            }

            if (int.Parse(txtHeight.Text) > int.Parse(txtMaxHeight.Text))
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxHeight"));
                return;
            }

        }

        /// <summary>
        /// Validates the width value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateWidth(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (txtMaxWidth.Text.Length == 0)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxWidth"));
                return;
            }

            int value;
            if (int.TryParse(txtMaxWidth.Text, out value) == false)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxWidth"));
                return;
            }

            if (int.Parse(txtWidth.Text) > int.Parse(txtMaxWidth.Text))
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxWidth"));
                return;
            }

        }

        /// <summary>
        /// Validates the line thickness
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateLineThickness(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";


            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false )
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (txtMaxLineThickness.Text.Length == 0)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxLineThickness"));
                return;
            }

            int value;
            if (int.TryParse(txtMaxLineThickness.Text, out value) == false)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxLineThickness"));
                return;
            }

            if (int.Parse(txtLineThickness.Text) > int.Parse(txtMaxLineThickness.Text))
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationBiggerThanMaxSetting", Master.GetLocalizedString("PxWebAdminFeaturesChartsGeneralMaxLineThickness"));
                return;
            }
            

        }

       /// <summary>
        /// Validates the line thickness
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateLineThicknessPhrame(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";


            if (InputValidation.ValidatePositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }
        }

        /// <summary>
        /// Validates legend height value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateLegendHeight(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (int.Parse(args.Value) > 100)
            {
                SetValidationError(val, args, "PxWebAdminSettingsValidationMaximumValue", "100");
                return;
            }
        }

        /// <summary>
        /// Validates that value is a valid color code
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateColor(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateColorCode(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Validates that value is an integer value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateMandatoryInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }
        }

        /// <summary>
        /// Validate the Logo path settings
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateLogoPath(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string path;

            if (args.Value.Length == 0 && bool.Parse(cboShowLogo.SelectedValue))
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }
            else
            {
                path = System.IO.Path.Combine(PXWeb.Settings.Current.General.Paths.ImagesPath, args.Value);
                if (!System.IO.File.Exists(Server.MapPath(path)))
                {
                    args.IsValid = false;
                    val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationNonexistentImage");
                    return;
                }
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Set error message for validator with invalid value
        /// </summary>
        /// <param name="val">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="errorKey">Key for error message</param>
        /// <param name="parameters">Eventual parameters to the localized string</param>
        private void SetValidationError(CustomValidator val, System.Web.UI.WebControls.ServerValidateEventArgs args, string errorKey, params string[] parameters)
        {
            args.IsValid = false;
            if (parameters.Length > 0)
            {
                val.ErrorMessage = string.Format(Master.GetLocalizedString(errorKey), parameters);
            }
            else
            {
                val.ErrorMessage = Master.GetLocalizedString(errorKey);
            }
        }
        #endregion Validators
        protected string GetString(object o)
        {
            return (string)o;
        }
        protected System.Drawing.Color GetColor(object o)
        {
            return System.Drawing.ColorTranslator.FromHtml((string)o);
        }

        private void AddColor(bool newColor)
        {
            List<string> lst = new List<string>();
            TextBox tb;

            if (validatorAddColor.IsValid)
            {
                foreach (RepeaterItem itm in rptColors.Items)
                {
                    tb = (TextBox)itm.FindControl("tbSelectedColor");
                    lst.Add(tb.Text);
                }
                if (txtAddSelectedColor.Text.Length == 7 && newColor)
                {
                    lst.Add(txtAddSelectedColor.Text);
                    txtAddSelectedColor.Text = string.Empty;
                    //txtAddSelectedColor.Text = "#ffffff";
                }
                rptColors.DataSource = lst;
                rptColors.DataBind();
                
            }
        }

        /// <summary>
        /// Called when linkbutton clicked within the color repeater
        /// </summary>
        /// <param name="source">clicked object</param>
        /// <param name="e">arguments</param>
        protected void rptColors_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteColor"))
            {
                // Delete color linkbutton clicked
                List<string> lst = new List<string>();
                TextBox tb;

                foreach (RepeaterItem itm in rptColors.Items)
                {
                    tb = (TextBox)itm.FindControl("tbSelectedColor");
                    if (!tb.Text.Equals(e.CommandArgument))
                    {
                        lst.Add(tb.Text);
                    }
                }
                rptColors.DataSource = lst;
                rptColors.DataBind();
            }
            this.txtAddSelectedColor.Text = "";
        }


        /// <summary>
        /// Add color link clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAddColor_Click(Object sender, EventArgs e)
        {
            AddColor(true);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptColors_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ClientScript.RegisterStartupScript(GetType(), "ColorpickerBinder" + e.Item.ItemIndex.ToString(), "BindColorPicker(\""+e.Item.ItemIndex.ToString() + "\",\"" + GetString(e.Item.DataItem) + "\");", true);
                TextBox tb = (TextBox)e.Item.FindControl("tbSelectedColor");
                tb.Text = GetString(e.Item.DataItem);
                tb.CssClass += " selectedColor" + e.Item.ItemIndex.ToString();
            }
  
        }
        /// <summary>
        /// Is called when the value "Show logo" changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboShowLogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHidePathImgLogo();
        }
        /// <summary>
        /// If the setting showLogo is true then show the control
        /// for url to image
        /// </summary>
        private void ShowHidePathImgLogo()
        {
            if (bool.Parse(cboShowLogo.SelectedValue))
            {
                pnlLogotypeInGraphs.Visible = true;    
            }
            else
            {
                pnlLogotypeInGraphs.Visible = false;
            }
        }

        protected void FontNameInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralFontName", "PxWebAdminFeaturesChartsGeneralFontNameInfo");
        }
        protected void FontSizeTitleInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralFontSizeTitle", "PxWebAdminFeaturesChartsGeneralFontSizeTitleInfo");
        }
        protected void FontSizeAxisInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralFontSizeAxis", "PxWebAdminFeaturesChartsGeneralFontSizeAxisInfo");
        }
        protected void HeightInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralHeight", "PxWebAdminFeaturesChartsGeneralHeightInfo");
        }
        protected void MaxHeightInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralMaxHeight", "PxWebAdminFeaturesChartsGeneralMaxHeightInfo");
        }
        protected void WidthInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralWidth", "PxWebAdminFeaturesChartsGeneralWidthInfo");
        }
        protected void MaxWidthInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralMaxWidth", "PxWebAdminFeaturesChartsGeneralMaxWidthInfo");
        }
        protected void LineThicknessInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLineThickness", "PxWebAdminFeaturesChartsGeneralLineThicknessInfo");
        }
        protected void MaxLineThicknessInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralMaxLineThickness", "PxWebAdminFeaturesChartsGeneralMaxLineThicknessInfo");
        }
        protected void LabelOrientationInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLabelOrientation", "PxWebAdminFeaturesChartsGeneralLabelOrientationInfo");
        }
        protected void TimeSortOrderInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralTimeSortOrder", "PxWebAdminFeaturesChartsGeneralTimeSortOrderInfo");
        }
        protected void ShowLegendInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralShowLegend", "PxWebAdminFeaturesChartsGeneralShowLegendInfo");
        }
        protected void LegendHeightInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLegendHeight", "PxWebAdminFeaturesChartsGeneralLegendHeightInfo");
        }
        protected void LegendFontSizeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLegendFontSize", "PxWebAdminFeaturesChartsGeneralLegendFontSizeInfo");
        }
        protected void GuidelinesColorInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralGuidelinesColor", "PxWebAdminFeaturesChartsGeneralGuidelinesColorInfo");
        }
        protected void GuidelinesHorizontalInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralGuidelinesHorizontal", "PxWebAdminFeaturesChartsGeneralGuidelinesHorizontalInfo");
        }
        protected void GuidelinesVerticalInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralGuidelinesVertical", "PxWebAdminFeaturesChartsGeneralGuidelinesVerticalInfo");
        }
        protected void MaxValuesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralMaxValues", "PxWebAdminFeaturesChartsGeneralMaxValuesInfo");
        }
        protected void ColorsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralColors", "PxWebAdminFeaturesChartsGeneralColorsInfo");
        }
        protected void ShowSourseInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralShowSourse", "PxWebAdminFeaturesChartsGeneralShowSourseInfo");
        }
        protected void ShowLogoInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralShowLogo", "PxWebAdminFeaturesChartsGeneralShowLogoInfo");
        }
        protected void ImgLogoInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLogoImg", "PxWebAdminFeaturesChartsGeneralLogoImgInfo");
        }
        protected void LineThicknessPhrameInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLineThicknessPhrame", "PxWebAdminFeaturesChartsGeneralLineThicknessPhrameInfo");
        }
        protected void BackgroundColorGraphsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralBackgroundColorGraphs", "PxWebAdminFeaturesChartsGeneralBackgroundColorGraphsInfo");
        }
        protected void LineColorPhrameInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesChartsGeneralLineColorPhrame", "PxWebAdminFeaturesChartsGeneralLineColorPhrameInfo");
        }        
    }
}
