using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Paxiom;
using PCAxis.Web.Core.Management;

namespace PXWeb.UserControls
{
    public partial class VariableOverviewControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VariableOverviewHeading.Text = LocalizationManager.GetLocalizedString("CtrlVariableSelectorVariableOverviewHeading");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillOverviewRepeater();
        }

        private void FillOverviewRepeater()
        {
            IOrderedEnumerable<Variable> query;

            if (HaveSetCandidateMustSelect())
            {
                query = (from v in PaxiomManager.PaxiomModel.Meta.Variables
                    orderby v.IsContentVariable descending, v.IsTime descending, v.ExtendedProperties["CandidateMustSelect"] descending
                    select v);
            }
            else
            {
                query = (from v in PaxiomManager.PaxiomModel.Meta.Variables
                    orderby v.IsContentVariable descending
                    select v);
            }

            VariableOverviewTitleRepeater.DataSource = query;
            VariableOverviewTitleRepeater.DataBind();
        }

        private static bool HaveSetCandidateMustSelect()
        {
            var haveSetCandidateMustSelect = true;
            foreach (var variable in PaxiomManager.PaxiomModel.Meta.Variables)
            {
                if (!variable.ExtendedProperties.ContainsKey("CandidateMustSelect"))
                    haveSetCandidateMustSelect = false;
            }

            return haveSetCandidateMustSelect;
        }

        protected void FillOverviewRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var variable = e.Item.DataItem as Variable;
                var label = e.Item.FindControl("VariableOverviewTitle") as Label;
                label.Text = variable.Name;

                if (!variable.Elimination)
                {
                    ((Label)e.Item.FindControl("VariableOverviewMandatoryText")).Visible = true;
                    ((Label)e.Item.FindControl("VariableOverviewMandatoryStar")).Visible = true;
                    ((Label)e.Item.FindControl("VariableOverviewMandatoryText")).Text = LocalizationManager.GetLocalizedString("CtrlVariableSelectorMandatoryText");
                }
                else
                {
                    ((Label)e.Item.FindControl("VariableOverviewMandatoryText")).Visible = false;
                    ((Label)e.Item.FindControl("VariableOverviewMandatoryStar")).Visible = false;
                }
            }
        }
    }
}