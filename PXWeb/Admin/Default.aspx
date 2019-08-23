<%@ Page Language="C#" MasterPageFile="~/Admin/AdminDefault.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PXWeb.Admin.Default" Title="PX-Web Administration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
<center>
    <div id="AdminHomeWrapper">
        <asp:Label ID="lblHeading" runat="server" Text="<%$ PxString: PxWebAdminHeading %>" CssClass="AdminHomeTitle"></asp:Label><br />
        <asp:Image ID="imgAdmin" runat="server" CssClass="AdminHomeImage" ImageUrl="<%$ PxImage: px-web-logo.gif %>" Height="100" Width="100" /><br />
        <asp:Label ID="lblVersion" runat="server" Text="<%$ PxString: PxWebAdminVersion %>"></asp:Label><br />
        <asp:HyperLink ID="lnkHomepage" runat="server" Text="<%$ PxString: PxWebAdminHomePage %>" NavigateUrl="http://www.scb.se/pc-axis" Target="_blank"></asp:HyperLink>
    </div>
</center>
</asp:Content>
