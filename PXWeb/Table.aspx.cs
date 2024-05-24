using PCAxis.Paxiom;
using PCAxis.Web.Controls;
using PCAxis.Web.Controls.CommandBar.Plugin;
using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace PXWeb
{
    public partial class Table : System.Web.UI.Page
    {
        private static log4net.ILog FeatureUsageLogger = log4net.LogManager.GetLogger("FeatureUsage");

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

        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;

            if (!IsPostBack)
            {
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
                {
                    InitializeCommandBar();
                    InitializeTableInformation();
                    InitializeTable();
                }
            }
            Table1.PxTableCroppedEvent += new EventHandler(HandlePxTableCroppedEvent);

            lblTableCropped.Visible = false;
            lblTableCroppedHeading.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
            {
                var siteTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("SiteTitle");

                if (PaxiomManager.PaxiomModel.Meta.DescriptionDefault && !string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.Description))
                {
                    Master.HeadTitle = PaxiomManager.PaxiomModel.Meta.Description;
                }
                else
                {
                    Master.HeadTitle = PaxiomManager.PaxiomModel.Meta.Title;
                }

                Master.HeadTitle += ". " + siteTitle;
            }
            else
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleTable");
            }

            if (!IsPostBack)
            {
                // Show mandatory footnotes?
                if (Table1.PromptForMandatoryFootnotes && (Request.Browser.EcmaScriptVersion.Major >= 1))
                {
                    List<FootnoteListItem> lstMandatoryFootnotes = FootnoteList.GetFootnoteList(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta, true);

                    if (lstMandatoryFootnotes != null)
                    {
                        if (lstMandatoryFootnotes.Count > 0)
                        {
                            if (!MandatoryFootnotesDisplayed())
                            {
                                ShowFootnoteDialog(true, false);

                                // Remember that mandatory footnotes have been displayed for this table
                                if (!string.IsNullOrEmpty(PxUrlObj.Table))
                                {
                                    Session["promptmandatoryfootnotes"] = PxUrlObj.Table;
                                }
                            }
                        }
                    }
                }
            }
            SettingsLabel.Text = Master.GetLocalizedString("PxWebTableUserSettingsShow");
            pnlForRblZeroOption.GroupingText = "<span class='font-heading'>" + Master.GetLocalizedString("PxWebTableUserSettingsLegend") + "</span>"; ;

        }

        /// <summary>
        /// Determines if mandatory footnotes have already been displayed or not
        /// </summary>
        /// <returns>True if mandatory footnotes have been displayed, else false</returns>
        private bool MandatoryFootnotesDisplayed()
        {
            if (Session["promptmandatoryfootnotes"] == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(PxUrlObj.Table))
            {
                return false;
            }

            if (Session["promptmandatoryfootnotes"].ToString().Equals(PxUrlObj.Table))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Show the footnote dialog
        /// </summary>
        /// <param name="showMandatoryOnly">If only mandatory footnotes shall be displayed</param>
        /// <param name="showNoFootnotes">If the text "No footnotes" shall be displayed if there are no footnotes</param>
        public void ShowFootnoteDialog(bool showMandatoryOnly, bool showNoFootnotes)
        {
            dialogModal.Visible = true;

            dialogModal.Attributes["title"] = Master.GetLocalizedString("PxWebFootnotes");
            Footnote1.ShowMandatoryOnly = showMandatoryOnly;
            Footnote1.ShowNoFootnotes = showNoFootnotes;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowModalDialog", "<script type='text/javascript'> jQuery(function() { jQuery('#" + dialogModal.ClientID + "').dialog({ width: 480, height: 400,  modal: true, buttons: {" + Master.GetLocalizedString("PxWebPopupDialogClose") + ": function () {jQuery(this).dialog('close');} } }); });</script>  ");
        }

        /// <summary>
        /// Initializes CommandBar
        /// </summary>
        private void InitializeCommandBar()
        {
            Master.SetCommandBarFilter(CommandBarFilterFactory.GetFilter(CommandBarPluginFilterType.TableLayout1.ToString()));
            Master.SetCommandBarPresentationView(PxUrlObj.Layout);
        }

        /// <summary>
        /// Initializes the TableInformation web controls
        /// </summary>
        private void InitializeTableInformation()
        {
            TableInformationView.ShowSourceDescription = PXWeb.Settings.Current.General.Global.ShowSourceDescription;
            TableInfo.ShowSourceDescription = PXWeb.Settings.Current.General.Global.ShowSourceDescription;
        }

        /// <summary>
        /// Initializes the table
        /// </summary>
        private void InitializeTable()
        {
            // Presentation.General settings
            Table1.PromptForMandatoryFootnotes = PXWeb.Settings.Current.Presentation.PromptMandatoryFootnotes;

            // Presentation.Table settings
            switch (PxUrlObj.Layout)
            {
                case Plugins.Views.TABLE_LAYOUT1:
                    Table1.Layout = TableLayoutType.Layout1;
                    break;
                case Plugins.Views.TABLE_LAYOUT2:
                    Table1.Layout = TableLayoutType.Layout2;
                    break;
                default:
                    Table1.Layout = PXWeb.Settings.Current.Presentation.Table.DefaultLayout;
                    break;
            }
            Table1.TableTransformation = PXWeb.Settings.Current.Presentation.Table.TableTransformation;
            Table1.MaxColumns = PXWeb.Settings.Current.Presentation.Table.MaxColumns;
            Table1.MaxRows = PXWeb.Settings.Current.Presentation.Table.MaxRows;
            Table1.TitleVisible = PXWeb.Settings.Current.Presentation.Table.TitleVisible;
            Table1.NewTitleLayout = PXWeb.Settings.Current.Presentation.NewTitleLayout;
            Table1.DisplayCellInformation = PXWeb.Settings.Current.Presentation.Table.Attributes.DisplayAttributes;
            Table1.DisplayDefaultAttributes = PXWeb.Settings.Current.Presentation.Table.Attributes.DisplayDefaultAttributes;

            // General.Global settings
            Table1.DataNotePlacement = PXWeb.Settings.Current.General.Global.DataNotePlacement;
            Table1.UseUpperCase = PXWeb.Settings.Current.General.Global.Uppercase;
            Table1.RemoveRowsOption = TableManager.Settings.ZeroOption;

            //pnlSettings.Visible = false;
            rblZeroOption.SelectedValue = TableManager.Settings.ZeroOption.ToString();
        }


        /// <summary>
        /// Handle Tablecropped event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandlePxTableCroppedEvent(object sender, EventArgs e)
        {
            lblTableCropped.Text = String.Format(PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTableCropped"),
                                        PXWeb.Settings.Current.Presentation.Table.MaxRows.ToString(),
                                        PXWeb.Settings.Current.Presentation.Table.MaxColumns.ToString());

            lblTableCroppedHeading.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTableCroppedHeading");
            tableMessagePanel.Visible = true;
            lblTableCropped.Visible = true;
            lblTableCroppedHeading.Visible = true;
            Master.ShowMessages(true);
        }

        /// <summary>
        /// Apply chart settings button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplySettings_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                TableManager.Settings.ZeroOption = (ZeroOptionType)Enum.Parse(typeof(ZeroOptionType), rblZeroOption.SelectedValue, true);
                Table1.RemoveRowsOption = TableManager.Settings.ZeroOption;
                logFeatureUsage();
            }
        }

        private void logFeatureUsage()
        {
            if (string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.TableID))
            {
                FeatureUsageLogger.InfoFormat(LogFormat.FEATURE_USAGE_LOG_FORMAT_PXFILE, "HideRows",
                  TableManager.Settings.ZeroOption.ToString(), PaxiomManager.PaxiomModel.Meta.Matrix);
            }
            else
            {
                FeatureUsageLogger.InfoFormat(LogFormat.FEATURE_USAGE_LOG_FORMAT_CNMM, "HideRows",
                  TableManager.Settings.ZeroOption.ToString(), PaxiomManager.PaxiomModel.Meta.TableID);
            }
        }

    }
}
