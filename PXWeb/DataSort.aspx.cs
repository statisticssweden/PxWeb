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
using PCAxis.Web.Core.Management;
using PCAxisPlugins = PCAxis.Web.Controls.CommandBar.Plugin;
using PCAxis.Web.Controls;

namespace PXWeb
{
    public partial class DataSort : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            //((PxWeb)this.Master).FooterText = "Footnote";

            if (!IsPostBack)
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleDataSort");
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
                {
                    InitializeCommandBar();
                }
            }

            Table.Layout = PCAxis.Web.Controls.TableLayoutType.Layout2;
            Table.TableTransformation = PCAxis.Web.Controls.TableTransformationType.Sort;
            //Table.MaxColumns = PXWeb.Settings.Current.Presentation.Table.MaxColumns;
            //Table.MaxRows = PXWeb.Settings.Current.Presentation.Table.MaxRows;

            lblTableTitle.Text = PCAxis.Util.GetModelTitle(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel);
            lblDescription.Text =  LocalizationManager.GetLocalizedString("PageViewDataSortDescription");
            if (! LocalizationManager.GetLocalizedString("PageViewDataSortCopyInstructions").Equals("PageViewDataSortCopyInstructions")){
                lblCopyDescription.Text = LocalizationManager.GetLocalizedString("PageViewDataSortCopyInstructions");
            }
            if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
            {
                var siteTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("SiteTitle");

                Master.HeadTitle = lblTableTitle.Text;
                Master.HeadTitle += ". " + siteTitle;
            }
            if (PXWeb.Settings.Current.Presentation.NewTitleLayout)
            {
                lblTableTitle.Text = "";
            }


        }

        /// <summary>
        /// Initializes CommandBar
        /// </summary>
        private void InitializeCommandBar()
        {
            Master.SetCommandBarFilter(PCAxisPlugins.CommandBarFilterFactory.GetFilter(PCAxisPlugins.CommandBarPluginFilterType.Sort.ToString()));
            Master.SetCommandBarPresentationView(Plugins.Views.TABLE_SORTED);
        }

        public string GetSymbolArray()
        {
            var symbolList = new List<string>();

            if (PaxiomManager.PaxiomModel != null)
            {

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbol1))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbol1);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.Symbol1);
                }

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbol2))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbol2);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.Symbol2);
                }

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbol3))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbol3);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.Symbol3);
                }

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbol4))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbol4);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.Symbol4);
                }

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbol5))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbol5);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.Symbol5);
                }

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbol6))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbol6);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.Symbol6);
                }


                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbolNIL))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbolNIL);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.DataSymbolNil);
                }

                if (!string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.DataSymbolSum))
                {
                    symbolList.Add(PaxiomManager.PaxiomModel.Meta.DataSymbolSum);
                }
                else
                {
                    symbolList.Add(PXWeb.Settings.Current.General.Global.DataSymbolSum);
                }
            }

            return "['" + string.Join("','", symbolList) + "']";
        }
    }
}
