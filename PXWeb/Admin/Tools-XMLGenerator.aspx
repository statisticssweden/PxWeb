<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-XMLGenerator.aspx.cs" Inherits="PXWeb.Admin.Tools_XMLGenerator" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">

    <div class="setting-field">
        <asp:Label ID="lblSelectDbType" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectDbType %>">"></asp:Label>
        <asp:DropDownList ID="cboSelectDbType" onselectedindexchanged="cboSelectDbType_SelectedIndexChanged" AutoPostBack="true" runat="server">
            <asp:ListItem Value="PX" Text="PX"></asp:ListItem>
            <asp:ListItem Value="CNMM" Text="CNMM"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgSelectDbTypeInfo" runat="server" onclick="imgSelectDbType_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectDb" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectDb %>">"></asp:Label>
        <asp:DropDownList ID="cboSelectDb" onselectedindexchanged="cboSelectDb_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
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
        <asp:ImageButton ID="imgSelectLicense" runat="server" onclick="imgSelectLicense_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectApiURL" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectApiURL %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectApiURL" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectApiURL" runat="server" onclick="imgSelectApiURL_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectLandingPageURL" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectLandingPageURL %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectLandingPageURL" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectLandingPageURL" runat="server" onclick="imgSelectLandingPageURL_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectPublisher" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorSelectPublisher %>">"></asp:Label>
        <asp:TextBox ID="textBoxSelectPublisher" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSelectPublisher" runat="server" onclick="imgSelectPublisher_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
        <div class="setting-field">
        <asp:Label ID="lblStatus" runat="server" Text="<%$ PxString: PxWebAdminToolsXMLGeneratorStatus %>">"></asp:Label>
        <asp:Label ID="lblStatusValue" runat="server" Text="NotCreated"></asp:Label>
        <asp:ImageButton ID="imgStatus" runat="server" onclick="imgStatus_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>

     <div class="setting-field">
         <asp:Button ID="btnGenerateXML" onclick="btnGenerateXML_Click" runat="server" Text="<%$ PxString: PxWebAdminGenerateButton %>" />
    </div>



</asp:Content>