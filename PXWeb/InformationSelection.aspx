<%@ Page Language="C#" MasterPageFile="~/PxWeb.Master" AutoEventWireup="true" CodeBehind="InformationSelection.aspx.cs" Inherits="PXWeb.InformationSelection" Title="<%$ PxString: PxWebInforamtionSelection %>" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <pxc:TableInformation runat="server" ID="TableInformationInformationSelect" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" />
    <pxc:information id="InformationSelect" runat="server"></pxc:information>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
