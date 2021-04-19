<%@ Page Title="<%$ PxString: PxWebTitleTable %>" Language="C#" MasterPageFile="~/Presentation.master" AutoEventWireup="true" CodeBehind="Table.aspx.cs" Inherits="PXWeb.Table" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="Head" ContentPlaceHolderID="cphHead" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Resources/Styles/print.css") %>' type="text/css" media="print" />
</asp:Content>

<asp:Content runat="server" ID="ContentSettingsLabel" ContentPlaceHolderID="cphSettingsLabel">
    <button type="button" class="accordion-header closed" id="SettingsHeader" onclick="accordionToggle(SettingsAccordionPanel, this)" >
        <span class="header-text"><asp:Label ID="SettingsLabel"  runat="server"></asp:Label></span>
    </button>
</asp:Content>                       
                   

<asp:Content ID="Settings" ContentPlaceHolderID="cphSettings" runat="server">
    <asp:Panel ID="pnlSettings" CssClass="px-settings settingpanel tablesettings" runat="server">
        <asp:Panel ID="pnlForRblZeroOption" runat="server">
            <asp:RadioButtonList ID="rblZeroOption" CssClass="px_setting_radiobuttonlist" RepeatLayout="Flow" runat="server" >
                <asp:ListItem Value="ShowAll" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionShowAll %>"></asp:ListItem>
                <asp:ListItem Value="NoZero" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoZero %>"></asp:ListItem>
                <asp:ListItem Value="NoZeroAndNil" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoZeroAndNil %>"></asp:ListItem>
                <asp:ListItem Value="NoSymbols" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoSymbols %>"></asp:ListItem>
                <asp:ListItem Value="NoZeroNilAndSymbol" Text="<%$ PxString: PxWebTableUserSettingsZeroOptionNoZeroNilAndSymbol %>"></asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
        <div id="divSettingButtons" class="container_exit_buttons_row">
            <asp:Button ID="btnCancelTableSettings" runat="server" CssClass="pxweb-btn" Text="<%$ PxString: PxWebTableUserSettingsCancel %>" OnClientClick="cancelTableSettings(); return false;" />
            <asp:Button ID="btnApply" Text="<%$ PxString: PxWebTableUserSettingsApply %>" CssClass="pxweb-btn primary-btn no-margin-right" runat="server" onclick="ApplySettings_Click" />
        </div>
    </asp:Panel>
    
    <script>
        function cancelTableSettings() {
            jQuery('#<%=rblZeroOption.ClientID %>').find("input[value='ShowAll']").prop('checked', true);
            closeAccordion('SettingsHeader', 'SettingsBody');
        }
    </script>
</asp:Content>

<asp:Content ID="Messages" ContentPlaceHolderID="cphMessages" runat="server">
    <asp:Panel runat="server" ID="tableMessagePanel" Visible="False" CssClass="px-messages flex-row">
        <div class="information-warning-box cropped-table">
        </div>
        <div class="flex-column justify-center">
            <asp:Label  class="px-message-text heading"  ID="lblTableCroppedHeading" runat="server"></asp:Label>
            <asp:Label  class="px-message-text" ID="lblTableCropped" runat="server"></asp:Label>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="cphMain" runat="server">
    <% if (PXWeb.Settings.Current.Presentation.NewTitleLayout)
        { %>
    <div class="m-margin-top"></div>
    <% } else { %>
    <pxc:tableinformation ID="TableInformationView" runat="server" Type="TableView" />
    <% } %>
    <pxc:table id="Table1" runat="server" DisplayCellInformationWithoutJavascript="false" >    
    </pxc:table>       
    <div id="dialogModal" runat="server" visible="false" enableviewstate="false">
        <pxc:TableInformation runat="server" ID="TableInfo" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="false" Visible="true" />
        <pxc:Footnote ID="Footnote1" runat="server" EnableViewState="false" />
    </div>
    <script>
        jQuery(document).ready(function() {
            jQuery("table").delegate('td.table-data-filled',
                'mouseover mouseleave',
                function(e) {
                    if (e.type == 'mouseover') {
                        jQuery(this).addClass("table-hover");
                        jQuery(this).siblings("td").addClass("table-hover");
                        jQuery(this).parent().find("th").last().addClass("table-hover");
                    } else {
                        jQuery(this).removeClass("table-hover");
                        jQuery(this).siblings("td").removeClass("table-hover");
                        jQuery(this).parent().find("th").last().removeClass("table-hover");
                    }
                });

            jQuery("table").delegate('th',
                'mouseover mouseleave',
                function (e) {
                    if (e.type == 'mouseover') {
                        jQuery(this).siblings("td").addClass("table-hover");
                        jQuery(this).parent().find("th").not(".table-header-first, .table-header-middle, .table-header-last").last().addClass("table-hover");
                    } else {
                        jQuery(this).siblings("td").removeClass("table-hover");
                        jQuery(this).parent().find("th").not(".table-header-first, .table-header-middle, .table-header-last").last().removeClass("table-hover");
                    }
                });
        });
    </script>

</asp:Content>

<asp:Content ID="Footer" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
