using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Paxiom;
using PCAxis.Web.Core.Management;
using PCAxis.Web.Controls.CommandBar.Plugin;
using PCAxis.Web.Controls;
using PXWeb.Views;

namespace PXWeb.UserControls
{
    public partial class SaveQueryCreate : System.Web.UI.UserControl 
    {
        
        private const string SAVE_AS_CAPTION = "CtrlSaveQueryAsCaption";
        private const string SAVEQUERY_FOR_SCREEN = "CtrlSaveQueryForScreen";
        private const string SAVEQUERY_CREATEQUERY = "CtrlSaveQuerybtnCreateQuery";
        private const string SAVEQUERY_TIMEOPTIONSWARNING = "CtrlSaveQueryTimeWarning";
        private const string SAVEQUERY_NOTIMEVALWARNING = "CtrlSaveQueryNoTimeValWarning";
        private const string SAVEQUERY_HELP_PAGE = "CtrlSaveQueryHelpPage";
        private const string SAVEQUERY_RBL_LEGEND = "CtrlSaveQueryChoosetimeperiodInformation";
        private const string SAVEQUERY_COPY_LINK_COPIED= "CtrlSaveQueryCopyLinkCopied";


        #region Private property

        private ICommandBarPluginFilter _outputFilter = null;         
        private string _tableTitle;

        // What outpotformats will be available
        public ICommandBarPluginFilter OutputFilter
        {
            get { return _outputFilter; }
            set { _outputFilter = value; }
        
        }
        //Subject for the mail
        public string Subject
        {
            get { return _tableTitle = GetTitleForMailSubjekt(); }    
        }

        private bool _enabled = true;
        public bool Enabled 
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                pnl2_SaveQuerySelection.Enabled = _enabled;
            }
        }

        public string CopyLinkCopied
        {
            get { return LocalizationManager.GetLocalizedString(SAVEQUERY_COPY_LINK_COPIED); }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                imgTimeWarning.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(BreadcrumbCodebehind), "PCAxis.Web.Controls.spacer.gif");
                FillDropDownlist();
                
                pnlForRbl.GroupingText = "<span class='font-heading'>" + LocalizationManager.GetLocalizedString(SAVEQUERY_RBL_LEGEND) + "</span>";
                lblResultAs.Text = LocalizationManager.GetLocalizedString(SAVE_AS_CAPTION);
            }
            SetDisplayModeOnPanels();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetTimeOptions();
        }

        private void SetTimeOptions()
        {
            //TODO: Maria om timeval är Null finns ingen variabel definierad för tid
            var timeval = PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(v => v.IsTime);

            if (PaxiomManager.OperationsTracker.IsTimeDependent || timeval == null)
            {
                rblTimePeriod.Items[0].Enabled = false;
                rblTimePeriod.Items[1].Enabled = false;
                rblTimePeriod.SelectedIndex = 2;
                divTimeWarning.Visible = true;

                //if timeval is involve in a operation 
                if (PaxiomManager.OperationsTracker.IsTimeDependent)
                {
                    lblTimeWarning.Text = LocalizationManager.GetLocalizedString(SAVEQUERY_TIMEOPTIONSWARNING);
                }
                //if no timeval exists (timeval == null) the time can´t be updated.
                else if (timeval == null)
                {
                    lblTimeWarning.Text = LocalizationManager.GetLocalizedString(SAVEQUERY_NOTIMEVALWARNING);
                }
            }
            else
            {
                divTimeWarning.Visible = false;
            }



        }

        #region "Private methods"
        private void FillDropDownlist()
        {
            //Add select text
            ddlOutputFormats.Items.Add(new ListItem(LocalizationManager.GetLocalizedString("CtrlSaveQuerySelectFormat"), "selectFormat"));
            
            List<string> _outputFormats = new List<string>();
            _outputFormats = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormats;
            //Add a heading for dropdown
            //SaveQueryAsDropDownList.Items.Add(LocalizationManager.GetLocalizedString(SAVE_AS_CAPTION));
            //Add choice to save query for screen
            var screenItem = new ListItem(LocalizationManager.GetLocalizedString(SAVEQUERY_FOR_SCREEN), "SCREEN");
            screenItem.Selected = true; //Default selected
            ddlOutputFormats.Items.Add(screenItem);

            if (OutputFilter != null)
            {
                foreach (string item in _outputFormats)
                {
                    if (OutputFilter.UseOutputFormat(item))
                    {
                        ddlOutputFormats.Items.Add(new ListItem(LocalizationManager.GetLocalizedString(item), item));
                    }
                }
            }
        }
        /// <summary>
        /// Create the url for the saved query. The url thaks the user back to the same query. 
        /// If the query includes a time variable, set the time filter to the choice the user have done 
        /// in the radiobuttonlist. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateSavedQueryUrl(Object sender, EventArgs e) 
        {
            txtSaveQueryUrl.Text = "";
            lblError.Visible = false;
            lblFormatError.Text = "";
            lblFormatError.Style.Add("visibility", "hidden");
            //Remove css class for red border highlighting error
            ddlOutputFormats.CssClass = ddlOutputFormats.CssClass.Replace("saveas_dropdownlist_error", "");
            string output = ddlOutputFormats.SelectedValue;

            // Output format must be selected
            if (output.Equals("selectFormat"))
            {
                lblFormatError.Text = LocalizationManager.GetLocalizedString("CtrlSaveQuerySelectFormatMissing");
                //Add css class for red border highlighting error
                ddlOutputFormats.CssClass = "saveas_dropdownlist_error";
                lblFormatError.Style.Add("visibility", "visible");
                pnl2_SaveQuerySelection.Style.Add("display", "inline-block");
                //Keeps accordion open
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openAccordion",
                    "jQuery(document).ready(function(){openAccordion('SaveQueryHeader', 'SaveQueryBody')});", true);
                return;
            }

            lblUpdateSummaryValue.Text = rblTimePeriod.SelectedItem.Text;
            lblOutputSummaryValue.Text = ddlOutputFormats.SelectedItem.Text;

            string time = rblTimePeriod.SelectedValue;
            try
            { 
            
                var query = GetQueries();


                //start jfi 
                foreach(var queryVariable in query.Quieries)
                {
                    var paxiomVariable = PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(v => v.Code.Equals(queryVariable.Code));
                    if (paxiomVariable == null)
                    {
                        if (string.IsNullOrEmpty(queryVariable.VariableType))
				queryVariable.VariableType = "N";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(queryVariable.VariableType))
				queryVariable.VariableType = this.GetVariableTypeCode(paxiomVariable);
                    }
                }
                //end jfi 

                var tvar = PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(v => v.IsTime);
                //No defined time variable in the model, skip time filter 
                if (tvar != null)
                {

                    var qq = query.Quieries.FirstOrDefault(q => q.Code == tvar.Code);
                    if (qq == null)
                    {
                        //Will (also?) get here if all timevaules is selected
                        qq = new PCAxis.Query.Query();
                        qq.Code = tvar.Code;
                        qq.VariableType = "T";
                        qq.Selection = new PCAxis.Query.QuerySelection();
                        qq.Selection.Filter = "all";
                        qq.Selection.Values = new string[] { "*" };
                        query.Quieries.Add(qq);
                    }

                    if (time == "item")
                    {
                        //Do Nothing
                    }
                    else if (time == "top")
                    {
                        qq.Selection.Filter = "top";
                        qq.Selection.Values = new string[] { tvar.Values.Count.ToString() };
                    }
                    else if (time == "from")
                    {
                        qq.Selection.Filter = "from";
                        var minValue = tvar.Values[0];
                        for (int i = 1; i < tvar.Values.Count; i++)
                        {
                            if (string.Compare(minValue.TimeValue, tvar.Values[i].TimeValue) > 0)
                            {
                                minValue = tvar.Values[i];
                            }
                        }
                        qq.Selection.Values = new string[] { minValue.Code };
                    }
                }

                PCAxis.Query.SavedQuery sq = new PCAxis.Query.SavedQuery();

                sq.Sources.Add(query);

                if (output == "SCREEN")
                {
                    sq.Output = ViewSerializerCreator.GetSerializer().Save();
                }
                else
                {
                    sq.Output = ViewSerializerCreator.GetSerializer(output).Save();
                }
                sq.TimeDependent = PaxiomManager.OperationsTracker.IsTimeDependent;

                sq.Workflow.AddRange(PaxiomManager.OperationsTracker.GetSteps().Where(ws => !ws.Type.ToUpper().StartsWith("PIVOT")).ToArray());

                //Due to convert json -> pxs we will always require manual worksteps as workflow.
                sq.Workflow.AddRange(GetManualWorkStepsForModel());


                string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/queries/");
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add(PCAxis.Query.SavedQueryManager.SAVE_PARAMETER_PATH, path); // Save query to this directory
                
                string name = PCAxis.Query.SavedQueryManager.Current.Save(sq, parameters);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (RouteInstance.RouteExtender == null)
                    {
                        txtSaveQueryUrl.Text = GetAppPath() + "sq/" + name;
                    }
                    else
                    {
                        txtSaveQueryUrl.Text = GetAppPath() + RouteInstance.RouteExtender.GetSavedQueryPath(PxUrl.Language, name);
                    }
                }
                else
                { 
                    lblError.Visible = true;
                }
                //Hide and show the right panels
                pnl2_SaveQuerySelection.Style.Add("display", "none");
                pnl3_ShowSaveQueryUrl.Style.Add("display", "inline-block");
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
            }
            finally
            {
                //Keeps accordion open
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openAccordion",
                    "jQuery(document).ready(function(){openAccordion('SaveQueryHeader', 'SaveQueryBody')});", true);
            }
        }
        
        private PCAxis.Query.WorkStep[] GetManualWorkStepsForModel()
        {
            //name is used as reference/identificator for variables due to some legacy(which can cause problems if name changes in db....)
            var result = new List<PCAxis.Query.WorkStep>();
            var pd = new List<PCAxis.Paxiom.Operations.PivotDescription>();

            foreach(var variable in PaxiomManager.PaxiomModel.Meta.Heading)
            {
                pd.Add(new PCAxis.Paxiom.Operations.PivotDescription(variable.Name, PCAxis.Paxiom.PlacementType.Heading));
            }

            foreach (var variable in PaxiomManager.PaxiomModel.Meta.Stub)
            {
                pd.Add(new PCAxis.Paxiom.Operations.PivotDescription(variable.Name, PCAxis.Paxiom.PlacementType.Stub));
            }

            PCAxis.Query.Serializers.IOperationSerializer ser = PCAxis.Query.OperationsTracker.CreateSerializer(PCAxis.Paxiom.Operations.OperationConstants.PIVOT);

            var manualWOrkStep = ser.Serialize(pd.ToArray());
            result.Add(manualWOrkStep);
            
            return result.ToArray();
        }

        private string GetVariableTypeCode(Variable tvar)
        {
            if(tvar.IsTime)
            {
                return "T";
            }
            else if (tvar.IsContentVariable)
            {
                return "C";
            }
            else if (! String.IsNullOrEmpty(tvar.Map))
            {
                return "G";
            }
            else
            {
                return "N";
            } 
            //Hva er tvar.VariableType?
        }

        private IPxUrl _pxUrl = null;
        private IPxUrl PxUrl
        {
            get
            {
                if (_pxUrl == null)
                {
                    _pxUrl = RouteInstance.PxUrlProvider.Create(null);
                }

                return _pxUrl;
            }
        }

        private PCAxis.Query.TableSource GetQueries()
        {
            PCAxis.Query.TableSource src = new PCAxis.Query.TableSource();

            src.Id = "1";
            src.Default = true;
            src.DatabaseId = PxUrl.Database;
            src.Language = PxUrl.Language;

            string path = PxUrl.Path;
            string table = PxUrl.Table;

            src.Source = path.Replace(PathHandler.NODE_DIVIDER, "/") + "/" + table;

            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(src.DatabaseId);
            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
            {
                src.Type = "CNMM";
            }
            else
            {
                src.Type = "PX";
            }
                        
            src.SourceIdType = "path";



            src.Quieries.AddRange(
                PaxiomManager.QueryModel.Query.Select(
                    q => new PCAxis.Query.Query() 
                        {Code = q.Code, 
                         Selection = new PCAxis.Query.QuerySelection() 
                            { Filter = q.Selection.Filter, Values = q.Selection.Values }  }).ToArray());

            return src;
        }
        /// <summary>
        /// Get the allpication path for the url to the saved query
        /// </summary>
        /// <returns></returns>
        private String GetAppPath()
        {
            string appPath = String.Empty;
            HttpContext context = HttpContext.Current;

            // UrlReferrer handles the case when PxWeb is installed on load balanced servers
            string scheme = !string.IsNullOrWhiteSpace(context.Request.UrlReferrer?.Scheme) ? context.Request.UrlReferrer.Scheme : context.Request.Url.Scheme;
            
            string host = context.Request.Url.Host;
            
            int port = context.Request.UrlReferrer is null ? context.Request.Url.Port : context.Request.UrlReferrer.Port;

            string applicationPath = context.Request.ApplicationPath;

            string portPart = (port == 80 && scheme == "http") || (port == 443 && scheme == "https") ? string.Empty : ":" + port;

            appPath = $"{scheme}://{host}{portPart}{applicationPath}";

            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }
            return appPath;
        }

        /// <summary>
        /// The subject for the mail that sends the saved query url and the name of page if bookmark funktion is availible.
        /// </summary>
        /// <returns></returns>
        private string GetTitleForMailSubjekt()
        {
            string subject = "Saved query";
            if (PXWeb.Settings.Current.Selection.TitleFromMenu)
            {
                IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                PCAxis.Menu.Item currentItem = PXWeb.Management.PxContext.GetMenuItem(url.Database, url.TablePath);
                subject = currentItem.Text;
            }
            else
            {
                subject = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Data.Model.Meta.Title;
            }


            return subject;
        }
        /// <summary>
        /// Depending on which link the user clicks on the panels will be set to visible or not. 
        /// Used if the script is disabled. 
        /// </summary>
        private void SetDisplayModeOnPanels()
        {
            if (this.Visible)
            {
                pnl2_SaveQuerySelection.Style.Add("display", "inline-block");
                pnl3_ShowSaveQueryUrl.Style.Add("display", "none");
            }        
        }
        #endregion
    }
}
