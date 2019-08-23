<%@ Page Language="C#" MasterPageFile="~/Presentation.Master" AutoEventWireup="true" CodeBehind="InformationPresentation.aspx.cs" Inherits="PXWeb.InformationPresentation" Title="<%$ PxString: PxWebTitleInformaionPresentaion %>" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="cphMain" runat="server">
    <pxc:TableInformation runat="server" ID="TableInformationInformationView" Type="PresentationView" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" />
    <pxc:information id="InformationView" runat="server"></pxc:information>
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
