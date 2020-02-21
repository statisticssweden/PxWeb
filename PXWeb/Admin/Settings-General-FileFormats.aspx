<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-FileFormats.aspx.cs" Inherits="PXWeb.Admin.Settings_General_FileFormats" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblCellLimitDownload" runat="server" Text="<%$ PxString: PxWebAdminSettingsFileFormatsCellLimitDownload %>"></asp:Label>
        <asp:TextBox ID="txtCellLimitDownload" runat="server" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgCellLimitDownload" runat="server" onclick="CellLimitDownloadInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorCellLimitDownload" runat="server" 
        ControlToValidate="txtCellLimitDownload" OnServerValidate="ValidateCellLimitDownload"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lbFileBaseName" runat="server" Text="<%$ PxString: PxWebAdminSettingsFileBaseName %>"></asp:Label>
        <asp:DropDownList ID="cboFileBaseName" runat="server">            
            <asp:ListItem Value="matrix" Text="<%$ PxString: PxWebAdminSettingsFileBaseNameMatrix %>"></asp:ListItem>
            <asp:ListItem Value="tableid" Text="<%$ PxString: PxWebAdminSettingsFileBaseNameTableId %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgFileBaseName" runat="server" onclick="FileBaseNameInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <p>
        <asp:Label ID="lblExcel" runat="server" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcel %>" CssClass="setting_keyword"></asp:Label>
    </p>
    <div class="setting-field">
        <asp:Label ID="lblExcelInformationLevel" runat="server" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcelInformationLevel %>"></asp:Label>
        <asp:DropDownList ID="cboExcelInformationLevel" runat="server">
            <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcelInformationLevelNone %>"></asp:ListItem>
            <asp:ListItem Value="MandantoryFootnotesOnly" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcelInformationLevelMandantoryFootnotesOnly %>"></asp:ListItem>
            <asp:ListItem Value="AllFootnotes" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcelInformationLevelAllFootnotes %>"></asp:ListItem>
            <asp:ListItem Value="AllInformation" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcelInformationLevelAllInformation %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgExcelInformationLevel" runat="server" onclick="ExcelInformationLevelInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblExcelDoubleColumn" runat="server" Text="<%$ PxString: PxWebAdminSettingsFileFormatsExcelDoubleColumn %>"></asp:Label>
        <asp:DropDownList ID="cboExcelDoubleColumn" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgExcelDoubleColumn" runat="server" onclick="ExcelDoubleColumnInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
</asp:Content>
