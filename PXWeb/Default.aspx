<%@ Page Title="<%$ PxString: PxWebTitleDefault %>" Language="C#" MasterPageFile="~/PxWeb.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PXWeb.Default" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:Label ID="lblSelectDb" runat="server" Text="<%$ PxString: PxWebSelectDb %>" CssClass="headingtext"></asp:Label>
    <br />
    <asp:Repeater ID="rptDatabases" runat="server" EnableViewState="false" onitemdatabound="rptDatabases_ItemDataBound">
        <HeaderTemplate/>
        <ItemTemplate>
            <asp:HyperLink runat="server" ID="lnkDatabasesItem" NavigateUrl=''>
                <%# DatabaseName(Container.DataItem)%>
            </asp:HyperLink>
            <br />
        </ItemTemplate>
        <AlternatingItemTemplate>
            <asp:HyperLink runat="server" ID="lnkDatabasesItem" NavigateUrl=''>
                <%# DatabaseName(Container.DataItem)%>
            </asp:HyperLink>
            <br />
        </AlternatingItemTemplate>
        <FooterTemplate/>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
