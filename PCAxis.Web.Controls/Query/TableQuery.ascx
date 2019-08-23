<%@ control  inherits="PCAxis.Web.Controls.TableQueryCodebehind" %>

<div class="tablequery_container">
    <a id="tablequerycontrol"></a>
        <asp:Panel ID="pnlQueryInformationHidden" CssClass="tablequery_informationpanel" runat="server">
            <asp:HyperLink ID="lnkTableQueryInformation" runat="server" CssClass="tablequery_showinformationlink"></asp:HyperLink>
        </asp:Panel>
        <asp:Panel ID="pnlQueryInformation" CssClass="tablequery_informationpanel" runat="server">
            <asp:HyperLink ID="lnkHideInformation" runat="server" CssClass="tablequery_hideinformationlink"></asp:HyperLink>
            <asp:Label ID="lblInformationText" runat="server" Text="" CssClass="tablequery_informationtext"></asp:Label>
            <asp:Label ID="lblUrl" runat="server" Text="" CssClass="tablequery_urlcaption"></asp:Label>
            <asp:TextBox ID="txtUrl" runat="server" ReadOnly="true" CssClass="tablequery_url"></asp:TextBox>
            <asp:Label ID="lblQuery" runat="server" Text="" CssClass="tablequery_querycaption"></asp:Label>
            <asp:TextBox ID="txtQuery" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="20" CssClass="tablequery_query"></asp:TextBox>
            <asp:HyperLink ID="lnkMoreInfo" runat="server" CssClass="tablequery_moreinformation"></asp:HyperLink>    
        </asp:Panel>
</div>   

<script type="text/javascript" >

    jQuery(document).ready(function () {
        jQuery('.tablequery_showinformationlink').click(function () {
            jQuery('[id$=pnlQueryInformation]').show(0);
            jQuery('[id$=pnlQueryInformationHidden]').hide(0);
            return false;
        });
        jQuery('.tablequery_hideinformationlink').click(function () {
            jQuery('[id$=pnlQueryInformation]').hide(0);
            jQuery('[id$=pnlQueryInformationHidden]').show(0);
            return false;
        });
    });

</script>    

