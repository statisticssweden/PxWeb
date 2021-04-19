<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-Presentation-General.aspx.cs" Inherits="PXWeb.Admin.Settings_Presentation_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblPromptNotes" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationGeneralPromptNotes %>">"></asp:Label>
        <asp:DropDownList ID="cboPromptNotes" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgPromptNotesInfo" runat="server" onclick="imgPromptNotesInfo_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>

    <div class="setting-field">
        <asp:Label ID="lblNewTitleLayout" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationGeneralNewTitleLayout %>"></asp:Label>
        <asp:DropDownList ID="cboNewTitleLayout" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgNewTitleLayout" runat="server" onclick="NewTitleLayoutInfo_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
</asp:Content>
