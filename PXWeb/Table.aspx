<%@ Page Title="<%$ PxString: PxWebTitleTable %>" Language="C#" MasterPageFile="~/Presentation.master" AutoEventWireup="true" CodeBehind="Table.aspx.cs" Inherits="PXWeb.Table" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="Head" ContentPlaceHolderID="cphHead" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Resources/Styles/print.css") %>' type="text/css" media="print" />
</asp:Content>

<asp:Content ID="Settings" ContentPlaceHolderID="cphSettings" runat="server">
    <asp:HyperLink ID="lnkShowTblSettings" runat="server" CssClass="tablesettings panelshowlink " data-showclass="tablesettings">
        <asp:Image ID="imgSettingsExpander" CssClass="px-settings-expandimage" runat="server" />
        <%= Master.GetLocalizedString("PxWebTableUserSettingsShow") %>
        <asp:Image ID="imgShowTblSettings" runat="server" ImageUrl="~/Resources/Images/settings-14.gif" CssClass="px-settings-imagelink" Visible="false" />
    </asp:HyperLink>

    <asp:Panel ID="pnlSettings" CssClass="px-settings settingpanel tablesettings" runat="server">
        <asp:Label ID="lblZeroOption" CssClass="px_setting_heading" runat="server" Text="<%$ PxString: PxWebTableUserSettingsZeroOption %>"></asp:Label>   
            <asp:RadioButtonList ID="rblZeroOption" CssClass="px_setting_radiobuttonlist" RepeatDirection="Vertical" runat="server" >
            <asp:ListItem Value="ShowAll" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionShowAll %>"></asp:ListItem>
            <asp:ListItem Value="NoZero" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoZero %>"></asp:ListItem>
            <asp:ListItem Value="NoZeroAndNil" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoZeroAndNil %>"></asp:ListItem>
            <asp:ListItem Value="NoSymbols" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoSymbols %>"></asp:ListItem>
            <asp:ListItem Value="NoZeroNilAndSymbol" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoZeroNilAndSymbol %>"></asp:ListItem>
        </asp:RadioButtonList>
        <div id="divSettingButtons" class="px_settings_buttons">
            <asp:HyperLink ID="lnkCancelSettings" runat="server" CssClass="px_settings_cancel">
                <asp:Label ID="lblCancelSettings" runat="server" Text="<%$ PxString: PxWebTableUserSettingsCancel %>"></asp:Label>
            </asp:HyperLink> 
            <asp:Button ID="btnApply" Text="<%$ PxString: PxWebTableUserSettingsApply %>" CssClass="px_setting_apply" runat="server" onclick="ApplySettings_Click" />
        </div>
    </asp:Panel>

    <script type="text/javascript" >

        jQuery(document).ready(function () {
            jQuery('.px_settings_cancel').click(function () {
                // Reset selected value in radiobuttonlist
                jQuery('#<%=rblZeroOption.ClientID %>').find("input[value='ShowAll']").prop('checked', true);

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

<asp:Content ID="Messages" ContentPlaceHolderID="cphMessages" runat="server">
    <asp:Image ID="imgTableCropped" CssClass="alertimage" runat="server" />
    <asp:Label ID="lblTableCropped" runat="server"></asp:Label>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="cphMain" runat="server">
    <pxc:tableinformation ID="TableInformationView" runat="server" Type="TableView" />
    <pxc:table id="Table1" runat="server" DisplayCellInformationWithoutJavascript="false" >    
    </pxc:table>       
    <div id="dialogModal" runat="server" visible="false" enableviewstate="false">
        <pxc:TableInformation runat="server" ID="TableInfo" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="false" Visible="true" />
        <pxc:Footnote ID="Footnote1" runat="server" EnableViewState="false" />
    </div>
</asp:Content>

<asp:Content ID="Footer" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>

   