<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccordianAboutTableControl.ascx.cs" Inherits="PXWeb.UserControls.AccordianAboutTableControl" %>

<asp:panel class="pxweb-accordion about-table s-margin-top" id="InformationBox" runat="server">
    <button type="button" class="accordion-header closed" id="InformationBoxHeader" aria-expanded="false" onclick="accordionToggle(<%=InformationBox.ClientID %>, this)" >
        <span class="header-text"><asp:Label ID="lblInfo"  runat="server" Text=""></asp:Label></span>
    </button>
    <div class="accordion-body closed">
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
