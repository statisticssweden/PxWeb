<%@ control  inherits="PCAxis.Web.Controls.TableQueryCodebehind" %>

<asp:panel ID="ApiAccordionPanel" class="pxweb-accordion"  runat="server">
<button type="button" class="accordion-header closed" id="ApiAccordionHeader" aria-expanded="false" onclick="accordionToggle(<%=ApiAccordionPanel.ClientID %>, this)" >
    <span class="header-text"><asp:Label ID="lblTableQueryInformation"  runat="server" Text=""></asp:Label></span>
</button>

<div>
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
        <asp:Panel ID="pnlQueryInformationV2" CssClass="tablequery_informationpanel" runat="server">
            <asp:Label ID="lblV2text" runat="server" Text="" CssClass="tablequery_V2caption">Api-V2:</asp:Label>
            <asp:Label ID="lblInformationTextV2" runat="server" Text="" CssClass="tablequery_informationtext"></asp:Label>
            <asp:Label ID="lblUrlV2" runat="server" Text="" AssociatedControlID="txtUrl" CssClass="tablequery_urlcaption"></asp:Label>
            <asp:TextBox ID="txtUrlV2" runat="server" ReadOnly="true" CssClass="tablequery_url"></asp:TextBox>
            <asp:Button ID="btnCopyUrlV2" text="<%$ PxString: CtrlTableQueryCopy %>" runat="server" ClientIDMode="static" CssClass="pxweb-btn pxweb-buttons" OnClientClick="CopyQuery();return false" CausesValidation="false"></asp:Button>
        </asp:Panel>
    </div>
</div>
</asp:panel>
<script type="text/javascript">
    function CopyQuery() {
        var copyText = document.getElementById('<%=txtUrlV2.ClientID %>')
        copyText.focus();
        copyText.select(); // for mark as copied

        // Copy the text inside the text field
        navigator.clipboard.writeText("<%=txtUrlV2.Text%>");
        return false;
    }
</script>
