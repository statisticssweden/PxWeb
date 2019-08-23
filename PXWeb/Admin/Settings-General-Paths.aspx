<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-Paths.aspx.cs" Inherits="PXWeb.Admin.Settings_General_Paths" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblLanguagePath" runat="server" Text="<%$ PxString: PxWebAdminSettingsPathsLanguagePath %>"></asp:Label>
        <asp:TextBox ID="txtLanguagePath" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgLanguagePath" runat="server" onclick="imgLanguagePath_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorLanguagePath" runat="server" 
        ControlToValidate="txtLanguagePath" OnServerValidate="ValidatePath"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblImagePath" runat="server" Text="<%$ PxString: PxWebAdminSettingsPathsImagePath %>"></asp:Label>
        <asp:TextBox ID="txtImagePath" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgImagePath" runat="server" onclick="imgImagePath_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorImagePath" runat="server" 
        ControlToValidate="txtImagePath" OnServerValidate="ValidatePath"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDatabasePath" runat="server" Text="<%$ PxString: PxWebAdminSettingsPathsPxDatabasePath %>"></asp:Label>
        <asp:TextBox ID="txtDatabasePath" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgDatabasePath" runat="server" onclick="imgDatabasePath_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorDatabasePath" runat="server" 
        ControlToValidate="txtDatabasePath" OnServerValidate="ValidatePath"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
   </div>
    <div class="setting-field">
        <asp:Label ID="lblAggregationPath" runat="server" Text="<%$ PxString: PxWebAdminSettingsPathsAggregationPath %>"></asp:Label>
         <asp:TextBox ID="txtAggregationPath" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgAggregationPath" runat="server" onclick="imgAggregationPath_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorAggregationPath" runat="server" 
        ControlToValidate="txtAggregationPath" OnServerValidate="ValidatePath"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
   </div>
</asp:Content>
