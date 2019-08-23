<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-Language.aspx.cs" Inherits="PXWeb.Admin.Settings_General_Language" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    
    <div class="setting-field">
    </div>
        <asp:Repeater ID="rptSiteLanguages" runat="server">
            <HeaderTemplate>
                <div class="settingsListingTextLanguageHeader">
                    <asp:Label ID="lblSiteLanguages" runat="server" Text="<%$ PxString: PxWebAdminSettingsLanguageSiteLanguages %>"></asp:Label>
                    <asp:ImageButton ID="imgSiteLanguages" runat="server" onclick="imgSiteLanguagesInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
                </div>
                <div class="settingsListingDropdownLanguageHeader">
                    <asp:Label ID="Label1" runat="server" Text="<%$ PxString: PxWebAdminSettingsLanguageDecimalSeparator %>"></asp:Label>
                    <asp:ImageButton ID="ImageButton1" runat="server" onclick="imgDecimalSeparatorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
                </div>
                <div class="settingsListingDropdownLanguageHeader">
                    <asp:Label ID="Label2" runat="server" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparator %>"></asp:Label>
                    <asp:ImageButton ID="ImageButton2" runat="server" onclick="imgThousandSeparatorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
                </div>
                <div class="settingsListingTextboxLanguageHeader">
                    <asp:Label ID="Label3" runat="server" Text="<%$ PxString: PxWebAdminSettingsLanguageDateFormat %>"></asp:Label>
                    <asp:ImageButton ID="ImageButton3" runat="server" onclick="imgDateFormatInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <p class="oddRow">
                    <asp:HiddenField ID="hidSetting" runat="server" value='<%# Bind("Id") %>' />
                    <asp:CheckBox ID="cbxLanguage" CssClass="settingsListingTextLanguage" runat="server" Text=<%# Eval("Name") %> Checked=<%# Eval("Selected") %> />
                    <asp:DropDownList ID="cboDecimalSeparator" CssClass="settingsListingDropdownLanguage" SelectedValue=<%# Eval("DecimalSeparator") %> runat="server" >
                        <asp:ListItem Value="Point" Text="<%$ PxString: PxWebAdminSettingsLanguageDecimalSeparatorPoint %>"></asp:ListItem>
                        <asp:ListItem Value="Comma" Text="<%$ PxString: PxWebAdminSettingsLanguageDecimalSeparatorComma %>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboThousandSeparator" CssClass="settingsListingTextLanguage" SelectedValue=<%# Eval("ThousandSeparator") %> runat="server" >
                        <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorNone %>"></asp:ListItem>
                        <asp:ListItem Value="Space" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorSpace %>"></asp:ListItem>
                        <asp:ListItem Value="Point" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorPoint %>"></asp:ListItem>
                        <asp:ListItem Value="Comma" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorComma %>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtDateFormat" CssClass="settingsListingTextboxLanguage" Text=<%# Eval("DateFormat") %> runat="server"></asp:TextBox>
                </p>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <p class="evenRow">
                    <asp:HiddenField ID="hidSetting" runat="server" value='<%# Bind("Id") %>' />
                    <asp:CheckBox ID="cbxLanguage" CssClass="settingsListingTextLanguage" runat="server" Text=<%# Eval("Name") %> Checked=<%# Eval("Selected") %> />
                    <asp:DropDownList ID="cboDecimalSeparator" CssClass="settingsListingDropdownLanguage" SelectedValue=<%# Eval("DecimalSeparator") %> runat="server" >
                        <asp:ListItem Value="Point" Text="<%$ PxString: PxWebAdminSettingsLanguageDecimalSeparatorPoint %>"></asp:ListItem>
                        <asp:ListItem Value="Comma" Text="<%$ PxString: PxWebAdminSettingsLanguageDecimalSeparatorComma %>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboThousandSeparator" CssClass="settingsListingTextLanguage" SelectedValue=<%# Eval("ThousandSeparator") %> runat="server" >
                        <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorNone %>"></asp:ListItem>
                        <asp:ListItem Value="Space" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorSpace %>"></asp:ListItem>
                        <asp:ListItem Value="Point" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorPoint %>"></asp:ListItem>
                        <asp:ListItem Value="Comma" Text="<%$ PxString: PxWebAdminSettingsLanguageThousandSeparatorComma %>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtDateFormat" CssClass="settingsListingTextboxLanguage" Text=<%# Eval("DateFormat") %> runat="server"></asp:TextBox>
                </p>
            </AlternatingItemTemplate>
        </asp:Repeater>    
<br />

    <div class="setting-field">
        <asp:Label ID="lblDefaultLanguage" runat="server" Text="<%$ PxString: PxWebAdminSettingsLanguageDefaultLanguage %>"></asp:Label>
        <asp:DropDownList ID="cboDefaultLanguage" runat="server" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
        <asp:ImageButton ID="imgDefaultLanguage" runat="server" onclick="imgDefaultLanguage_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
</asp:Content>
