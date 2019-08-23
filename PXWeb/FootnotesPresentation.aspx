<%@ Page Language="C#" MasterPageFile="~/Presentation.Master" AutoEventWireup="true" CodeBehind="FootnotesPresentation.aspx.cs" Inherits="PXWeb.FootnotesPresentation" Title="<%$ PxString: PxFootnotesPresentation %>" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="cphMain" runat="server">
        <pxc:TableInformation runat="server" ID="TableInformationFootnoteView" Type="PresentationView" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" ShowSourceDescription="true" />
        <pxc:Footnote ID="FootnoteSelectLink" runat="server" />
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
