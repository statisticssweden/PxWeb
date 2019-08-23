<%@ control Inherits="PCAxis.Excel.Web.Controls.SaveAsXlsxCodebehind" %>

<asp:PlaceHolder ID="ExcelFileFormatContainer" runat="server">
    <asp:ListBox ID="ExcelFileFormats" runat="server" CssClass="commandbar_saveas_excel_fileformats" />
</asp:PlaceHolder>

<asp:Button ID="ContinueButton" runat="server" CssClass="excelcontinue" />
<asp:Button ID="CancelButton" runat="server" CssClass="excelcontinue" />