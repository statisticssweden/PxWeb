using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Web.Core.Management;
using PCAxis.Paxiom;

namespace PXWeb.UserControls
{
    public partial class MetadataSystemControl : System.Web.UI.UserControl
    {
        private PCAxis.Metadata.IMetaIdProvider _linkManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel == null)
                {
                    return;
                }

                PXModel model = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel;                
                ListItem li;

                cboVariables.Items.Clear();
                li = new ListItem(LocalizationManager.GetLocalizedString("PxWebMetadataSelectVariable"));
                li.Selected = true;
                cboVariables.Items.Add(li);

                foreach (Variable variable in model.Meta.Variables)
                {
                    li = new ListItem(variable.Name, variable.Code);
                    cboVariables.Items.Add(li);
                }
            }

            lblVariableHeader.Text = LocalizationManager.GetLocalizedString("PxWebMetadataVariableHeader");
            lblValuesHeader.Text = LocalizationManager.GetLocalizedString("PxWebMetadataValuesHeader");

            PxUrl url = new PxUrl(null);
            _linkManager = PXWeb.Settings.Current.Database[url.Database].Metadata.MetaLinkMethod;
        }

        /// <summary>
        /// Explicitly select a variable and display its metadata
        /// </summary>
        /// <param name="variable"></param>
        public void SelectVariable(PCAxis.Paxiom.Variable variable)
        {
            if (cboVariables.Items.FindByValue(variable.Code) != null)
            {
                cboVariables.SelectedValue = variable.Code;
                DisplayVariable();
            }
            else
            {
                cboVariables.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Display metadata links for the selected variable
        /// </summary>
        private void DisplayVariable()
        {
            UpdateVisibility();

            if (cboVariables.SelectedIndex > 0)
            {
                List<MetaItem> lstVariableLinks = GetVariableLinks();

                if (lstVariableLinks.Count > 0)
                {
                    repVariable.DataSource = lstVariableLinks;
                    repVariable.DataBind();
                    repVariable.Visible = true;
                    lblNoMetaVariable.Visible = false;
                }
                else
                {
                    repVariable.Visible = false;
                    lblNoMetaVariable.Text = LocalizationManager.GetLocalizedString("PxWebMetadataNoVariableMeta");
                    lblNoMetaVariable.Visible = true;
                }

                List<MetaItem> lstValueLinks = GetValueLinks();

                if (lstValueLinks.Count > 0)
                {
                    repValue.DataSource = lstValueLinks;
                    repValue.DataBind();
                    repValue.Visible = true;
                    lblNoMetaValues.Visible = false;
                }
                else
                {
                    repValue.Visible = false;
                    lblNoMetaValues.Text = LocalizationManager.GetLocalizedString("PxWebMetadataNoValueMeta");
                    lblNoMetaValues.Visible = true;
                }
            }
        }

        private List<MetaItem> GetVariableLinks()
        {
            List<MetaItem> lst = new List<MetaItem>();

            PCAxis.Paxiom.Variable variable = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.GetByCode(cboVariables.SelectedValue);

            MetaItem itm = new MetaItem();
            itm.Name = variable.Name;

            if (!string.IsNullOrWhiteSpace(variable.MetaId))
            {
                itm.Links = _linkManager.GetVariableLinks(variable.MetaId, LocalizationManager.CurrentCulture.Name).ToList();
            }

            // Only display the variable if it has metadata links
            if (itm.Links.Count > 0)
            {
                lst.Add(itm);
            }

            return lst;
        }


        private List<MetaItem> GetValueLinks()
        {
            List<MetaItem> links = new List<MetaItem>();

            PCAxis.Paxiom.Variable variable = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.GetByCode(cboVariables.SelectedValue);

            foreach (PCAxis.Paxiom.Value value in variable.Values)
            {
                if (!string.IsNullOrWhiteSpace(value.MetaId))
                {
                    MetaItem itm = new MetaItem();
                    itm.Name = value.Text;
                    itm.Links = _linkManager.GetValueLinks(value.MetaId, LocalizationManager.CurrentCulture.Name).ToList();

                    // Only display value if it has metadata links
                    if (itm.Links.Count > 0)
                    {
                        links.Add(itm);
                    }
                }
            }

            return links;
        }



        /// <summary>
        /// Update visibility of the metadata panel
        /// </summary>
        private void UpdateVisibility()
        {
            if (cboVariables.SelectedIndex > 0)
            {
                pnlVariable.Visible = true;
            }
            else
            {
                pnlVariable.Visible = false;
            }
        }

        protected void cboVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayVariable();
        }


        /// <summary>
        /// Class for holding metadatalink-information for one variable or value
        /// </summary>
        private class MetaItem
        {
            /// <summary>
            /// Variable/Value name
            /// </summary>
            public string Name;
            /// <summary>
            /// List of links
            /// </summary>
            public List<PCAxis.Metadata.MetaLink> Links;

            /// <summary>
            /// Constructor
            /// </summary>
            public MetaItem()
            {
                Links = new List<PCAxis.Metadata.MetaLink>();
            }
        }

        protected void repVariable_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem itm = e.Item;
            MetaItem currentItem = e.Item.DataItem as MetaItem;

            Label lbl = (Label)itm.FindControl("lblVariableName");
            lbl.Text = currentItem.Name;

            Repeater rep = (Repeater)itm.FindControl("repVariableLinks");
            rep.DataSource = currentItem.Links;
            rep.DataBind();
        }

        protected void repVariableLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CreateLink(e, "divVarLink");
        }

        protected void repValue_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem itm = e.Item;
            MetaItem currentItem = e.Item.DataItem as MetaItem;

            Label lbl = (Label)itm.FindControl("lblValueName");
            lbl.Text = currentItem.Name;

            Repeater rep = (Repeater)itm.FindControl("repValueLinks");
            rep.DataSource = currentItem.Links;
            rep.DataBind();
        }

        protected void repValueLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CreateLink(e, "divValueLink");
        }

        private void CreateLink(RepeaterItemEventArgs e, string placeholder)
        {
            RepeaterItem itm = e.Item;
            PCAxis.Metadata.MetaLink currentItem = e.Item.DataItem as PCAxis.Metadata.MetaLink;

            System.Web.UI.HtmlControls.HtmlGenericControl ph = (System.Web.UI.HtmlControls.HtmlGenericControl)itm.FindControl(placeholder);

            if (currentItem != null)
            {
                HyperLink lnk = new HyperLink();
                lnk.Text = currentItem.LinkText;
                lnk.NavigateUrl = currentItem.Link;
                lnk.Target = currentItem.Target;
                ph.Controls.Add(lnk);
            }
        }
    }
}
