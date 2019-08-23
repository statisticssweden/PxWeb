<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-ChangePassword.aspx.cs" Inherits="PXWeb.Admin.Tools_ChangePassword" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblOldPassword" runat="server" Text="<%$ PxString: PxWebAdminToolsChangePasswordOld %>"></asp:Label>
        <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:CustomValidator ID="validatorOldPassword" runat="server" 
        ControlToValidate="txtOldPassword" OnServerValidate="ValidateOldPassword" 
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblNewPassword" runat="server" Text="<%$ PxString: PxWebAdminToolsChangePasswordNew %>"></asp:Label>
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:CustomValidator ID="validatorNewPassword" runat="server" 
        ControlToValidate="txtNewPassword" OnServerValidate="ValidateNewPassword" 
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblVerifyPassword" runat="server" Text="<%$ PxString: PxWebAdminToolsChangePasswordVerify %>"></asp:Label>
        <asp:TextBox ID="txtVerifyPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:CustomValidator ID="validatorVerifyPassword" runat="server" 
        ControlToValidate="txtVerifyPassword" OnServerValidate="ValidateVerifyPassword" 
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Button ID="btnChange" runat="server" CssClass="toolbutton" 
            Text="<%$ PxString: PxWebAdminToolsChangePasswordChange %>" 
            onclick="btnChange_Click" />
    </div>
</asp:Content>
