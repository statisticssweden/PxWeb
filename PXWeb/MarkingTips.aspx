<%@ Page Language="C#" MasterPageFile="~/PxWeb.Master" AutoEventWireup="true" CodeBehind="MarkingTips.aspx.cs" Inherits="PXWeb.MarkingTips" Title="<%$ PxString: PxWebTitleMarkingTips %>" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <p>
        <asp:Label ID="lblHeader" runat="server" Text="Marking tips" CssClass="headingtext"></asp:Label>
    </p>
    <br />
    <asp:Panel ID="pnlTips" runat="server" CssClass="markingTipsText">
        <asp:Literal ID="litTips" runat="server" ></asp:Literal>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
