<%@ control  inherits="PCAxis.Web.Controls.SaveAsChartCodebehind" %>

<asp:PlaceHolder ID="ChartFileFormatContainer" runat="server">
    <asp:ListBox ID="ChartFileFormats" runat="server" CssClass="commandbar_saveas_chart_fileformats" />
</asp:PlaceHolder>

<asp:Button ID="ContinueButton" runat="server" CssClass="chartcontinue" />
<asp:Button ID="CancelButton" runat="server" CssClass="chartcontinue" />