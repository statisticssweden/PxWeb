<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-Administration.aspx.cs" Inherits="PXWeb.Admin.Settings_General_Administration" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblUseIPFilter" runat="server" Text="<%$ PxString: PxWebAdminSettingsAdministrationUseIPFilter %>"></asp:Label>
        <asp:DropDownList ID="cboUseIPFilter" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgUseIPFilter" runat="server" onclick="UseIPFilterInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <p>
        <asp:Label ID="lblIpAddresses" runat="server" Text="<%$ PxString: PxWebAdminSettingsAdministrationIPAddresses %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgIPAddresses" runat="server" 
            onclick="IPAddressesInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/> <br />
        <asp:Repeater ID="rptIPAddresses" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:TextBox ID="txtIPAddress" runat="server" Text="<%#Container.DataItem %>"></asp:TextBox>
                <asp:Label ID="lblError" runat="server" Text="" CssClass="errortext"></asp:Label>
                <br />
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>
    </p>
    <p>
        <asp:Label ID="Label1" runat="server" Text="<%$ PxString: PxWebAdminSettingsAdministrationAddNewIPAddress %>" ></asp:Label><br />
        <asp:TextBox ID="txtAddNewIPAddress" runat="server"></asp:TextBox>
        <asp:Label ID="lblAddNewIPAddressError" runat="server" CssClass="errortext"></asp:Label>
        <asp:Button ID="btnAddNewIPAddress" runat="server" onclick="AddNewIPAddress" Text="<%$ PxString: PxWebAdminSettingsAdministrationAddNewIPAddress %>" />
    </p>
</asp:Content>
