<%@ control inherits="PCAxis.Web.Controls.VariableSelectorOutputFormatsCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<asp:PlaceHolder ID="OutputFormatPlaceHolder" runat="server">
         <asp:DropDownList ID="OutputFormatDropDownList" runat="server" CssClass="variableselector_outputformats_dropdown" />
         <asp:panel id="FileTypeControlHolder" runat="server" visible="false"></asp:panel>
</asp:PlaceHolder>    
