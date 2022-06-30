<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-XMLGenerator.aspx.cs" Inherits="PXWeb.Admin.Tools_XMLGenerator" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblSelectDb" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectDb %>">"></asp:Label>
        <asp:DropDownList ID="cboSelectDb" runat="server"></asp:DropDownList>
        <asp:ImageButton ID="imgSelectDbInfo" runat="server" onclick="imgSelectDb_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectBaseURI" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectBaseURI %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectBaseURI" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectBaseURI" runat="server" onclick="imgSelectBaseURI_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectCatalogTitle" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectCatalogTitle %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectCatalogTitle" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectCatalogTitle" runat="server" onclick="imgSelectCatalogTitle_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectCatalogDesc" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectCatalogDesc %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectCatalogDesc" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectCatalogDesc" runat="server" onclick="imgSelectCatalogDesc_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectLicense" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectLicense %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectLicense" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectLicense" runat="server" onclick="imgSelectBaseURI_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>

     <div class="setting-field">
         <asp:Button ID="btnGenerateXML" onclick="btnGenerateXML_Click" runat="server" Text="<%$ PxString: PxWebAdminGenerateButton %>" />
    </div>



</asp:Content>