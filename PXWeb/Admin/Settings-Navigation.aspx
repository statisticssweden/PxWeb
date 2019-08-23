<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeBehind="Settings-Navigation.aspx.cs"  Inherits="PXWeb.Admin.Settings_Navigation" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblNavigationFlow" runat="server" Text="<%$ PxString: PxWebAdminSettingsNavigationNavigationFlowVisible %>">"></asp:Label>
        <asp:DropDownList ID="cboShowNavigationFlow" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgNavigationFlowInfo" runat="server" onclick="imgNavigationFlowInfo_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
</asp:Content>

