<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Features-SaveQuery-General.aspx.cs" Inherits="PXWeb.Admin.Features_SaveQuery_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdminHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <p>        
        <asp:Label ID="lblSavedQuerySetting" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSaveQueryGeneralSaveQuerySettings %>" CssClass="setting_keyword"></asp:Label>
    </p>
    <div class="setting-field">
        <asp:Label ID="lblStorageType" runat="server" Text="<%$ PxString: PxWebAdminSettingsSavedQueryStorageType %>"></asp:Label>
        <asp:DropDownList ID="cboStorageType" runat="server">
            <asp:ListItem Value="File" Text="<%$ PxString: PxWebAdminSettingsSavedQueryStorageTypeFile %>"></asp:ListItem>
            <asp:ListItem Value="Database" Text="<%$ PxString: PxWebAdminSettingsSavedQueryStorageTypeDatabase %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgStorageType" runat="server" onclick="StorageTypeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
   <div class="setting-field">
        <asp:Label ID="lblEnableCache" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralEnableCache %>"></asp:Label>
        <asp:DropDownList ID="cboEnableCache" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgEnableCache" runat="server" onclick="EnableCacheInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
     
    <div class="setting-field">
        <asp:Label ID="lblCacheTime" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralCacheTime %>"></asp:Label>
        <asp:TextBox ID="txtCacheTime" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgCacheTime" runat="server" onclick="CacheTimeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorCacheTime" runat="server" 
        ControlToValidate="txtCacheTime" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

    <div class="setting-field">
        <asp:Label ID="lblEnableCORS" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralEnableCORS %>"></asp:Label>
        <asp:DropDownList ID="cboEnableCORS" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgEnableCORS" runat="server" onclick="EnableCORSInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblShowPeriodAndId" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralShowPeriodAndId %>"></asp:Label>
        <asp:DropDownList ID="cboShowPeriodAndId" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgShowPeriodAndId" runat="server" onclick="EnableShowPeriodAndIdInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblEnableLimitRequest" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralEnableLimitRequest %>"></asp:Label>
        <asp:DropDownList ID="cboEnableLimitRequest" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="imgEnableLimitRequest" runat="server" onclick="EnableLimitRequestInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblLimiterRequests" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralLimiterRequests %>"></asp:Label>
        <asp:TextBox ID="txtLimiterRequestsSq" runat="server" CssClass="smallinput" MaxLength="10" ></asp:TextBox>
         &nbsp;<asp:ImageButton ID="imgLimiterRequests" runat="server" onclick="LimiterRequestsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLimiterRequests" runat="server" 
        ControlToValidate="txtLimiterRequestsSq" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>

    <div class="setting-field">
        <asp:Label ID="lblLimiterTimespan" runat="server" Text="<%$ PxString: PxWebAdminFeaturesSavedQueryGeneralLimiterTimespan %>"></asp:Label>
        <asp:TextBox ID="txtLimiterTimespanSq" runat="server" CssClass="smallinput" MaxLength="10" ></asp:TextBox>
         &nbsp;<asp:ImageButton ID="imgLimiterTimespan" runat="server" onclick="LimiterTimespanInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLimiterTimespan" runat="server" 
        ControlToValidate="txtLimiterTimespanSq" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
</asp:Content>
