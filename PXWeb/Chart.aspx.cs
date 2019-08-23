using System;
using System.Collections;
using System.Collections.Generic;
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
using PCAxis.Web.Controls;
using PCAxis.Web.Controls.CommandBar.Plugin;
using PCAxis.Web.Core.Management;
using PCAxis.Chart;
using PCAxis.Paxiom.Operations;
using System.Windows.Forms.DataVisualization.Charting;
using PXWeb.Misc;
using PCAxis.Paxiom;
using System.Text;

namespace PXWeb
{
    public partial class Chart : System.Web.UI.Page
    {
        private int _errorNumber = 1;

        private string GetLayout()
        {
            if (RouteInstance.RouteExtender == null) return PxUrl.GetParameter(PxUrl.LAYOUT_KEY);
            return PxUrlObj.Layout;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;

            if (!IsPostBack)
            {
                imgSettingsExpander.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(BreadcrumbCodebehind), "PCAxis.Web.Controls.spacer.gif");
                InitializeChart();

                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
                {
                    InitializeCommandBar();
                }
                //lnkShowChartSettings.NavigateUrl = PanelLink.BuildLink("chartsettings");
                //lnkHideChartSettings.NavigateUrl = PanelLink.BuildLink("");
                SetDisplayModeOnPanels();
            }
            PCAxis.Web.Core.Management.PaxiomManager.RegisterPaxiomModelChanged(PaxoimModelChanged);
            IsLanguageChanged();
            lnkCancelSettings.NavigateUrl = PanelLink.BuildLink("");
        }

        private void SetTitle()
        {
            var manyValuesVariableCodes = PaxiomManager.PaxiomModel.Meta.Variables.Where(x => x.Values.Count > 1).Select(x => x.Code).ToList();
            var singleValueVariableCodes = PaxiomManager.PaxiomModel.Meta.Variables.Where(x => x.Values.Count == 1).Select(x => x.Code).ToArray();
            var notExplicitManyVariableCodes = PaxiomManager.PaxiomModel.Meta.Variables.Where(x => !manyValuesVariableCodes.Contains(x.Code) && !singleValueVariableCodes.Contains(x.Code)).Select(x => x.Code);
            manyValuesVariableCodes.AddRange(notExplicitManyVariableCodes);

            var manyValuesVariableNames = new List<string>();

            foreach (var code in manyValuesVariableCodes)
            {
                var variable = PaxiomManager.PaxiomModel.Meta.Variables.Single(x => x.Code == code);
                manyValuesVariableNames.Add(variable.Name);
            }

            var singleValueTexts = new List<string>();

            foreach (var code in singleValueVariableCodes)
            {
                var variable = PaxiomManager.PaxiomModel.Meta.Variables.Single(x => x.Code == code);
                var selectedValueCode = PaxiomManager.PaxiomModel.Meta.Variables.Single(x => x.Code == code).Values.First().Code;
                var valueText = variable.Values.First(x => x.Code == selectedValueCode).Value;
                singleValueTexts.Add(valueText);
            }

            var result = new StringBuilder();

            string byPart = string.Format(" {0} ", LocalizationManager.GetLocalizedString("PxcMetaTitleBy"));
            string andPart = string.Format(" {0} ", LocalizationManager.GetLocalizedString("PxcMetaTitleAnd"));

            result.Append(PaxiomManager.PaxiomModel.Meta.Contents);

            if (manyValuesVariableNames.Count() > 0)
            {
                result.Append(byPart);

                if (manyValuesVariableNames.Count() > 1)
                {
                    result.Append(string.Join(", ", manyValuesVariableNames.Take(manyValuesVariableNames.Count() - 1)));
                    result.Append(andPart);
                    result.Append(manyValuesVariableNames.Last());
                }
                else
                {
                    result.Append(manyValuesVariableNames.First());
                }
            }
            else
            {
                if (result.ToString().EndsWith(",")) // avoid ,. in text
                {
                    result.Remove(result.Length - 1, 1);
                }
            }

            result.Append(". ");
            result.Append(string.Join(", ", singleValueTexts));

            if (singleValueTexts.Count() > 0)
            {
                result.Append(".");
            }

            txtTitle.Text = result.ToString();
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (VerifyChartData())
            {
                pnlChart.Visible = true;              
                SetLocalizedTexts();
            }
            else
            {
                pnlChart.Visible = false;               
            }

            if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
            {
                var siteTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("SiteTitle");

                Master.HeadTitle = txtTitle.Text;
                Master.HeadTitle += ". " + siteTitle;
            }
            else
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleChart");
            }

            ChartManager.Settings.UseSettingTitle = false;
        }

        /// <summary>
        /// Checks if language has been changed. Changes title if language has been changed.
        /// </summary>
        private void IsLanguageChanged()
        {
            if (PCAxis.Web.Core.Management.LocalizationManager.CurrentCulture != ChartManager.Settings.CurrentCulture)
            {
                UpdateChartTitle();
            }
        }

        /// <summary>
        /// This method is called when the Paxiom model has been changed (when a operation has been executed for example)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaxoimModelChanged(object sender, System.EventArgs e)
        {
            UpdateChartTitle();
        }


        /// <summary>
        /// Update chart title
        /// </summary>
        private void UpdateChartTitle()
        {
            if (ChartManager.Settings.UseSettingTitle && !string.IsNullOrEmpty(ChartManager.Settings.Title))
            {
                txtTitle.Text = ChartManager.Settings.Title;
            }
            else if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
            {
                SetTitle();
            }
            UpdateSettings();
            CreateChart();
        }

        /// <summary>
        /// Verifies that the chart type can be displayed with the current PXModel
        /// </summary>
        /// <returns></returns>
        private bool VerifyChartData()
        {
            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
            {
                if (VerifyCellNumber() == false)
                {
                    pnlIllegalChart.Visible = !VerifyPieOrPyramid();
                    pnlIllegalChart2.Visible = true;
                    Master.EnableSaveQueryFeature(false);
                    return false;
                }
                if (VerifyPieOrPyramid() == false)
                {
                    pnlIllegalChart.Visible = true;
                    Master.EnableSaveQueryFeature(false);
                    return false;
                }
                Master.EnableSaveQueryFeature(true);
                return true;
            }            
            return false;            
        }
        
        private bool VerifyPieOrPyramid()
        {
            switch (GetLayout())
            {
                case Plugins.Views.CHART_PIE:
                    if (VerifyPieData() == false)
                    {
                        DisplayChartInformation(Plugins.Views.CHART_PIE);
                        return false;
                    }
                    break;
                case Plugins.Views.CHART_POPULATIONPYRAMID:
                    if (VerifyPyramidData() == false)
                    {
                        DisplayChartInformation(Plugins.Views.CHART_POPULATIONPYRAMID);
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Verifies that the number of cells in data do not exceed allowed number
        /// </summary>
        /// <returns>True if number of cells in data is ok, else false</returns>
        private bool VerifyCellNumber()
        {
            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Data.MatrixSize > PXWeb.Settings.Current.Features.Charts.MaxValues)
            {
                lblIllegalChartTitle2.Text = Master.GetLocalizedString("PxWebChartTooManyValues");
                lblIllegalChart2.Text = string.Format(Master.GetLocalizedString("PxWebChartTooManyValuesInformation"), PXWeb.Settings.Current.Features.Charts.MaxValues.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifies that a pie chart can be displayed, if not information is displayed
        /// </summary>
        /// <returns>True if a pie chart can be displayed, else false</returns>
        private bool VerifyPieData()
        {
            bool moreThanOneValue = false;

            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.Count > 1)
            {
                foreach (Variable var in PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables)
                {
                    if (var.Values.Count > 1)
                    {
                        if (moreThanOneValue == false)
                        {
                            moreThanOneValue = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Verifies that a pyramid chart can be displayed, if not information is displayed
        /// </summary>
        /// <returns>True if a pyramid chart can be displayed, else false</returns>
        private bool VerifyPyramidData()
        {
            bool twoValues = false;
            bool manyValues = false;

            foreach (PCAxis.Paxiom.Variable var in PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables)
            {
                switch (var.Values.Count)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                        if (twoValues == true)
                        {
                            return false;
                        }
                        else
                        {
                            twoValues = true;
                        }
                        break;
                    default:
                        if (manyValues == true)
                        {
                            return false;
                        }
                        else
                        {
                            manyValues = true;
                        }
                        break;
                }
            }

            if (manyValues == false || twoValues == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show information about why the specified chart cannot be displayed
        /// </summary>
        /// <param name="chart">Chart type</param>
        private void DisplayChartInformation(string chart)
        {            
            switch (chart)
            {
                case Plugins.Views.CHART_PIE:
                    lblIllegalChartTitle.Text = Master.GetLocalizedString("PxWebChartPieChartTitle");
                    lblIllegalChart.Text = Master.GetLocalizedString("PxWebChartPieChartInformation");
                    break;
                case Plugins.Views.CHART_POPULATIONPYRAMID:
                    lblIllegalChartTitle.Text = Master.GetLocalizedString("PxWebChartPyramidChartTitle");
                    lblIllegalChart.Text = Master.GetLocalizedString("PxWebChartPyramidChartInformation");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initializes CommandBar
        /// </summary>
        private void InitializeCommandBar()
        {
            Master.SetCommandBarFilter(CommandBarFilterFactory.GetFilter(CommandBarPluginFilterType.Chart.ToString()));
            Master.SetCommandBarPresentationView(GetLayout());
        }

        private IPxUrl _pxUrlObj;

        private IPxUrl PxUrlObj
        {
            get
            {
                if (_pxUrlObj == null)
                {
                    _pxUrlObj = RouteInstance.PxUrlProvider.Create(null);
                }

                return _pxUrlObj;
            }
        }

        /// <summary>
        /// Initializes chart
        /// </summary>
        private void InitializeChart()
        {
            //pnlSettings.Visible = false;

            // Default values for user settings
            ChartSettings settings = ChartManager.Settings;
            settings.ChartType = ChartSettings.ConvertToChartType(GetLayout(), settings.ChartType);
            
            if (GetLayout().Equals(Plugins.Views.CHART_COLUMNLINE))
            {
                settings.IsColumnLine = true;
            }
            else
            {
                settings.IsColumnLine = false;
            }
            if (settings.UseSettingTitle && !string.IsNullOrEmpty(settings.Title))
            {
                txtTitle.Text = settings.Title;
            }
            else if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
            {
                SetTitle();
            }

            txtHeight.Text = settings.Height.ToString();
            txtWidth.Text = settings.Width.ToString();
            rblSortTime.SelectedValue = settings.TimeSortOrder.ToString();
            txtLineThickness.Text = settings.LineThickness.ToString();
            chkShowLegend.Checked = settings.ShowLegend;
            txtLegendHeight.Text = settings.LegendHeight.ToString();
            chkHorizontalGuidelines.Checked = ((settings.Guidelines & ChartSettings.GuidelinesType.Horizontal) == ChartSettings.GuidelinesType.Horizontal);
            chkVerticalGuidelines.Checked = ((settings.Guidelines & ChartSettings.GuidelinesType.Vertical) == ChartSettings.GuidelinesType.Vertical);
            rblLabelOrientation.SelectedValue = settings.LabelOrientation.ToString();

            LegendSettings();

            ShowLineThickness();
            ShowSortTime();
            ShowLabelOrientation();
            ShowGuidelines();

            SortTime();
            CreateChart();

            //lnkChartSettings.Text = Master.GetLocalizedString("PxWebChartUserSettingsShow");
        }


        /// <summary>
        /// Shall the Line thickness setting be displayed or not?
        /// </summary>
        private void ShowLineThickness()
        {
            switch (GetLayout())
            {
                case Plugins.Views.CHART_LINE:
                case Plugins.Views.CHART_COLUMNLINE:
                case Plugins.Views.CHART_RADAR:
                    pnlLineThickness.Visible = true;
                    break;
                default:
                    pnlLineThickness.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// Shall the Sort time setting be displayed or not?
        /// </summary>
        private void ShowSortTime()
        {
            switch (GetLayout())
            {
                case Plugins.Views.CHART_PIE:
                case Plugins.Views.CHART_RADAR:
                case Plugins.Views.CHART_POPULATIONPYRAMID:
                    pnlSortTime.Visible = false;
                    break;
                default:
                    pnlSortTime.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Shall the Label orientation setting be displayed or not?
        /// </summary>
        private void ShowLabelOrientation()
        {
            switch (GetLayout())
            {
                case Plugins.Views.CHART_PIE:
                case Plugins.Views.CHART_RADAR:
                    pnlLabelOrientation.Visible = false;
                    break;
                default:
                    pnlLabelOrientation.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Shall the Guidelines settings be displayed or not?
        /// </summary>
        private void ShowGuidelines()
        {
            switch (GetLayout())
            {
                case Plugins.Views.CHART_PIE:
                    pnlGuidelines.Visible = false;
                    break;
                default:
                    pnlGuidelines.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// display texts in selected language
        /// </summary>
        private void SetLocalizedTexts()
        {
            
            foreach (ListItem li in rblSortTime.Items)
            {
                switch (li.Value.ToLower())
                {
                    case "ascending":
                        li.Text = Master.GetLocalizedString("PxWebChartUserSettingsSortTimeAscending");
                        break;
                    case "descending":
                        li.Text = Master.GetLocalizedString("PxWebChartUserSettingsSortTimeDescending");
                        break;
                    case "none":
                        li.Text = Master.GetLocalizedString("PxWebChartUserSettingsSortTimeNone");
                        break;
                }
            }
           
            foreach (ListItem li in rblLabelOrientation.Items)
            {
                switch (li.Value.ToLower())
                {
                    case "horizontal":
                        li.Text = Master.GetLocalizedString("PxWebChartUserSettingsLabelOrientationHorizontal");
                        break;
                    case "vertical":
                        li.Text = Master.GetLocalizedString("PxWebChartUserSettingsLabelOrientationVertical");
                        break;
                }
            }
            

            
            //btnApply.Text = Master.GetLocalizedString("PxWebChartUserSettingsApply");
        }

        /// <summary>
        /// Create the chart image
        /// </summary>
        void CreateChart()
        {
            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
            {
                chartImage.ImageUrl = CreateChartLink("screen");
                chartImage.AlternateText = txtTitle.Text;
            }
        }

        /// <summary>
        /// Sort timevariable 
        /// </summary>
        private void SortTime()
        {
            if (rblSortTime.SelectedValue != "None")
            {
                // Only sort time when selection is changed (we do not want to this every time the chart is created)
                if (rblSortTime.SelectedValue != ChartManager.Settings.TimeSortOrder.ToString())
                {
                    SortTimeVariable sortOperation = new SortTimeVariable();
                    SortTimeVariableDescription sortDescription;

                    switch (rblSortTime.SelectedValue.ToLower())
                    {
                        case "ascending":
                            sortDescription = new SortTimeVariableDescription(SortTimeVariableDescription.SortOrderType.Ascending);
                            ChartManager.Settings.TimeSortOrder = ChartSettings.SortType.Ascending;
                            break;
                        case "descending":
                            sortDescription = new SortTimeVariableDescription(SortTimeVariableDescription.SortOrderType.Descending);
                            ChartManager.Settings.TimeSortOrder = ChartSettings.SortType.Descending;
                            break;
                        default:
                            sortDescription = new SortTimeVariableDescription(SortTimeVariableDescription.SortOrderType.None);
                            ChartManager.Settings.TimeSortOrder = ChartSettings.SortType.None;
                            break;
                    }
                    PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = sortOperation.Execute(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel, sortDescription);
                    PaxiomManager.OperationsTracker.AddStep("SORT_TIME", sortDescription);
                }
            }

        }

        /// <summary>
        /// Create URL to chart service
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Chart URL</returns>
        private string CreateChartLink(String format)
        {
            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
            {
                List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();

                linkItems.Add(new LinkManager.LinkItem(ChartParameters.CHARTTYPE, GetLayout()));
                linkItems.Add(new LinkManager.LinkItem(ChartParameters.TITLE, HttpUtility.UrlEncode(txtTitle.Text)));
                linkItems.Add(new LinkManager.LinkItem(ChartParameters.WIDTH, txtWidth.Text));
                linkItems.Add(new LinkManager.LinkItem(ChartParameters.HEIGHT, txtHeight.Text));
                linkItems.Add(new LinkManager.LinkItem(ChartParameters.LINE_THICKNESS, txtLineThickness.Text));
                linkItems.Add(new LinkManager.LinkItem(ChartParameters.LABELORIENTATION, rblLabelOrientation.SelectedValue));
                if (chkHorizontalGuidelines.Checked)
                {
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.GUIDELINES_HORIZONTAL, "true"));
                }
                else
                {
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.GUIDELINES_HORIZONTAL, "false"));
                }
                if (chkVerticalGuidelines.Checked)
                {
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.GUIDELINES_VERTICAL, "true"));
                }
                else
                {
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.GUIDELINES_VERTICAL, "false"));
                }
                if (chkShowLegend.Checked)
                {
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.LEGEND, "true"));
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.LEGENDHEIGHT, txtLegendHeight.Text));
                }
                else
                {
                    linkItems.Add(new LinkManager.LinkItem(ChartParameters.LEGEND, "false"));
                }
                // Add query string parameter with current time to force IE9 to reload image. 
                linkItems.Add(new LinkManager.LinkItem("ver", HttpUtility.UrlEncode(DateTime.Now.ToLongTimeString())));

                return PCAxis.Web.Core.Management.LinkManager.DefaultCreateLink("ChartHandler.ashx", false, linkItems.ToArray());
            }

            return "";
        }

        /// <summary>
        /// Validate if title is valid
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateTitle(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateNoIllegalCharcters(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }
        }

        /// <summary>
        /// Validate if height is valid
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateHeight(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (int.Parse(txtHeight.Text) > PXWeb.Settings.Current.Features.Charts.MaxHeight)
            {
                SetValidationError(val, args, "PxWebChartUserSettingsLargeValue", PXWeb.Settings.Current.Features.Charts.MaxHeight.ToString());
                return;
            }

        }

        /// <summary>
        /// Validate if width value is valid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void ValidateWidth(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (int.Parse(txtWidth.Text) > PXWeb.Settings.Current.Features.Charts.MaxWidth)
            {
                SetValidationError(val, args, "PxWebChartUserSettingsLargeValue", PXWeb.Settings.Current.Features.Charts.MaxWidth.ToString());
                return;
            }
        }

        /// <summary>
        /// Validate if line thickness value is valid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void ValidateLineThickness(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            if (int.Parse(txtLineThickness.Text) > PXWeb.Settings.Current.Features.Charts.MaxLineThickness)
            {
                SetValidationError(val, args, "PxWebChartUserSettingsLargeValue", PXWeb.Settings.Current.Features.Charts.MaxLineThickness.ToString());
                return;
            }
        }

        /// <summary>
        /// Validate if legend height value is valid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void ValidateLegendHeight(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
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
        /// Set error message for validator with invalid value
        /// </summary>
        /// <param name="val">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="errorKey">Key for error message</param>
        /// <param name="parameters">Eventual parameters to the localized string</param>
        private void SetValidationError(CustomValidator val, System.Web.UI.WebControls.ServerValidateEventArgs args, string errorKey, params string[] parameters)
        {
            args.IsValid = false;
            val.Text = GetErrorText();
            if (parameters.Length > 0)
            {
                val.ErrorMessage = GetErrorText() + string.Format(Master.GetLocalizedString(errorKey), parameters);
            }
            else
            {
                val.ErrorMessage = GetErrorText() + Master.GetLocalizedString(errorKey);
            }
            IncrementErrorNumber();
        }

        /// <summary>
        /// Get error indicator text (validation of user settings input)
        /// </summary>
        /// <returns></returns>
        private string GetErrorText()
        {
            return _errorNumber.ToString() + ") ";
        }

        /// <summary>
        /// Increment the error indicator text (validation of user settings input)
        /// </summary>
        private void IncrementErrorNumber()
        {
            _errorNumber += 1;
        }

        ///// <summary>
        ///// Show/hide chart settings link has been clicked
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ChartSettings_Click(object sender, EventArgs e)
        //{
        //    if (pnlSettings.Visible == false)
        //    {
        //        pnlSettings.Visible = true;
        //        lnkChartSettings.Text = Master.GetLocalizedString("PxWebChartUserSettingsHide");
        //    }
        //    else
        //    {
        //        pnlSettings.Visible = false;
        //        lnkChartSettings.Text = Master.GetLocalizedString("PxWebChartUserSettingsShow");
        //    }
        //}

        /// <summary>
        /// Apply chart settings button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplySettings_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string title;

                // Title may be reset if the SortTime opertation results in the PX-model being changed.
                // Therefore we need to handle title in special way...
                title = txtTitle.Text;
                SortTime();
                txtTitle.Text = title;
                UpdateSettings();
                CreateChart();
            }
        }

        /// <summary>
        /// Show/hide legend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowLegend_CheckedChanged(object sender, EventArgs e)
        {
            LegendSettings();
        }

        /// <summary>
        /// Set legend settings enabled/disabled depending on the value of 'Show legend'
        /// </summary>
        private void LegendSettings()
        {
            if (chkShowLegend.Checked)
            {
                lblLegendHeight.Enabled = true;
                txtLegendHeight.Enabled = true;
            }
            else
            {
                lblLegendHeight.Enabled = false;
                txtLegendHeight.Enabled = false;
            }
        }

        private void UpdateSettings()
        {
            ChartSettings settings = ChartManager.Settings;
            settings.CurrentCulture = PCAxis.Web.Core.Management.LocalizationManager.CurrentCulture;
            settings.ChartType = ChartSettings.ConvertToChartType(GetLayout(), settings.ChartType);
            //settings.Title = txtTitle.Text;
            int t;
            settings.Width = int.TryParse(txtWidth.Text, out t) ? t : settings.Width;
            settings.Height = int.TryParse(txtHeight.Text, out t) ? t : settings.Height;
            settings.LineThickness = int.TryParse(txtLineThickness.Text, out t) ? t : settings.LineThickness;
            settings.LabelOrientation = ChartSettings.ConvertToLabelOrientation(rblLabelOrientation.SelectedValue, settings.LabelOrientation);
            settings.Guidelines = ChartSettings.GuidelinesType.None;
            if (chkVerticalGuidelines.Checked) { settings.Guidelines = settings.Guidelines | ChartSettings.GuidelinesType.Vertical; }
            if (chkHorizontalGuidelines.Checked) { settings.Guidelines = settings.Guidelines | ChartSettings.GuidelinesType.Horizontal; }
            settings.ShowLegend = chkShowLegend.Checked;
            settings.LegendHeight = int.TryParse(txtLegendHeight.Text, out t) ? t : settings.LegendHeight;
        }

        private void SetDisplayModeOnPanels()
        {
            if (!string.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter(PanelLink.DISPLAY_PANEL)))
            {
                if (QuerystringManager.GetQuerystringParameter(PanelLink.DISPLAY_PANEL).Equals("chartsettings"))
                {
                    pnlSettings.Style.Add("display", "inline-block");
                    lnkShowChartSettings.NavigateUrl = PanelLink.BuildLink("");
                    imgSettingsExpander.CssClass = "px-settings-collapseimage";
                    return;
                }
            }

            pnlSettings.Style.Add("display", "none");
            lnkShowChartSettings.NavigateUrl = PanelLink.BuildLink("chartsettings");
            imgSettingsExpander.CssClass = "px-settings-expandimage";
        }

    }
}
