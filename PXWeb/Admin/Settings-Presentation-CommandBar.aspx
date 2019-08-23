<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-Presentation-CommandBar.aspx.cs" Inherits="PXWeb.Admin.Settings_Presentation_CommandBar" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <asp:Label ID="lbViewMode" CssClass="settingsListingTextCommandBar"  runat="server"/>
    <asp:DropDownList ID="lstViewMode" runat="server" AutoPostBack="true" onselectedindexchanged="lstViewMode_SelectedIndexChanged" />
    <asp:Panel ID="pnlContent" runat="server">

<%--       ******************
           *   OPERATIONS   *
           ******************
--%>            
                <p>&nbsp;</p>
                <asp:Label ID="lblOperations" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationCommandBarHeaderOperations %>" CssClass="setting_keyword"></asp:Label>
                <asp:ImageButton ID="imgOperations" runat="server" onclick="imgOperations_Click"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
                <p>&nbsp;</p>
                <asp:Label ID="lbHeaderSettingText" CssClass="settingsListingTextCommandBar"  runat="server"/>
                <asp:Label ID="lbHeaderOperationSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server"/>
                <asp:Label ID="lbHeaderOperationShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" />
               
                <asp:Repeater ID="rptSettings" runat="server" >
                    <HeaderTemplate/>
                    <ItemTemplate>   
                        <p class="oddRow">
                            <asp:HiddenField ID="hidSetting" runat="server" value='<%# Bind("Setting") %>' />
                            <asp:Label ID="lbSetting" CssClass="settingsListingTextCommandBar"  runat="server" Text='<%# Bind("SettingText") %>'></asp:Label>
                            <asp:CheckBox ID="cbxOperationSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OperationSelect") %>' />
                            <asp:CheckBox ID="cbxOperationShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OperationShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                        </p>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <p class="evenRow">
                            <asp:HiddenField ID="hidSetting"  runat="server" value='<%# Bind("Setting") %>' />
                            <asp:Label ID="lbSetting" CssClass="settingsListingTextCommandBar"  runat="server" Text='<%# Bind("SettingText") %>'></asp:Label>
                            <asp:CheckBox ID="cbxOperationSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OperationSelect") %>' />
                            <asp:CheckBox ID="cbxOperationShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OperationShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                        </p>
                    </AlternatingItemTemplate>
                    <FooterTemplate/>
            </asp:Repeater>
            
<%--       ********************
           *   FILE FORMATS   *
           ********************
--%>            
            <p>&nbsp;</p>
            <asp:Label ID="lblFileformats" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationCommandBarHeaderFileformats %>" CssClass="setting_keyword"></asp:Label>
                <asp:ImageButton ID="imgHeaderFileFormatText" runat="server" onclick="imgHeaderFileFormatText_Click"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
            <p>&nbsp;</p>
            <span class="settingsListingTextCommandBar">
                <asp:Label ID="lbHeaderFileFormatText" runat="server"/>
             </span>
            <asp:Label ID="lbFileFormatSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server"/>
            <asp:Label ID="lbFileFormatShortcut" CssClass="settingsListingCheckboxCommandBar" runat="server" />
            <asp:Repeater ID="rptFileFormats" runat="server" >
                    <HeaderTemplate/>
                    <ItemTemplate>   
                        <p class="oddRow">
                            <asp:HiddenField ID="hidFileFormat" runat="server" value='<%# Bind("OutputFormat") %>' />
                            <asp:Label ID="lbFileFormat" CssClass="settingsListingTextCommandBar"  runat="server" Text='<%# Bind("OutputFormatText") %>'></asp:Label>
                            <asp:CheckBox ID="cbxFileFormatSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OutputFormatSelect") %>' />
                            <asp:CheckBox ID="cbxFileFormatShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OutputFormatShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                        </p>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <p class="evenRow">
                            <asp:HiddenField ID="hidFileFormat" runat="server" value='<%# Bind("OutputFormat") %>' />
                            <asp:Label ID="lbFileFormat" CssClass="settingsListingTextCommandBar"  runat="server" Text='<%# Bind("OutputFormatText") %>'></asp:Label>
                            <asp:CheckBox ID="cbxFileFormatSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OutputFormatSelect") %>' />
                            <asp:CheckBox ID="cbxFileFormatShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("OutputFormatShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                        </p>
                    </AlternatingItemTemplate>
                    <FooterTemplate/>
            </asp:Repeater>

<%--       **************************
           *   PRESENTATION VIEWS   *
           **************************
--%>            
            <p>&nbsp;</p>
            <asp:Label ID="lblPresentationViews" runat="server" Text="<%$ PxString: PxWebAdminSettingsPresentationCommandBarHeaderPresentationViews %>" CssClass="setting_keyword"></asp:Label>
                <asp:ImageButton ID="imgPresentationViews" runat="server" onclick="imgPresentationViews_Click"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
            <p>&nbsp;</p>
                <asp:Label ID="lbHeaderPresentationViewText" CssClass="settingsListingTextCommandBar" runat="server"/>
                <asp:Label ID="lbHeaderPresentationViewSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server"/>
                <asp:Label ID="lbHeaderPresentationViewShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" />
                <asp:Label ID="lbHeaderCommandbarShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server"/>
               
                <asp:Repeater ID="rptPresentationViews" runat="server" >
                    <HeaderTemplate/>
                    <ItemTemplate>   
                        <p class="oddRow">
                            <asp:HiddenField ID="hidPresentationView" runat="server" value='<%# Bind("PresentationView") %>' />
                            <asp:Label ID="lbPresentationView" CssClass="settingsListingTextCommandBar"  runat="server" Text='<%# Bind("PresentationViewText") %>'></asp:Label>
                            <asp:CheckBox ID="cbxSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("PresentationViewSelect") %>' />
                            <asp:CheckBox ID="cbxShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("PresentationViewShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                            <asp:CheckBox ID="cbxCommandbarShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("CommandbarShortcut") %>' Visible="<%# ShowDropDownModeControl %>" />
                        </p>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <p class="evenRow">
                            <asp:HiddenField ID="hidPresentationView"  runat="server" value='<%# Bind("PresentationView") %>' />
                            <asp:Label ID="lbPresentationView" CssClass="settingsListingTextCommandBar"  runat="server" Text='<%# Bind("PresentationViewText") %>'></asp:Label>
                            <asp:CheckBox ID="cbxSelect" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("PresentationViewSelect") %>' />
                            <asp:CheckBox ID="cbxShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("PresentationViewShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                            <asp:CheckBox ID="cbxCommandbarShortcut" CssClass="settingsListingCheckboxCommandBar"  runat="server" Checked='<%# Bind("CommandbarShortcut") %>' Visible="<%# ShowDropDownModeControl %>"/>
                        </p>
                    </AlternatingItemTemplate>
                    <FooterTemplate/>
            </asp:Repeater>

    </asp:Panel>
         
</asp:Content>
