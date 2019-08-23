<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-Site.aspx.cs" Inherits="PXWeb.Admin.Settings_General_Site" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblApplicationName" runat="server" Text="<%$ PxString: PxWebAdminSettingsGeneralSiteApplicationName %>"></asp:Label>
        <asp:TextBox ID="txtApplicationName" runat="server" Text="<%$ PxSetting: PXWeb.Settings.Current.General.Site.ApplicationName %>"></asp:TextBox>
        <asp:ImageButton ID="imgApplicationName" runat="server" onclick="imgApplicationName_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>        
    </div>
    
    <div class="setting-field">
        <asp:Label ID="lblLogoPath" runat="server" Text="<%$ PxString: PxWebAdminSettingsGeneralSiteLogoPath %>"></asp:Label>
        <asp:TextBox ID="txtLogoPath" runat="server" Text="<%$ PxSetting: PXWeb.Settings.Current.General.Site.LogoPath %>"></asp:TextBox>
        <asp:ImageButton ID="imgLogoPath" runat="server" onclick="imgLogoPath_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorLogoPath" runat="server" 
        ControlToValidate="txtLogoPath" OnServerValidate="ValidateLogoPath"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

</asp:Content>
