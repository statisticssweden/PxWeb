<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Features-General.aspx.cs" Inherits="PXWeb.Admin.Features_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
        <asp:Label ID="lblFeature" runat="server" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeature %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgFeature" runat="server" onclick="imgFeature_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" /><br />
        <asp:CheckBox ID="chkCharts" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureCharts %>" runat="server" /><br />
        <asp:CheckBox ID="chkApi" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureApi %>" runat="server" /><br />
        <asp:CheckBox ID="chkSavedQuery" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureSavedQuery %>" runat="server" /><br />
        <asp:CheckBox ID="chkUserFriendlyUrls" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureUserFriendlyUrls %>" runat="server" /><br />
        <asp:CheckBox ID="chkUserStatistics" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureUserStatistics %>" runat="server" /><br />
        <asp:CheckBox ID="chkSearch" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureSearch %>" runat="server" /><br />
        <asp:CheckBox ID="chkBackgroundWorker" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureBackgroundWorker %>" runat="server" /><br />
        <asp:CheckBox ID="chkBulkLink" Text="<%$ PxString: PxWebAdminFeaturesGeneralFeatureBulkLink %>" runat="server" /><br />
        <br />
        <p>
            <asp:Label ID="Label1" runat="server" Text="Cache" CssClass="setting_keyword"></asp:Label>
        </p>
        <div class="setting-field">
            <asp:Label id="lblClearCache" runat="server" Text="<%$ PxString: PxWebAdminFeaturesGeneralClearCache %>"></asp:Label>
            <asp:TextBox ID="txtClearCache" runat="server" MaxLength="50"></asp:TextBox>
            <asp:ImageButton ID="imgClearCache" runat="server" onclick="ClearCacheInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            <br />
            <asp:CustomValidator ID="validatorClearCache" runat="server" 
            ControlToValidate="txtClearCache" OnServerValidate="ValidateClearCacheTimes"
            ErrorMessage="*" CssClass="setting-field-validator"  ></asp:CustomValidator>
        </div>           
</asp:Content>

