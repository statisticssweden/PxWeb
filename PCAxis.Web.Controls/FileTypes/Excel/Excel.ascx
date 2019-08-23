<%@ control  inherits="PCAxis.Web.Controls.ExcelCodebehind" %>
<!--<asp:PlaceHolder ID="FileFormatContainer" runat="server">
    <asp:RadioButtonList ID="InformationLevel" runat="server" CssClass="commandbar_saveas_excel_informationtype">
    </asp:RadioButtonList>
</asp:PlaceHolder>
<asp:CheckBox ID="chkDoubleColumn" runat="server" CssClass="exceldoublecolumn" />-->

<asp:PlaceHolder ID="ExcelFileFormatContainer" runat="server">
    <asp:ListBox ID="ExcelFileFormats" runat="server" CssClass="commandbar_saveas_excel_fileformats" />
</asp:PlaceHolder>

<asp:Button ID="ContinueButton" runat="server" CssClass="excelcontinue" />
<asp:Button ID="CancelButton" runat="server" CssClass="excelcontinue" />