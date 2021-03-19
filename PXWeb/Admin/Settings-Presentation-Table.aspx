<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-Presentation-Table.aspx.cs" Inherits="PXWeb.Admin.Settings_Presentation_Table" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblTableTransform" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableTransform %>">"></asp:Label>
        <asp:DropDownList ID="cboTableTransform" runat="server">
            <asp:ListItem Value="NoTransformation" Text="<%$ PxString: PxWebAdminSettingsPresentationTableTransformNoTransformation %>"></asp:ListItem>
            <asp:ListItem Value="SingleValueFirst" Text="<%$ PxString: PxWebAdminSettingsPresentationTableTransformSingleValueFirst %>"></asp:ListItem>
            <asp:ListItem Value="SingleValueFirstAndHeaderOnlyOneMultiple" Text="<%$ PxString: PxWebAdminSettingsPresentationTableTransformSingleValueFirstAndHeaderOnlyOneMultiple %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgTableTransform" runat="server" onclick="TableTransformInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    
    <br />
    
    <div class="setting-field">
        <asp:Label ID="lblDefaultLayout" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationDefaultLayout %>">"></asp:Label>
        <asp:DropDownList ID="cboDefaultLayout" runat="server">
            <asp:ListItem Value="Layout1" Text="<%$ PxString: PxWebAdminSettingsPresentationDefaultLayoutLayout1 %>"></asp:ListItem>
            <asp:ListItem Value="Layout2" Text="<%$ PxString: PxWebAdminSettingsPresentationDefaultLayoutLayout2 %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDefaultLayout" runat="server" onclick="DefaultLayoutInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    
    <div class="setting-field">
        <asp:Label ID="lblMaxColumns" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableMaxColumns %>"></asp:Label>
        <asp:TextBox ID="txtMaxColumns" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgMaxColumns" runat="server" onclick="MaxColumnsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMaxColumns" runat="server" 
        ControlToValidate="txtMaxColumns" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
    <div class="setting-field">
        <asp:Label ID="lblMaxRows" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableMaxRows %>"></asp:Label>
        <asp:TextBox ID="txtMaxRows" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgMaxRows" runat="server" onclick="MaxRowsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMaxRows" runat="server" 
        ControlToValidate="txtMaxRows" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
    <div class="setting-field">
        <asp:Label ID="lblTitleVisible" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableTitleVisible %>"></asp:Label>
        <asp:DropDownList ID="cboTitleVisible" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgTitleVisible" runat="server" onclick="TitleVisibleInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDisplayAttributes" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableDisplayAttributes %>"></asp:Label>
        <asp:DropDownList ID="cboDisplayAttributes" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDisplayAttributes" runat="server" onclick="DisplayAttributesInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDisplayDefaultAttributes" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableDisplayDefaultAttributes %>"></asp:Label>
        <asp:DropDownList ID="cboDisplayDefaultAttributes" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDisplayDefaultAttributes" runat="server" onclick="DisplayDefaultAttributesInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
        <div class="setting-field">
        <asp:Label ID="lblStickyHeaderFullscreen" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationTableStickyHeaderFullscreen %>"></asp:Label>
        <asp:DropDownList ID="cboStickyHeaderFullscreen" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgStickyHeaderFullscreen" runat="server" onclick="StickyHeaderFullscreen" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    
</asp:Content>
