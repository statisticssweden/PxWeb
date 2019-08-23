<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-LanguageManager.aspx.cs" Inherits="PXWeb.Admin.Tools_LanguageManager" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <asp:DropDownList ID="cboLanguage" runat="server" AutoPostBack="true" 
        onselectedindexchanged="cboLanguage_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:ImageButton ID="imgLanguageManager" runat="server" onclick="LanguageManagerInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    <asp:CustomValidator ID="validatorLanguage" runat="server" 
    ControlToValidate="cboLanguage" OnServerValidate="ValidateLanguage"
    ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    <br /><br />
    <asp:PlaceHolder ID="PlaceHolderTable" runat="server">
        <table id="tblLanguage" runat="server" border="0" cellspacing="0" cellpadding="5" class="languageManagerTable">
        <tr>
            <th class="languageManagerKeyColumn"><asp:Label ID="lblKey" runat="server" Text="<%$ PxString: PxWebAdminToolsLanguageManagerKey %>"></asp:Label></th>
            <th class="languageManagerFallbackColumn"><asp:Label ID="lblFallback" runat="server" Text="<%$ PxString: PxWebAdminToolsLanguageManagerFallback %>"></asp:Label></th>
            <th class="languageManagerLanguageColumn"><asp:Label ID="lblLanguage" runat="server" Text="<%$ PxString: PxWebAdminToolsLanguageManagerLanguage %>"></asp:Label></th>
        </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
