<%@ Page Language="C#" MasterPageFile="~/PxWeb.Master" AutoEventWireup="true" CodeBehind="FootnotesSelection.aspx.cs" Inherits="PXWeb.FootnotesSelection" Title="<%$ PxString: PxWebTitleFootnotesSelection %>" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
        <pxc:TableInformation runat="server" ID="TableInformationFootnoteSelect" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="true" Visible="true" />
        <pxc:Footnote ID="FootnoteSelectLink" runat="server" />
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
