<%@ Page Title="<%$ PxString: PxWebTitleDefault %>" Language="C#" MasterPageFile="~/PxWeb.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PXWeb.Default" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<%@ Register TagPrefix="pxwebCC" Namespace="PXWeb.CustomControls" Assembly="PXWeb" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="pxcontent"> 
        <pxwebCC:UserManualScreenReader ID="UserManualDatabases" manualFor="Databases" runat="server" ClientIDMode="Static"/> 
    </div>
    <div class="databaseList">
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
    </div>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
