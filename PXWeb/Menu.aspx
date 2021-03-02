<%@ Page Title="<%$ PxString: PxWebTitleMenu %>" Language="C#" MasterPageFile="~/PxWeb.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="PXWeb.Menu" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>

<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>
<%@ Register Src="~/UserControls/SearchControl.ascx" TagPrefix="uc1" TagName="SearchControl" %>
<%@ Register Src="~/UserControls/MenuExplanation.ascx" TagPrefix="uc1" TagName="MenuExplanation" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="menuTopLeftContent">
        <h1>
            <asp:Label ID="lblDatabase" CssClass="menu_databasename" runat="server" Text="" ></asp:Label>
        </h1>
    </div>
    <div class="menuTopRightContent">
        <uc1:SearchControl runat="server" ID="pxSearch" />
    </div>
    <div class="break"></div>
    <pxc:TableOfContent ID="TableOfContent1" runat="server" />
    <pxc:TableList ID="TableList1" runat="server" />

    <div id="dialogModal" runat="server" visible="false" enableviewstate="false">
        <asp:Label ID="lblMsg" runat="server" />    
    </div>

    <div>
        <uc1:MenuExplanation runat="server" ID="MenuExplanation" />
    </div>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
