<%@ control  inherits="PCAxis.Web.Controls.TableQueryCodebehind" %>

<asp:panel ID="ApiAccordionPanel" class="pxweb-accordion"  runat="server">
<button type="button" class="accordion-header closed" id="ApiAccordionHeader" aria-expanded="false" onclick="accordionToggle(<%=ApiAccordionPanel.ClientID %>, this)" >
    <span class="header-text"><asp:Label ID="lblTableQueryInformation"  runat="server" Text=""></asp:Label></span>
</button>

<div class="accordion-body closed">
    <asp:Panel ID="pnlQueryInformation" CssClass="tablequery_informationpanel flex-column" runat="server">
        <asp:Label ID="lblInformationText" runat="server" Text="" CssClass="tablequery_informationtext"></asp:Label>
        <asp:Label ID="lblUrl" runat="server" Text="" AssociatedControlID="txtUrl" CssClass="tablequery_urlcaption"></asp:Label>
        <asp:TextBox ID="txtUrl" runat="server" ReadOnly="true" CssClass="tablequery_url"></asp:TextBox>
        <asp:Label ID="lblQuery" runat="server" AssociatedControlID="txtQuery" Text="" CssClass="tablequery_querycaption"></asp:Label>
        <asp:TextBox ID="txtQuery" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="20" CssClass="tablequery_query"></asp:TextBox>
        <div>
            <asp:Button ID="btnSaveQuery" runat="server" CssClass="pxweb-btn pxweb-buttons icon-placement download-icon"></asp:Button>
            <div class="s-margin-top">
                <div class="pxweb-link with-icon">
                    <asp:HyperLink ID="lnkMoreInfo" runat="server" rel="noopener" CssClass="external-link-icon"></asp:HyperLink>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
</asp:panel>
