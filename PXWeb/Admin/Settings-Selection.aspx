<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-Selection.aspx.cs" Inherits="PXWeb.Admin.Settings_Selection" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
<%--    <div class="setting-field">
        <asp:Label ID="lblCellLimitScreen" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionCellLimitScreen %>"></asp:Label>
        <asp:TextBox ID="txtCellLimitScreen" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgCellLimitScreen" runat="server" onclick="CellLimitScreenInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorCellLimitScreen" runat="server" 
        ControlToValidate="txtCellLimitScreen" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
--%>    
    <div class="setting-field">
        <asp:Label ID="lblShowMandatoryMark" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowMandatoryMark %>"></asp:Label>
        <asp:DropDownList ID="cboShowMandatoryMark" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowMandatoryMark" runat="server" onclick="ShowMandatoryMarkInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblAllowAggregations" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionAllowAggregations %>"></asp:Label>
        <asp:DropDownList ID="cboAllowAggregations" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgAllowAggregations" runat="server" onclick="AllowAggregationsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblShowHierarchies" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowHierarchies %>"></asp:Label>
        <asp:DropDownList ID="cboShowHierarchies" runat="server" AutoPostBack="true" 
            onselectedindexchanged="cboShowHierarchies_SelectedIndexChanged">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowHierarchies" runat="server" onclick="ShowHierarchiesInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field" id="divHierarchicalLevelsOpen" runat="server">
        <asp:Label ID="lblHierarchicalLevelsOpen" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionHierarchicalLevelsOpen %>"></asp:Label>
        <asp:TextBox ID="txtHierarchicalLevelsOpen" runat="server" CssClass="smallinput" MaxLength="2"></asp:TextBox>
        <asp:ImageButton ID="imgHierarchicalLevelsOpen" runat="server" onclick="HierarchicalLevelsOpenInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorHierarchicalLevelsOpen" runat="server" 
        ControlToValidate="txtHierarchicalLevelsOpen" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
    <div class="setting-field">
        <asp:Label ID="lblShowMarkingTips" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowMarkingTips %>"></asp:Label>
        <asp:DropDownList ID="cboShowMarkingTips" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowMarkingTips" runat="server" onclick="ShowMarkingTipsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
<%--
    <div class="setting-field" id="divMarkingTipsURL" runat="server">
        <asp:Label ID="lblMarkingTipsURL" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionMarkingTipsURL %>"></asp:Label>
        <asp:TextBox ID="txtMarkingTipsURL" runat="server" ></asp:TextBox>
        <asp:ImageButton ID="imgMarkingTipsURL" runat="server" onclick="MarkingTipsURLInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMarkingTipsURL" runat="server" 
        ControlToValidate="txtMarkingTipsURL" OnServerValidate="ValidateMarkingTipsURL"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
--%>
    <div class="setting-field">
        <asp:Label ID="lblSearchButtonMode" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionSearchButtonMode %>"></asp:Label>
        <asp:DropDownList ID="cboSearchButtonMode" runat="server" AutoPostBack="true" onselectedindexchanged="SearchButtonMode_SelectedIndexChanged">
            <asp:ListItem Value="ManyValues" Text="<%$ PxString: PxWebAdminSettingsSelectionSearchButtonModeManyValues %>"></asp:ListItem>
            <asp:ListItem Value="Always" Text="<%$ PxString: PxWebAdminSettingsSelectionSearchButtonModeAlways %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgSearchButtonMode" runat="server" onclick="SearchButtonModeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblMaxRowsWithoutSearch" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionMaxRowsWithoutSearch %>"></asp:Label>
        <asp:TextBox ID="txtMaxRowsWithoutSearch" runat="server" CssClass="smallinput" MaxLength="6"></asp:TextBox>
        <asp:ImageButton ID="imgMaxRowsWithoutSearch" runat="server" onclick="MaxRowsWithoutSearchInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMaxRowsWithoutSearch" runat="server" 
        ControlToValidate="txtMaxRowsWithoutSearch" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
     <div class="setting-field">
        <asp:Label ID="lblAlwaysShowTimeVariableWithoutSearch" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionAlwaysShowTimeVariableWithoutSearch %>"></asp:Label>
        <asp:DropDownList ID="cboAlwaysShowTimeVariableWithoutSearch" runat="server" >
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgAlwaysShowTimeVariableWithoutSearch" runat="server" onclick="AlwaysShowTimeVariableWithoutSearchInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
     </div>
    <div class="setting-field">
        <asp:Label ID="lblListSize" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionListSize %>"></asp:Label>
        <asp:TextBox ID="txtListSize" runat="server" CssClass="smallinput" MaxLength="2"></asp:TextBox>
        <asp:ImageButton ID="imgListSize" runat="server" onclick="ListSizeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorListSize" runat="server" 
        ControlToValidate="txtListSize" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
    <div class="setting-field">
        <asp:Label ID="lblShowSelectionLimits" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowSelectionLimits %>"></asp:Label>
        <asp:DropDownList ID="cboShowSelectionLimits" runat="server" >
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowSelectionLimits" runat="server" onclick="ShowSelectionLimitsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblValuesetMustBeSelectedFirst" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionValuesetMustBeSelectedFirst %>"></asp:Label>
        <asp:DropDownList ID="cboValuesetMustBeSelectedFirst" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgValuesetMustBeSelectedFirst" runat="server" onclick="ValuesetMustBeSelectedFirstInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblShowAllAvailableValuesSearchButton" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowAllAvailableValuesSearchButton %>"></asp:Label>
        <asp:DropDownList ID="cboShowAllAvailableValuesSearchButton" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowAllAvailableValuesSearchButton" runat="server" onclick="ShowAllAvailableValuesSearchButtonInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblTitleFromMenu" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionTitleFromMenu %>"></asp:Label>
        <asp:DropDownList ID="cboTitleFromMenu" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgTitleFromMenu" runat="server" onclick="TitleFromMenuInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field" style="padding-bottom:14px">
        <asp:Label ID="lblStandardApplicationHeadTitle" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionStandardApplicationHeadTitle %>"></asp:Label>
        <asp:DropDownList ID="cboStandardApplicationHeadTitle" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgStandardApplicationHeadTitle" runat="server" onclick="StandardApplicationHeadTitleInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblMetadataAsLinks" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionMetadataAsLinks %>"></asp:Label>
        <asp:DropDownList ID="cboMetadataAsLinks" runat="server">
            <asp:ListItem Value="True"  Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False"  Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgMetadataAsLinks" runat="server" onclick="MetadataAsLinksInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSelectValuesFromGroup" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowSelectValuesFromGroup %>"></asp:Label>
        <asp:DropDownList ID="cboSelectValuesFromGroup" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgSelectValuesFromGroup" runat="server" onclick="SelectValuesFromGroupInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblButtonsForContentVariable" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionButtonsForContentVariable %>"></asp:Label>
        <asp:DropDownList ID="cboButtonsForContentVariable" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgButtonsForContentVariable" runat="server" onclick="ButtonsForContentVariableInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDefaultSearch" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionDefaultSearch %>"></asp:Label>
        <asp:DropDownList ID="cboDefaultSearch" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminSettingsSelectionDefaultSearchBeginningOfWord %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminSettingsSelectionDefaultSearchFreeText %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDefaultSearch" runat="server" onclick="DefaultSearchInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblPreSelectFirstContentAndTime" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionPreSelectFirstContentAndTime %>"></asp:Label>
        <asp:DropDownList ID="cboPreSelectFirstContentAndTime" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgPreSelectFirstContentAndTime" runat="server" onclick="PreSelectFirstContentAndTimeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblShowNoFootnoteForSelection" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionShowNoFootnoteForSelection %>"></asp:Label>
        <asp:DropDownList ID="cboShowNoFootnoteForSelection" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowNoFootnoteForSelection" runat="server" onclick="ShowNoFootnoteForSelectionInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSortVariableOrder" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionSortVariableOrder %>"></asp:Label>
        <asp:DropDownList ID="cboSortVariableOrder" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgSortVariableOrder" runat="server" onclick="SortVariableOrderInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblAlwaysShowCodeAndTextInAdvancedSearchResult" runat="server" Text="<%$ PxString: PxWebAdminSettingsSelectionAlwaysShowCodeAndTextInAdvancedSearchResult %>"></asp:Label>
        <asp:DropDownList ID="cboAlwaysShowCodeAndTextInAdvancedSearchResult" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgAlwaysShowCodeAndTextInAdvancedSearchResult" runat="server" onclick="AlwaysShowCodeAndTextInAdvancedSearchResultInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
</asp:Content>
