<%@ Page Title="<%$ PxString: PxWebTitleSelection %>" Language="C#" MasterPageFile="~/PxWeb.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Selection.aspx.cs" Inherits="PXWeb.Selection" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<%@ Register Src="~/UserControls/MetadataSystemControl.ascx" TagPrefix="ucMetadata" TagName="Metadata" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <meta name="Description" content="<%= TableTitle %>" />
    <meta property="og:title" content="<%= TableTitle  %>-<%= PXWeb.Settings.Current.General.Site.ApplicationName.ToString() %>" />
    <meta property="og:url" content="<%= PageUrl %>" />
    <meta property="og:type" content="article" />
    <meta property="og:site_name" content="<%= PXWeb.Settings.Current.General.Site.ApplicationName.ToString() %>" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <pxc:TableInformation runat="server" Type="Normal" ID="TableInformationSelect" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" />
    <asp:Label ID="MenuTitle" CssClass="hierarchical_tableinformation_title" runat="server" Text=""></asp:Label>
    <div id="InformationLinks" runat="server">
        <asp:HyperLink ID="lnkInformation" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="lnkFootnotes" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="lnkDetailedInformation" runat="server" Target="_blank"></asp:HyperLink>
        <asp:Literal ID="litDetailedInformation" runat="server" visible="false"></asp:Literal>
    </div>

    <div id="PageElements">
        <asp:Panel ID="PanelTabs" runat="server" >
        <ul  >    
            <li><a href="#VariableSelection"><%= Master.GetLocalizedString("PxWebSelectVariable") %></a></li>    
            <li><a href="#AboutTable"><%= Master.GetLocalizedString("PxWebAboutTable") %></a></li>   
<%--            <li id="MetadataTab" runat="server"><a href="#Metadata"><%= Master.GetLocalizedString("Metadata") %></a></li>--%>
        </ul>
        </asp:Panel>
        <div id="VariableSelection">
            <pxc:VariableSelector ID="VariableSelector1" runat="server" EnableViewState="true" 
                ViewPageUrl="Table.aspx" 
                ShowSelectedRowsColumns="true" 
                EliminationImagePath="mandatory.gif"
                JavascriptRowLimit="500"/>   
        </div>
        <div id="AboutTable">
            <div id="divTableLinks" runat="server" class="meta_tablelinks"></div> 
            <asp:HyperLink ID="lnkShowInformation" runat="server"  CssClass="information panelshowlink" data-showclass="information">
                <asp:Image ID="imgShowInformationExpander" CssClass="px-settings-expandimage" runat="server" />
                <%= Master.GetLocalizedString("PxWebAboutTableContact") %>
                <asp:Image ID="imgShowInformation" ImageUrl="~/Resources/Images/download-16.gif" CssClass="px-settings-imagelink" runat="server" Visible="false" />
            </asp:HyperLink>
            <div id="divInformation" class="settingpanel information" runat="server">
                <pxc:information id="SelectionInformation" ContactForEveryContent="false" LastUpdatedForEveryContent="false" runat="server"></pxc:information> 
                    <dl class="information_definitionlist">
                        <dt><asp:Literal ID="litDetailedInformation2" runat="server" visible="false" ></asp:Literal></dt>
                        <dd>
                            <asp:HyperLink ID="lnkDetailedInformation2" runat="server" CssClass="information_detailedLink_value" Target="_blank"></asp:HyperLink>
                        </dd>
                    </dl>
            </div>

            <asp:HyperLink ID="lnkShowFootnotes" runat="server"  CssClass="footnotes panelshowlink" data-showclass="footnotes">
                <asp:Image ID="imgShowFootnotesExpander" CssClass="px-settings-expandimage" runat="server" />
                <%= Master.GetLocalizedString("PxWebAboutTableFootnotes") %>
                <asp:Image ID="imgShowFootnotes" ImageUrl="~/Resources/Images/download-16.gif" CssClass="px-settings-imagelink" runat="server" Visible="false" />
            </asp:HyperLink>
            <div id="divFootnotes" class="settingpanel footnotes" runat="server">
                <pxc:Footnote ID="SelectionFootnotes" runat="server" />
            </div>

            <asp:HyperLink ID="lnkMetadata" runat="server"  CssClass="metadata panelshowlink" data-showclass="metadata">
                <asp:Image ID="imgShowMetadataExpander" CssClass="px-settings-expandimage" runat="server" />
                <%= Master.GetLocalizedString("PxWebAboutTableDefinitions") %>
                <asp:Image ID="imgShowMetadata" ImageUrl="~/Resources/Images/download-16.gif" CssClass="px-settings-imagelink" runat="server" Visible="false" />
            </asp:HyperLink>
            <div id="divMetadata" class="settingpanel metadata" runat="server">
                <ucMetadata:Metadata runat="server" id="ucMetadataSystem" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="PageElementsSelectedTab" Value="0" runat="server" />
    <asp:HiddenField ID="AboutTableSelectedAccordion" Value="" runat="server" />
    <script type="text/javascript">
        jQuery(function () {
            var selectedTabId = jQuery("[id$=PageElementsSelectedTab]").val();
            selectedTabId = selectedTabId === null ? 0 : selectedTabId; //your default being 0
            jQuery("#PageElements").tabs({
                active: selectedTabId,
                activate: function (event, ui) {
                    selectedTabId = jQuery("#PageElements").tabs("option", "active");
                    jQuery("[id$=PageElementsSelectedTab]").val(selectedTabId); // set new value
                }
            });
        });

        jQuery(document).ready(function () {

            //Hide any currently displayed setting panel
            settingpanelCollapseAll();
            // Check if any panel on the About table tab shall be displayed
            var AboutTableSelectedClass = jQuery("[id$=AboutTableSelectedAccordion]").val();

            if (AboutTableSelectedClass != '') {
                // Display panel
                settingpanelExpand(AboutTableSelectedClass);
            }

            jQuery('.panelshowlink').click(function () {
                //Hide any currently displayed setting panel
                settingpanelCollapseAll();

                if (!settingpanelIsExpanded(this)) {
                    //Get my currently clicked panel
                    var showclass = jQuery(this).data('showclass');
                    //Show my setting panel
                    settingpanelExpand(showclass);
                    //Keep this accordion open
                    jQuery("[id$=AboutTableSelectedAccordion]").val(showclass);
                }
                else {
                    //Remove expanded class from this panellink
                    settingpanelCollapse(this);
                }

                return false;
            });
        });

        jQuery(function () {
            jQuery(window).bind('beforeunload', function (e) {
                for (let i = 0; i < sessionStorage.length; i++) {
                    let key = sessionStorage.key(i);
                    if (key.indexOf("ValuesListBox") != -1) {
                        sessionStorage.removeItem(key);
                        i--;
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>

