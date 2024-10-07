<%@ Page Title="<%$ PxString: PxWebTitleSelection %>" Language="C#" MasterPageFile="~/PxWeb.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Selection.aspx.cs" Inherits="PXWeb.Selection" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<%@ Register Src="~/UserControls/MetadataSystemControl.ascx" TagPrefix="ucMetadata" TagName="Metadata" %>
<%@ Register Src="~/UserControls/VariableOverviewControl.ascx" TagPrefix="ucVariableOverview" TagName="VariableOverview" %>
<%@ Register Src="~/UserControls/AccordianAboutTableControl.ascx" TagPrefix="ucAccordianAboutTable" TagName="AccordianAboutTable" %>
<%@ Register TagPrefix="pxwebCustomControl" Namespace="PXWeb.CustomControls" Assembly="PXWeb" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <meta name="Description" content="<%= TableTitle %>" />
    <meta property="og:title" content="<%= TableTitle  %>-<%= PXWeb.Settings.Current.General.Site.ApplicationName.ToString() %>" />
    <meta property="og:url" content="<%= PageUrl %>" />
    <meta property="og:type" content="article" />
    <meta property="og:site_name" content="<%= PXWeb.Settings.Current.General.Site.ApplicationName.ToString() %>" />
</asp:Content>
<asp:Content runat="server" ID="ContentTitle" ContentPlaceHolderID="TitlePlaceHolder">
    <pxwebCustomControl:HeadingLabel ID="MenuTitle" runat="server" Text=""></pxwebCustomControl:HeadingLabel>
    <pxc:TableInformation runat="server" Type="Normal" ID="TableInformationSelect" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="SelectionPage">
        
        <div id="InformationLinks" runat="server">
            <asp:HyperLink ID="lnkInformation" runat="server"></asp:HyperLink>
            <asp:HyperLink ID="lnkFootnotes" runat="server"></asp:HyperLink>
            <asp:HyperLink ID="lnkDetailedInformation" runat="server" Target="_blank"></asp:HyperLink>
            <asp:Literal ID="litDetailedInformation" runat="server" visible="false"></asp:Literal>
        </div>

        <% if (!string.IsNullOrWhiteSpace(Master.OfficialStatisticsImage)){%>
            <img src=<%= Master.OfficialStatisticsImage%> class="officialStatisticsImage" alt="<%= Master.GetLocalizedString("PxWebOfficialStatisticsLogo") %>"/>
        <%} %>
        <div id="linkBulkDiv" runat="server" visible="false">
        <%if (PXWeb.Settings.Current.Features.General.BulkLinkEnabled)
            {%>                   
                <asp:HyperLink ID="linkBulkLink" class="pxweb-link" runat="server"></asp:HyperLink>  
            <br />
            <br />  
           <%}%>
        </div> 
        <div id="PageElements">
            <div id="subheader">
                <pxwebCustomControl:HeadingLabel id="lblSubHeader" runat="server" Text="<%$ PxString: PxWebSubHeaderChooseVariables%>"></pxwebCustomControl:HeadingLabel>
            </div>
            <div class="flex-row justify-space-between">
                <ucAccordianAboutTable:AccordianAboutTable runat="server" ID="UcAccordianAboutTable" />
                <div id="switchLayoutContainer" class="switch-layout-container m-margin-left">
                    <asp:Button runat="server" ID="SwitchLayout"  OnClick="SwitchLayout_Click" Text="Bytt visningsformat"/>
                </div>
            </div>
            
            <%--<ucVariableOverview:VariableOverview runat="server" ID="ucVariableOverview" />--%>
            <div id="VariableSelection">
                <pxc:VariableSelector ID="VariableSelector1" runat="server" EnableViewState="true" 
                    ShowSelectedRowsColumns="true" 
                    EliminationImagePath="mandatory.gif"
                    JavascriptRowLimit="500"  />
            </div>
            <div id="SearchResults" role="status" class="screenreader-only"></div>
            <div id="divFootnotes" class="settingpanel footnotes" runat="server">
                <pxc:Footnote ID="SelectionFootnotes" InAccordionStyle="true" runat="server" />
            </div>
        </div>
        <asp:HiddenField ID="AboutTableSelectedAccordion" Value="" runat="server" />
        <script>
                jQuery(document).ready(function () {

            ////Hide any currently displayed setting panel
            //settingpanelCollapseAll();
            //// Check if any panel on the About table tab shall be displayed
            //var AboutTableSelectedClass = jQuery("[id$=AboutTableSelectedAccordion]").val();

            //if (AboutTableSelectedClass != '') {
            //    // Display panel
            //    settingpanelExpand(AboutTableSelectedClass);
            //}

            //jQuery('.panelshowlink').click(function () {
            //    //Hide any currently displayed setting panel
            //    settingpanelCollapseAll();

            //    if (!settingpanelIsExpanded(this)) {
            //        //Get my currently clicked panel
            //        var showclass = jQuery(this).data('showclass');
            //        //Show my setting panel
            //        settingpanelExpand(showclass);
            //        //Keep this accordion open
            //        jQuery("[id$=AboutTableSelectedAccordion]").val(showclass);
            //    }
            //    else {
            //        //Remove expanded class from this panellink
            //        settingpanelCollapse(this);
            //    }

            //    return false;
            //});

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
    </div>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>

