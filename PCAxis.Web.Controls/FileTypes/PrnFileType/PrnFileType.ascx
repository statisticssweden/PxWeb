<%@ control inherits="PCAxis.Web.Controls.PrnFileTypeCodebehind" %>
<asp:PlaceHolder ID="FileFormatContainer" runat="server">
    <asp:ListBox ID="HeadingValues" runat="server" CssClass="commandbar_saveas_prn_headingvalues" />
    <asp:ListBox ID="SeparatorValues" runat="server" CssClass="commandbar_saveas_prn_separatorvalues" />
</asp:PlaceHolder>
<asp:Button ID="ContinueButton" runat="server" />
<asp:Button ID="CancelButton" runat="server" />