<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-XMLGenerator.aspx.cs" Inherits="PXWeb.Admin.Tools_XMLGenerator" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblSelectDb" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectDb %>">"></asp:Label>
        <asp:DropDownList ID="cboSelectDb" runat="server"></asp:DropDownList>
        <asp:ImageButton ID="imgSelectDbInfo" runat="server" onclick="imgSelectDb_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectPreferredLanguage" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectPreferredLanguage %>">"></asp:Label>
        <asp:DropDownList ID="cboLanguage" runat="server" AutoPostBack="true" ></asp:DropDownList>
        <asp:ImageButton ID="imgSelectPreferredLanguageInfo" runat="server" onclick="imgSelectPreferredLanguage_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>

</asp:Content>