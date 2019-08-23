<%@ control inherits="PCAxis.Web.Controls.VariableSelectorEliminationInformationCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>
<asp:PlaceHolder ID="EliminationTextPlaceholder" runat="server">
    <p>
        <asp:Label runat="server" ID="EliminationInformationText" CssClass="variableselector_eliminationinformation_text" />
            <asp:Image runat="server" ID="EliminationInformationImage" CssClass="variableselector_eliminationinformation_image"/>
        <asp:Label runat="server" ID="EliminationInformationTextEnd" CssClass="variableselector_eliminationinformation_text" />
    </p>
</asp:PlaceHolder>
    
