<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchControl.ascx.cs" Inherits="PXWeb.UserControls.SearchControl" %>

<div class="search_container">
    <asp:Panel ID="pnlSearch" CssClass="search_panel" runat="server" DefaultButton="cmdSearch">
        <asp:Label ID="lblSearch" AssociatedControlID="txtSearch" CssClass="search_label" runat="server" Text=""></asp:Label>
        <asp:TextBox ID="txtSearch" CssClass="search_textbox" runat="server"></asp:TextBox>
        <asp:Button ID="cmdSearch" CssClass="search_button" runat="server" Text="Search" OnClick="GoToSearch" />
        <asp:Label ID="lblError" CssClass="search_error" runat="server" Text=""></asp:Label>
        <asp:HiddenField ID="hidRedirect" runat="server" Value="True" />
    </asp:Panel>
</div>   
