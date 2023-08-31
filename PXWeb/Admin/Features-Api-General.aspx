<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Features-Api-General.aspx.cs" Inherits="PXWeb.Admin.Features_Api_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdminHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <p>        
        <asp:Label ID="lblApiSetting" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralApiSettings %>" CssClass="setting_keyword"></asp:Label>
    </p>

    <p>&nbsp;</p>
    <asp:Label ID="lblPxDatabases" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralApiPxDatabases %>" CssClass="setting_keyword"></asp:Label>
    <asp:ImageButton ID="imgPxDatabases" runat="server" onclick="PxDatabasesInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>

    <asp:Table ID="tblPxDatabases" runat="server">
    </asp:Table>
    <p>&nbsp;</p>

    <asp:Label ID="lblCnmmDatabases" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralApiCnmmDatabases %>" CssClass="setting_keyword"></asp:Label>
    <asp:ImageButton ID="imgCnmmDatabases" runat="server" onclick="CnmmDatabasesInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>

    <asp:Table ID="tblCnmmDatabases" runat="server">
    </asp:Table>
    <p>&nbsp;</p>

    <div class="setting-field">
        <asp:Label ID="lblRoutePrefix" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralRoutePrefix %>"></asp:Label>
        <asp:TextBox ID="txtRoutePrefix" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgRoutePrefix" runat="server" onclick="RoutePrefixInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorRoutePrefix" runat="server" 
        ControlToValidate="txtRoutePrefix" OnServerValidate="ValidateRoutePrefix"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

    <div class="setting-field">
        <asp:Label ID="lblUrlRoot" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralUrlRoot %>"></asp:Label>
        <asp:TextBox ID="txtUrlRoot" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgUrlRoot" runat="server" onclick="UrlRootInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorUrlRoot" runat="server" 
        ControlToValidate="txtUrlRoot" OnServerValidate="ValidateUrlRoot"
        ErrorMessage="*" ValidateEmptyText="False" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

    <div class="setting-field">
        <asp:Label ID="lblEnableApiV2QueryLink" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralEnableApiV2QueryLink %>"></asp:Label>
        <asp:DropDownList ID="cboEnableApiV2QueryLink" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgEnableApiV2QueryLink" runat="server" onclick="EnableApiV2QueryLinkInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblUrlRootV2" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralUrlRootV2 %>"></asp:Label>
        <asp:TextBox ID="txtUrlRootV2" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgUrlRootV2" runat="server" onclick="UrlRootInfoV2" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorUrlRootV2" runat="server" 
        ControlToValidate="txtUrlRootV2" OnServerValidate="ValidateUrlRoot"
        ErrorMessage="*" ValidateEmptyText="False" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

    <div class="setting-field">
        <asp:Label ID="lblMaxValuesReturned" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralMaxValuesReturned %>"></asp:Label>
        <asp:TextBox ID="txtMaxValuesReturned" runat="server" CssClass="smallinput" MaxLength="10" Width="50px"></asp:TextBox>
        <asp:ImageButton ID="imgMaxValuesReturned" runat="server" onclick="MaxValuesReturnedInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorMaxValuesReturned" runat="server" 
        ControlToValidate="txtMaxValuesReturned" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblFetchCellLimit" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralFetchCellLimit %>"></asp:Label>
        <asp:TextBox ID="txtFetchCellLimit" runat="server" CssClass="smallinput" MaxLength="15" Width="50px" ></asp:TextBox>
        <asp:ImageButton ID="imgFetchCellLimit" runat="server" onclick="FetchCellLimitInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorFetchCellLimit" runat="server" 
        ControlToValidate="txtFetchCellLimit" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>

    <div class="setting-field">
        <asp:Label ID="lblLimiterRequests" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralLimiterRequests %>"></asp:Label>
        <asp:TextBox ID="txtLimiterRequests" runat="server" CssClass="smallinput" MaxLength="10" Width="50px"></asp:TextBox>
        <asp:ImageButton ID="imgLimiterRequests" runat="server" onclick="LimiterRequestsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLimiterRequests" runat="server" 
        ControlToValidate="txtLimiterRequests" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>

    <div class="setting-field">
        <asp:Label ID="lblLimiterTimespan" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralLimiterTimespan %>"></asp:Label>
        <asp:TextBox ID="txtLimiterTimespan" runat="server" CssClass="smallinput" MaxLength="10" Width="50px"></asp:TextBox>
        <asp:ImageButton ID="imgLimiterTimespan" runat="server" onclick="LimiterTimespanInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLimiterTimespan" runat="server" 
        ControlToValidate="txtLimiterTimespan" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>

    <div class="setting-field">
        <asp:Label ID="lblEnableCORS" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralEnableCORS %>"></asp:Label>
        <asp:DropDownList ID="cboEnableCORS" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgEnableCORS" runat="server" onclick="EnableCORSInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblEnableCache" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralEnableCache %>"></asp:Label>
        <asp:DropDownList ID="cboEnableCache" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgEnableCache" runat="server" onclick="EnableCacheInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblShowQueryInformation" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralShowQueryInformation %>"></asp:Label>
        <asp:DropDownList ID="cboShowQueryInformation" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgShowQueryInformation" runat="server" onclick="ShowQueryInformation" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
     
    <div class="setting-field">
        <asp:Label ID="lblDefaultExampleResponseFormat" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralDefaultExampleResponseFormat %>"></asp:Label>
        <asp:DropDownList ID="cboDefaultExampleResponseFormat" runat="server">
            <asp:ListItem Value="px" Text="<%$ PxString: FileTypePX %>"></asp:ListItem>
            <asp:ListItem Value="csv" Text="<%$ PxString: FileTypeCsvWithHeadingAndComma %>"></asp:ListItem>
            <asp:ListItem Value="json" Text="<%$ PxString: FileTypeJson %>"></asp:ListItem>
            <asp:ListItem Value="json-stat" Text="<%$ PxString: FileTypeJsonStat %>"></asp:ListItem>
            <asp:ListItem Value="json-stat2" Text="<%$ PxString: FileTypeJsonStat2 %>"></asp:ListItem>
            <asp:ListItem Value="xlsx" Text="<%$ PxString: FileTypeExcelX %>"></asp:ListItem>
            <asp:ListItem Value="sdmx" Text="<%$ PxString: FileTypeSDMX %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgDefaultExampleResponseFormat" runat="server" onclick="DefaultExampleResponseFormatInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblShowSaveApiQueryButton" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralShowSaveApiQueryButton %>"></asp:Label>
        <asp:DropDownList ID="cboShowSaveApiQueryButton" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboShowSaveApiQueryButton_SelectedIndexChanged">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgShowSaveApiQueryButton" runat="server" onclick="ShowSaveApiQueryButtonInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <asp:Panel ID="pnlSaveApiQueryText" runat="server" Visible="false">
    <div class="setting-field">
        <asp:Label ID="lblSaveApiQueryText" runat="server" Text="<%$ PxString: PxWebAdminFeaturesApiGeneralSaveApiQueryText %>"></asp:Label>
        <asp:TextBox ID="txtSaveApiQueryText" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSaveApiQueryText" runat="server" onclick="SaveApiQueryTextInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="CustomValidator1" runat="server" 
        ControlToValidate="txtSaveApiQueryText" OnServerValidate="ValidateRoutePrefix"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    </asp:Panel>

</asp:Content>
