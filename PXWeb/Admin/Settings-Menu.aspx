<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-Menu.aspx.cs" Inherits="PXWeb.Admin.Settings_Menu" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblMenuMode" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuMenuMode %>"></asp:Label>
        <asp:DropDownList ID="cboMenuMode" runat="server" AutoPostBack="true" 
            onselectedindexchanged="cboMenuMode_SelectedIndexChanged">
            <asp:ListItem Value="List" Text="<%$ PxString: PxWebAdminSettingsMenuMenuModeList %>"></asp:ListItem>
            <asp:ListItem Value="TreeViewWithoutFiles" Text="<%$ PxString: PxWebAdminSettingsMenuMenuModeTreeViewWithoutFiles %>"></asp:ListItem>
            <asp:ListItem Value="TreeViewWithFiles" Text="<%$ PxString: PxWebAdminSettingsMenuMenuModeTreeViewWithFiles %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgMenuMode" runat="server" onclick="MenuModeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <br />
    <asp:Panel ID="pnlWithoutAndFiles" runat="server">
         <div class="setting-field">
            <asp:Label ID="lblTree1ShowRoot" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowRoot %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowRoot" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowRoot" runat="server" onclick="ShowRootInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
         <div class="setting-field">
            <asp:Label ID="lblTree1ExpandAll" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuExpandAll %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ExpandAll" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ExpandAll" runat="server" onclick="ExpandAllInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
<!--        
         <div class="setting-field">
            <asp:Label ID="lblTree1SortByAlias" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuSortByAlias %>"></asp:Label>
            <asp:DropDownList ID="cboTree1SortByAlias" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1SortByAlias" runat="server" onclick="SortByAliasInfo"/>
        </div>
-->
        <div class="setting-field">
            <asp:Label ID="lblTree1ShowSelectionLink" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowSelectionLink %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowSelectionLink" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowSelectionLink" runat="server" onclick="ShowSelectionLinkInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1ShowDownloadLink" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowDownloadLink %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowDownloadLink" runat="server">
                <asp:ListItem Value="AlwaysHide" Text="<%$ PxString: PxWebAdminSettingsMenuShowDownloadLinkAlwaysHide %>"></asp:ListItem>
                <asp:ListItem Value="AlwaysShow" Text="<%$ PxString: PxWebAdminSettingsMenuShowDownloadLinkAlwaysShow %>"></asp:ListItem>
                <asp:ListItem Value="ShowIfSmallFile" Text="<%$ PxString: PxWebAdminSettingsMenuShowDownloadLinkShowIfSmallFile %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowDownloadLink" runat="server" onclick="ShowDownloadLinkInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1ViewLinkMode" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuViewLinkMode %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ViewLinkMode" runat="server" AutoPostBack="true" 
                onselectedindexchanged="cboTree1ViewLinkMode_SelectedIndexChanged">
                <asp:ListItem Value="DefaultValues" Text="<%$ PxString: PxWebAdminSettingsMenuViewLinkModeDefaultValues %>"></asp:ListItem>
                <asp:ListItem Value="AllValues" Text="<%$ PxString: PxWebAdminSettingsMenuViewLinkModeAllValues %>"></asp:ListItem>
                <asp:ListItem Value="Hidden" Text="<%$ PxString: PxWebAdminSettingsMenuViewLinkModeHidden %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ViewLinkMode" runat="server" onclick="ViewLinkModeInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1NumberOfValuesInDefaultView" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuNumberOfValuesInDefaultView %>"></asp:Label>
            <asp:TextBox ID="txtTree1NumberOfValuesInDefaultView" runat="server" CssClass="smallinput" MaxLength="2"></asp:TextBox>
            <asp:ImageButton ID="imgTree1NumberOfValuesInDefaultView" runat="server" onclick="NumberOfValuesInDefaultViewInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
             <asp:CustomValidator ID="validatorTree1NumberOfValuesInDefaultView" runat="server" 
            ControlToValidate="txtTree1NumberOfValuesInDefaultView" OnServerValidate="ValidateNumberOfValuesInDefaultView"
            ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1ShowModifiedDate" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowModifiedDate %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowModifiedDate" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowModifiedDate" runat="server" onclick="ShowModifiedDateInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1ShowLastUpdated" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowLastUpdated %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowLastUpdated" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowLastUpdated" runat="server" onclick="ShowLastUpdatedInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1ShowFileSize" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowFileSize %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowFileSize" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowFileSize" runat="server" onclick="ShowFileSizeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree1ShowVariablesAndValues" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowVariablesAndValues %>"></asp:Label>
            <asp:DropDownList ID="cboTree1ShowVariablesAndValues" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree1ShowVariablesAndValues" runat="server" onclick="ShowVariablesAndValuesInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
    </asp:Panel>
    
    
    <asp:Panel ID="pnlTreeViewWithFiles" runat="server">
         <div class="setting-field">
            <asp:Label ID="lblTree2ShowRoot" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowRoot %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ShowRoot" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ShowRoot" runat="server" onclick="ShowRootInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
         <div class="setting-field">
            <asp:Label ID="lblTree2ExpandAll" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuExpandAll %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ExpandAll" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ExpandAll" runat="server" onclick="ExpandAllInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
<!--        
         <div class="setting-field">
            <asp:Label ID="lblTree2SortByAlias" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuSortByAlias %>"></asp:Label>
            <asp:DropDownList ID="cboTree2SortByAlias" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2SortByAlias" runat="server" onclick="SortByAliasInfo"/>
        </div>
-->
        <div class="setting-field">
            <asp:Label ID="lblTree2ShowModifiedDate" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowModifiedDate %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ShowModifiedDate" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ShowModifiedDate" runat="server" onclick="ShowModifiedDateInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree2ShowLastUpdated" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowLastUpdated %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ShowLastUpdated" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ShowLastUpdated" runat="server" onclick="ShowLastUpdatedInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree2ShowFileSize" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowFileSize %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ShowFileSize" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ShowFileSize" runat="server" onclick="ShowFileSizeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree2ShowTableCategory" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowTableCategory %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ShowTableCategory" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ShowTableCategory" runat="server" onclick="ShowTableCategoryInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree2ShowTableUpdatedAfterPublish" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowTableUpdatedAfterPublish %>"></asp:Label>
            <asp:DropDownList ID="cboTree2ShowTableUpdatedAfterPublish" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2ShowTableUpdatedAfterPublish" runat="server" onclick="ShowTableUpdatedAfterPublishInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblTree2MetadataAsIcons" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuMetadataAsIcons %>"></asp:Label>
            <asp:DropDownList ID="cboTree2MetadataAsIcons" runat="server" AutoPostBack="true" onselectedindexchanged="cboTree2MetadataAsIcons_SelectedIndexChanged" >
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgTree2MetadataAsIcons" runat="server" onclick="MetadataAsIconsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <div class="setting-field">
            <asp:Label ID="lblShowMenuExplanation" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowMenuExplanation %>"></asp:Label>
            <asp:DropDownList ID="cboShowMenuExplanation" runat="server" AutoPostBack="true" >
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgShowMenuExplanation" runat="server" onclick="ShowMenuExplanationInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
        <asp:Panel id="pnlShowTextForMetadata"  runat="server" >
            <div class="setting-field">
                <asp:Label ID="lblShowTextToMetadata" runat="server" Text="<%$ PxString: PxWebAdminSettingsMenuShowTextToMetadata %>"></asp:Label>
                <asp:DropDownList ID="cboShowTextToMetadata" runat="server"  >
                    <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                    <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:ImageButton ID="imgTree2ShowTextToMetadata" runat="server" onclick="ShowTextToMetadataInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            </div>   
        </asp:Panel>
    </asp:Panel>
    

    
</asp:Content>
