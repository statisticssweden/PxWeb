<%@ Page Language="C#" MasterPageFile="~/Presentation.master" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="PXWeb.Chart" Title="<%$ PxString: PxWebTitleChart %>" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="Head" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content runat="server" ID="ContentSettingsLabel" ContentPlaceHolderID="cphSettingsLabel">
    <button type="button" class="accordion-header closed" id="SettingsHeader" aria-expanded="false" onclick="accordionToggle(SettingsAccordionPanel, this)" >
          <span class="header-text"><asp:Label ID="SettingsLabel"  runat="server"></asp:Label></span>
    </button>
</asp:Content>
                  

<asp:Content ID="Settings" ContentPlaceHolderID="cphSettings" runat="server">
    <asp:Panel ID="pnlSettings" CssClass="px-settings settingpanel chartsettings flex-column" runat="server">
            <asp:Label ID="lblTitle" CssClass="font-heading" runat="server" AssociatedControlID="txtTitle" Text="<%$ PxString: PxWebChartUserSettingsTitle %>" ></asp:Label>
            <asp:TextBox ID="txtTitle" CssClass="px_setting_textbox_title" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="validatorTitle" runat="server"
                ControlToValidate="txtTitle" OnServerValidate="ValidateTitle"
                Text="*" ErrorMessage="*" CssClass="px_setting_validator" Display="Dynamic" >
            </asp:CustomValidator>
        <div id="sizeSettings" class="flex-row s-margin-top">
        <asp:Label ID="lblHeight" CssClass="font-heading" runat="server" AssociatedControlID="txtHeight" Text="<%$ PxString: PxWebChartUserSettingsHeight %>"></asp:Label>
            <asp:TextBox ID="txtHeight" CssClass="px_setting_textbox_normal" runat="server" TextMode="Number"></asp:TextBox>
            <asp:CustomValidator ID="validatorHeight" runat="server" 
                ControlToValidate="txtHeight" OnServerValidate="ValidateHeight"
                Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" Display="Dynamic" >
            </asp:CustomValidator>
            <asp:Label ID="lblWidth" CssClass="font-heading xs-margin-left" runat="server" AssociatedControlID="txtWidth" Text="<%$ PxString: PxWebChartUserSettingsWidth %>"></asp:Label>
            <asp:TextBox ID="txtWidth" CssClass="px_setting_textbox_normal" runat="server" TextMode="Number"></asp:TextBox>
            <asp:CustomValidator ID="validatorWidth" runat="server" 
                ControlToValidate="txtWidth" OnServerValidate="ValidateWidth"
                Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" Display="Dynamic" >
            </asp:CustomValidator>
        </div>
        <asp:Panel ID="pnlLineThickness" runat="server">
                <asp:Label ID="lblLineThickness" CssClass="font-heading" runat="server" AssociatedControlID="txtLineThickness" Text="<%$ PxString: PxWebChartUserSettingsLineThickness %>" ></asp:Label>
                <asp:TextBox ID="txtLineThickness" CssClass="px_setting_textbox_normal" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="validatorLineThickness" runat="server" 
                    ControlToValidate="txtLineThickness" OnServerValidate="ValidateLineThickness"
                    Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" Display="Dynamic" >
                </asp:CustomValidator>
        </asp:Panel>
        <asp:Panel ID="pnlSortTime" CssClass="s-margin-top" runat="server">
                <asp:RadioButtonList ID="rblSortTime" CssClass="px_setting_radiobuttonlist" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server" >
                    <asp:ListItem Value="Ascending"></asp:ListItem>
                    <asp:ListItem Value="Descending"></asp:ListItem>
                    <asp:ListItem Value="None"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>            
            <asp:Panel ID="pnlLabelOrientation" CssClass="s-margin-top" runat="server">
                <asp:RadioButtonList ID="rblLabelOrientation" CssClass="px_setting_radiobuttonlist" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Value="Horizontal"></asp:ListItem>
                    <asp:ListItem Value="Vertical"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
        <asp:Panel ID="pnlGuidelines" CssClass="s-margin-top checkbox-list" runat="server">
                <asp:Label ID="lblGuidelines" CssClass="font-heading" runat="server" Text="<%$ PxString: PxWebChartUserSettingsGuidelines %>" ></asp:Label>
                <asp:CheckBox ID="chkHorizontalGuidelines" CssClass="px_setting_checkbox" runat="server" Text="<%$ PxString: PxWebChartUserSettingsGuidelinesHorizontal %>"/>
                <asp:CheckBox ID="chkVerticalGuidelines" CssClass="px_setting_checkbox" runat="server" Text="<%$ PxString: PxWebChartUserSettingsGuidelinesVertical %>" />
            </asp:Panel>            
            <div id="legendSettings" class="flex-row s-margin-top">
                <asp:Label ID="lblLegend" CssClass="font-heading" runat="server" Text="<%$ PxString: PxWebChartUserSettingsLegend %>"></asp:Label>
                <asp:CheckBox ID="chkShowLegend" CssClass="px_setting_checkbox" runat="server" AutoPostBack="true" 
                    oncheckedchanged="ShowLegend_CheckedChanged" Text="<%$ PxString: PxWebChartUserSettingsLegendShow %>" />
                <asp:Label ID="lblLegendHeight" runat="server" AssociatedControlID="txtLegendHeight" Text="<%$ PxString: PxWebChartUserSettingsLegendHeight %>"></asp:Label>
                <asp:TextBox ID="txtLegendHeight" CssClass="px_setting_textbox_normal" runat="server" TextMode="Number"></asp:TextBox>
                <asp:CustomValidator ID="validatorLegendHeight" runat="server" 
                    ControlToValidate="txtLegendHeight" OnServerValidate="ValidateLegendHeight"
                    Text="*" ErrorMessage="*" ValidateEmptyText="True" CssClass="px_setting_validator" Display="Dynamic" >
                </asp:CustomValidator>
            </div>
            <div role="region" id="validationsummarychartnotifyscreenreader" aria-live="assertive" aria-atomic="true">
                <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" role="alert" CssClass="px_setting_validation_summary variableselector_error_summary flex-column" runat="server" />
             </div>
            <div id="divSettingButtons" class="container_exit_buttons_row">
                <asp:Button ID="btnCancelChartSettings" runat="server" CssClass="pxweb-btn" Text="<%$ PxString: PxWebChartUserSettingsCancel %>" OnClientClick="closeAccordion('SettingsHeader', 'SettingsBody'); return false;" />
                <asp:Button ID="btnApply" Text="<%$ PxString: PxWebChartUserSettingsApply %>" CssClass="pxweb-btn primary-btn no-margin-right" runat="server" onclick="ApplySettings_Click" />
            </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel ID="pnlChart" runat="server" CssClass="">
        <asp:Image ID="chartImage" runat="server" CssClass="chart-image" />
    </asp:Panel>    
    <asp:Panel ID="pnlIllegalChart" CssClass="flex-row px-messages align-center" runat="server" Visible="False">
        <div class="Information-warning-box-icon information-warning-box">
        </div>
        <div class="flex-column justify-center">
            <asp:Label ID="lblIllegalChartTitle" CssClass="px-message-text heading" runat="server" ></asp:Label>
            <asp:Label ID="lblIllegalChart" CssClass="px-message-text"  runat="server"></asp:Label> 
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlIllegalChart2" CssClass="flex-row px-messages align-center" runat="server" Visible="False">
        <div class="Information-warning-box-icon information-warning-box">
        </div>
        <div class="flex-column justify-center">
            <asp:Label ID="lblIllegalChartTitle2" CssClass="px-message-text heading" runat="server"></asp:Label>
            <asp:Label ID="lblIllegalChart2" CssClass="px-message-text" runat="server"></asp:Label>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Footer" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
