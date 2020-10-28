<%@ Page Title="<%$ PxString: PxWebTitleSelection %>" Language="C#" MasterPageFile="~/PxWeb.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Selection.aspx.cs" Inherits="PXWeb.Selection" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<%@ Register Src="~/UserControls/MetadataSystemControl.ascx" TagPrefix="ucMetadata" TagName="Metadata" %>
<%@ Register Src="~/UserControls/VariableOverviewControl.ascx" TagPrefix="ucVariableOverview" TagName="VariableOverview" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <meta name="Description" content="<%= TableTitle %>" />
    <meta property="og:title" content="<%= TableTitle  %>-<%= PXWeb.Settings.Current.General.Site.ApplicationName.ToString() %>" />
    <meta property="og:url" content="<%= PageUrl %>" />
    <meta property="og:type" content="article" />
    <meta property="og:site_name" content="<%= PXWeb.Settings.Current.General.Site.ApplicationName.ToString() %>" />
</asp:Content>
<asp:Content runat="server" ID="ContentTitle" ContentPlaceHolderID="TitlePlaceHolder">
    <asp:Label ID="MenuTitle" CssClass="hierarchical_tableinformation_title" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="switchLayoutContainer" class="switch-layout-container">
    <asp:Button runat="server" ID="SwitchLayout"  OnClick="SwitchLayout_Click"/>
    </div> 
    <pxc:TableInformation runat="server" Type="Normal" ID="TableInformationSelect" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" />
    <div id="InformationLinks" runat="server">
        <asp:HyperLink ID="lnkInformation" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="lnkFootnotes" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="lnkDetailedInformation" runat="server" Target="_blank"></asp:HyperLink>
        <asp:Literal ID="litDetailedInformation" runat="server" visible="false"></asp:Literal>
    </div>
    <div id="PageElements">
          
        <asp:panel class="pxweb-accordion" id="InformationBox" runat="server">
        <button type="button" class="accordion-header closed" id="InformationBoxHeader" onclick="accordionToggle()" >
            <span class="button-grid">
                <i id="accordion-expand"> <svg focusable="false" xmlns="http://www.w3.org/2000/svg"  width="20" height="20" viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="expand-icon"><polyline points="6 9 12 15 18 9"></polyline></svg></i>
                <i id="accordion-collapse" class="hidden" ><svg focusable="false" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="expand-icon"><polyline points="18 15 12 9 6 15"></polyline></svg></i>
                <span class="header-text"><asp:Label ID="lblInfo"  runat="server" Text=""></asp:Label></span>
            </span>
        </button>
        <div class="accordion-body closed" id="InformationBoxBody">
                <div id="divTableLinks" runat="server"> </div>
                <pxc:Information id="SelectionInformation" ContactForEveryContent="false" LastUpdatedForEveryContent="false" runat="server"></pxc:Information> 
                <dl class="information_definitionlist">
                    <dt><asp:Literal ID="litDetailedInformation2" runat="server" visible="true"></asp:Literal></dt>
                    <dd>
                        <asp:HyperLink ID="lnkDetailedInformation2" runat="server" visible="false" CssClass="information_detailedLink_value" Target="_blank"></asp:HyperLink>
                    </dd>
                </dl>
        </div>
    </asp:panel>

      

        <ucVariableOverview:VariableOverview runat="server" ID="ucVariableOverview" />
        <div id="VariableSelection">
            <pxc:VariableSelector ID="VariableSelector1" runat="server" EnableViewState="true" 
                ShowSelectedRowsColumns="true" 
                EliminationImagePath="mandatory.gif"
                JavascriptRowLimit="500"  />
        </div>
        <div id="divFootnotes" class="settingpanel footnotes" runat="server">
            <pxc:Footnote ID="SelectionFootnotes" runat="server" />
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
   
        function accordionToggle() {
            var accbody = document.getElementById("InformationBoxBody");
            accbody.classList.toggle("closed");
            var AccordionExpand = document.getElementById("accordion-expand");
            AccordionExpand.classList.toggle('hidden')
            var AccordionCollapse = document.getElementById("accordion-collapse");
            AccordionCollapse.classList.toggle('hidden')

        }
    </script>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>

