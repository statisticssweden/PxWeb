<%@ Page Language="C#" MasterPageFile="~/Presentation.master" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="PXWeb.Chart" Title="<%$ PxString: PxWebTitleChart %>" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="Head" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="Settings" ContentPlaceHolderID="cphSettings" runat="server">
    <asp:HyperLink ID="lnkShowChartSettings" runat="server" CssClass="chartsettings panelshowlink " data-showclass="chartsettings">
        <asp:Image ID="imgSettingsExpander" CssClass="px-settings-expandimage" runat="server" />
        <%= Master.GetLocalizedString("PxWebChartUserSettingsShow") %>
        <asp:Image ID="imgShowChartSettings" runat="server" ImageUrl="~/Resources/Images/settings-14.gif" CssClass="px-settings-imagelink" Visible="false" />
    </asp:HyperLink>
    <asp:Panel ID="pnlSettings" CssClass="px-settings settingpanel chartsettings" runat="server">
            <asp:Label ID="lblTitle" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsTitle %>" ></asp:Label>
            <asp:TextBox ID="txtTitle" CssClass="px_setting_textbox_title" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="validatorTitle" runat="server" 
                ControlToValidate="txtTitle" OnServerValidate="ValidateTitle"
                Text="*" ErrorMessage="*" CssClass="px_setting_validator" >
            </asp:CustomValidator>
            <br />
            <asp:Label ID="lblHeight" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsHeight %>"></asp:Label>
            <asp:TextBox ID="txtHeight" CssClass="px_setting_textbox_normal" runat="server" ></asp:TextBox>
            <asp:CustomValidator ID="validatorHeight" runat="server" 
                ControlToValidate="txtHeight" OnServerValidate="ValidateHeight"
                Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" >
            </asp:CustomValidator>
            <asp:Label ID="lblWidth" CssClass="px_setting_label_width" runat="server" Text="<%$ PxString: PxWebChartUserSettingsWidth %>"></asp:Label>
            <asp:TextBox ID="txtWidth" CssClass="px_setting_textbox_normal" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="validatorWidth" runat="server" 
                ControlToValidate="txtWidth" OnServerValidate="ValidateWidth"
                Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" >
            </asp:CustomValidator>
            <br />
            <asp:Panel ID="pnlLineThickness" runat="server">
                <asp:Label ID="lblLineThickness" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsLineThickness %>" ></asp:Label>
                <asp:TextBox ID="txtLineThickness" CssClass="px_setting_textbox_normal" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="validatorLineThickness" runat="server" 
                    ControlToValidate="txtLineThickness" OnServerValidate="ValidateLineThickness"
                    Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" >
                </asp:CustomValidator>
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlSortTime" runat="server">
                <asp:Label ID="lblSortTime" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsSortTime %>"></asp:Label>
                <asp:RadioButtonList ID="rblSortTime" CssClass="px_setting_radiobuttonlist" RepeatDirection="Horizontal" runat="server" >
                    <asp:ListItem Value="Ascending"></asp:ListItem>
                    <asp:ListItem Value="Descending"></asp:ListItem>
                    <asp:ListItem Value="None"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
            </asp:Panel>            
            <asp:Panel ID="pnlLabelOrientation" runat="server">
                <asp:Label ID="lblLabelOrientation" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsLabelOrientation %>"></asp:Label>
                <asp:RadioButtonList ID="rblLabelOrientation" CssClass="px_setting_radiobuttonlist" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Value="Horizontal"></asp:ListItem>
                    <asp:ListItem Value="Vertical"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlGuidelines" runat="server">
                <asp:Label ID="lblGuidelines" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsGuidelines %>" ></asp:Label>
                <asp:CheckBox ID="chkHorizontalGuidelines" CssClass="px_setting_checkbox" runat="server" Text="<%$ PxString: PxWebChartUserSettingsGuidelinesHorizontal %>"/>
                <asp:CheckBox ID="chkVerticalGuidelines" CssClass="px_setting_checkbox" runat="server" Text="<%$ PxString: PxWebChartUserSettingsGuidelinesVertical %>" />
                <br />
            </asp:Panel>            
            <asp:Label ID="lblLegend" CssClass="px_setting_label_normal" runat="server" Text="<%$ PxString: PxWebChartUserSettingsLegend %>"></asp:Label>
            <asp:CheckBox ID="chkShowLegend" CssClass="px_setting_checkbox" runat="server" AutoPostBack="true" 
                oncheckedchanged="ShowLegend_CheckedChanged" Text="<%$ PxString: PxWebChartUserSettingsLegendShow %>" />
            <asp:Label ID="lblLegendHeight" runat="server" Text="<%$ PxString: PxWebChartUserSettingsLegendHeight %>"></asp:Label>
            <asp:TextBox ID="txtLegendHeight" CssClass="px_setting_textbox_normal" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="validatorLegendHeight" runat="server" 
                ControlToValidate="txtLegendHeight" OnServerValidate="ValidateLegendHeight"
                Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" >
            </asp:CustomValidator>
            <asp:ValidationSummary ID="ValidationSummary" CssClass="px_setting_validation_summary" runat="server" />
            <div id="divSettingButtons" class="px_settings_buttons">
                <asp:HyperLink ID="lnkCancelSettings" runat="server" CssClass="px_settings_cancel">
                    <asp:Label ID="lblCancelSettings" runat="server" Text="<%$ PxString: PxWebChartUserSettingsCancel %>"></asp:Label>
                </asp:HyperLink> 
                <asp:Button ID="btnApply" Text="<%$ PxString: PxWebChartUserSettingsApply %>" CssClass="px_setting_apply" runat="server" onclick="ApplySettings_Click" />
            </div>
    </asp:Panel>

    <script type="text/javascript" >

        jQuery(document).ready(function () {
            jQuery('.px_settings_cancel').click(function () {
                //Hide any currently displayed setting panel
                jQuery('.settingpanel').hide(0);

                //Change expand image on all links
                var col = jQuery('.px-settings-collapseimage');
                col.removeClass('px-settings-collapseimage');
                col.addClass('px-settings-expandimage');

                // Collapse all setting panels
                jQuery('.panelshowlink').removeClass('settingpanelexpanded');
                return false;
            });
        });

    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel ID="pnlChart" runat="server" CssClass="">
        <asp:Image ID="chartImage" runat="server" CssClass="chart-image" />
    </asp:Panel>    
    <asp:Panel ID="pnlIllegalChart" CssClass="chart_information" runat="server" Visible="False">
        <p><asp:Label ID="lblIllegalChartTitle" CssClass="chart_information_title" runat="server" ></asp:Label></p>
        <p><asp:Label ID="lblIllegalChart" runat="server"></asp:Label></p>   
    </asp:Panel>
    <asp:Panel ID="pnlIllegalChart2" CssClass="chart_information" runat="server" Visible="False">
        <p><asp:Label ID="lblIllegalChartTitle2" CssClass="chart_information_title" runat="server"></asp:Label></p>
        <p><asp:Label ID="lblIllegalChart2" runat="server"></asp:Label></p>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Footer" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
