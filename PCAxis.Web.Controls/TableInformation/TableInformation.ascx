<%@ control language="VB" inherits="PCAxis.Web.Controls.TableInformationCodebehind" explicit="true"  %> 
<asp:Panel runat="server" ID="TableInformationPanel" CssClass="tableinformation_container">
    <asp:Literal ID="lblTableTitle" runat="server" />    
    <asp:Panel runat="server" ID="pnlTableDescription">
        <asp:Label ID="lblTableDescription" runat="server" CssClass="tableinformation_description" />       
    </asp:Panel>    
    
</asp:Panel>