using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Web.Controls;
using PCAxis.Web.Core.Management;
using PCAxisPlugins = PCAxis.Web.Controls.CommandBar.Plugin;

namespace PXWeb
{
    public partial class Presentation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(LinkManager.CreateLink("~/Table.aspx"));

            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            ((PxWeb)this.Master).FooterText = "Presentation";

            if (!IsPostBack)
            {
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
                {
                    InitializeCommandBar();
                    InitializeTableInformation();
                    InitializeTable();
                    InitializeInformationAndFootnotes();
                }
            }
            CommandBar1.PxActionEvent += new PCAxis.Web.Controls.PxActionEventHandler(HandlePxAction);
            Table1.PxTableCroppedEvent += new EventHandler(HandlePxTableCroppedEvent);

            lblTableCropped.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
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
                                if (!string.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_tableid")))
                                {
                                    Session["promptmandatoryfootnotes"] = QuerystringManager.GetQuerystringParameter("px_tableid");
                                }
                            }
                        }
                    }
                }
            }
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
            if (string.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_tableid")))
            {
                return false;
            }

            if (Session["promptmandatoryfootnotes"].ToString().Equals(QuerystringManager.GetQuerystringParameter("px_tableid")))
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

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowModalDialog", "<script type='text/javascript'> jQuery(function() { jQuery('#" + dialogModal.ClientID + "').dialog({ width: 480, height: 400,  modal: true }); });</script>  ");
        }

        /// <summary>
        /// Initializes CommandBar
        /// </summary>
        private void InitializeCommandBar()
        {
            CommandBarSettings.InitializeCommandBar(CommandBar1);
            if (Table1.Layout == TableLayoutType.Layout1)
            {
                CommandBar1.CommandBarFilter = PCAxisPlugins.CommandBarFilterFactory.GetFilter(PCAxisPlugins.CommandBarPluginFilterType.TableLayout1.ToString());
            }
            else
            {
                CommandBar1.CommandBarFilter = PCAxisPlugins.CommandBarFilterFactory.GetFilter(PCAxisPlugins.CommandBarPluginFilterType.TableLayout2.ToString());
            }
            switch (CommandBar1.ViewMode)
            {
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden:
                    CommandBar1.Visible = false;
                    break;
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown:
                    CommandBar1.Operations = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.Operations;
                    CommandBar1.OperationShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OperationShortcuts;
                    CommandBar1.OutputFormats = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormats;
                    CommandBar1.FileformatShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormatShortcuts;
                    CommandBar1.PresentationViews = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.PresentationViews;
                    CommandBar1.PresentationViewShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.PresentationViewShortcuts;
                    CommandBar1.CommandbarShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.CommandBarShortcuts;
                    break;
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.ImageButtons:
                    CommandBar1.OperationButtons = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OperationButtons;
                    CommandBar1.FiletypeButtons = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.FileTypeButtons;
                    CommandBar1.CommandbarShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.CommandBarShortcuts;
                    break;
                default:
                    CommandBar1.Visible = false;
                    break;
            }
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
            switch (QuerystringManager.GetQuerystringParameter("tablelayout"))
            {
                case "layout1":
                    Table1.Layout = TableLayoutType.Layout1;
                    break;
                case "layout2":
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
            Table1.DisplayCellInformation = PXWeb.Settings.Current.Presentation.Table.Attributes.DisplayAttributes;
            Table1.DisplayDefaultAttributes = PXWeb.Settings.Current.Presentation.Table.Attributes.DisplayDefaultAttributes;
            
            // General.Global settings
            Table1.DataNotePlacement = PXWeb.Settings.Current.General.Global.DataNotePlacement;
            Table1.UseUpperCase = PXWeb.Settings.Current.General.Global.Uppercase;
        }

        /// <summary>
        /// Initializes the Information and Footnote controls
        /// </summary>
        private void InitializeInformationAndFootnotes()
        {
            Footnotes.Visible = true;
            Information.Visible = true;
            Information.ShowInformationTypes = PXWeb.Settings.Current.General.Global.ShowInformationTypes.GetSelectedInformationTypes();
            Footnotes.ShowMandatoryOnly = false;
            Footnotes.ShowNoFootnotes = false;

            switch (PXWeb.Settings.Current.General.Global.TableInformationLevel)
            {
                case PCAxis.Paxiom.InformationLevelType.AllFootnotes:
                    Information.Visible = false;
                    break;
                case PCAxis.Paxiom.InformationLevelType.AllInformation:
                    break;
                case PCAxis.Paxiom.InformationLevelType.MandantoryFootnotesOnly:
                    Footnotes.ShowMandatoryOnly = true;
                    Information.Visible = false;
                    break;
                case PCAxis.Paxiom.InformationLevelType.None:
                    Footnotes.Visible = false;
                    Information.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handle PX-actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandlePxAction(object sender, PCAxis.Web.Controls.PxActionEventArgs e)
        {
            //TODO: Add logging of user actions...
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
            lblTableCropped.Visible = true;
        }
    }
}
