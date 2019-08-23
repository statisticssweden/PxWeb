<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Features-Search-General.aspx.cs" Inherits="PXWeb.Admin.Features_Search_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdminHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <p>        
        <asp:Label ID="lblSearchSetting" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSearchGeneralSearchSettings %>" CssClass="setting_keyword"></asp:Label>
    </p>
    <div class="setting-field">
        <asp:Label ID="lblCacheTime" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSearchGeneralCacheTime %>"></asp:Label>
        <asp:TextBox ID="txtCacheTime" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgCacheTime" runat="server" onclick="CacheTimeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorCacheTime" runat="server" 
        ControlToValidate="txtCacheTime" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblResultListLength" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSearchGeneralResultListLength %>"></asp:Label>
        <asp:TextBox ID="txtResultListLength" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgResultListLength" runat="server" onclick="ResultListLengthInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorResultListLength" runat="server" 
        ControlToValidate="txtResultListLength" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDefaultOperator" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSearchGeneralDefaultOperator %>"></asp:Label>
        <asp:DropDownList ID="cboDefaultOperator" runat="server">
            <asp:ListItem Value="AND" Text="<%$ PxString: PxWebAdminFeaturesSearchGeneralDefaultOperatorAnd %>"></asp:ListItem>
            <asp:ListItem Value="OR" Text="<%$ PxString: PxWebAdminFeaturesSearchGeneralDefaultOperatorOr %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDefaultOperator" runat="server" onclick="DefaultOperatorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

</asp:Content>
