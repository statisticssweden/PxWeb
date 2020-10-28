<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VariableOverviewControl.ascx.cs" Inherits="PXWeb.UserControls.VariableOverviewControl" %>

<asp:Panel ID="VariableOverview" CssClass="variable_overview_container" runat="server" EnableViewState="True">
    <asp:Label ID="VariableOverviewHeading" CssClass="variable_overview_heading" runat="server"></asp:Label>
    <div class="variable_overview_title_list">
        <asp:Repeater ID="VariableOverviewTitleRepeater" OnItemDataBound="FillOverviewRepeater_ItemDataBound" runat="server">
            <ItemTemplate>
                <div class="variable_overview_column">
                    <asp:Label ID="VariableOverviewTitle" CssClass="variable_overview_title" runat="server"></asp:Label>
                    <div class="mandatory_container">
                        <asp:Label ID="VariableOverviewMandatoryText" CssClass="variable_overview_mandatory_text" runat="server"></asp:Label>
                        <asp:Label ID="VariableOverviewMandatoryStar" CssClass="variable_overview_mandatory_star" runat="server">*</asp:Label>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>