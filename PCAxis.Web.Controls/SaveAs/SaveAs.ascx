<%@ control inherits="PCAxis.Web.Controls.SaveAsCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<div class="saveas">
    <%--<asp:PlaceHolder ID="Content" runat="server" />--%>
    <asp:DropDownList ID="FileFormatDropDownList" runat="server" />
    <asp:ListBox ID="FileFormatListBox" runat="server" />
    <asp:Button ID="ContinueButton" runat="server" />
    <asp:PlaceHolder ID="Footer" runat="server" Visible="false" /> 
</div>

