<%@ control  inherits="PCAxis.Web.Controls.TableQueryCodebehind" %>

<asp:panel ID="ApiAccordionPanel" class="pxweb-accordion"  runat="server">
<button type="button" class="accordion-header closed" id="InformationBoxHeader" aria-expanded="False" onclick="accordionToggle(<%=ApiAccordionPanel.ClientID %>, this)" >
    <span class="header-text"><asp:Label ID="lblTableQueryInformation"  runat="server" Text=""></asp:Label></span>
</button>

<div class="accordion-body closed">
    <asp:Panel ID="pnlQueryInformation" CssClass="tablequery_informationpanel" runat="server">
        <asp:Label ID="lblInformationText" runat="server" Text="" CssClass="tablequery_informationtext"></asp:Label>
        <asp:Label ID="lblUrl" runat="server" Text="" CssClass="tablequery_urlcaption"></asp:Label>
        <asp:TextBox ID="txtUrl" runat="server" ReadOnly="true" CssClass="tablequery_url"></asp:TextBox>
        <asp:Label ID="lblQuery" runat="server" Text="" CssClass="tablequery_querycaption"></asp:Label>
        <asp:TextBox ID="txtQuery" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="20" CssClass="tablequery_query"></asp:TextBox>
        <asp:HyperLink ID="lnkMoreInfo" runat="server" CssClass="tablequery_moreinformation"></asp:HyperLink>    
        <asp:Button ID="btnSaveQuery" runat="server" CssClass="tablequery_savequery"></asp:Button>
    </asp:Panel>
</div>
</asp:panel>
