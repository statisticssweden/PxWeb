<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Features-BackgroundWorker-General.aspx.cs" Inherits="PXWeb.Admin.Features_BackgroundWorker_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdminHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <p>        
        <asp:Label ID="lblApiSetting" runat="server" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerGeneralSettings %>" CssClass="setting_keyword"></asp:Label>
    </p>
    <div class="setting-field">
        <asp:Label ID="lblSleepTime" runat="server" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerGeneralSleepTime %>"></asp:Label>
        <asp:TextBox ID="txtSleepTime" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSleepTime" runat="server" onclick="SleepTimeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSleepTime" runat="server" 
        ControlToValidate="txtSleepTime" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

    <asp:UpdatePanel ID="StatusUpdatePanel" runat="server">
        <ContentTemplate>
            <div class="setting-field">
                <asp:Label ID="lblStatus" runat="server" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerGeneralStatus %>"></asp:Label>
                <asp:Label ID="lblStatusValue" runat="server" Text="?" CssClass="status-value"></asp:Label>
                <asp:ImageButton ID="imgStatus" runat="server" onclick="StatusInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
                <asp:LinkButton ID="lnkStop" runat="server" OnClick="Stop" Visible="false" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerStop %>"></asp:LinkButton>
                <asp:LinkButton ID="lnkStart" runat="server" OnClick="Restart" Visible="false" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerRestart %>"></asp:LinkButton>
            </div>
            <div class="setting-field">
                <asp:Label ID="lblCurrentActivity" runat="server" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerGeneralCurrentActivity %>"></asp:Label>
                <asp:Label ID="lblCurrentActivityValue" runat="server" Text="?" CssClass="status-value"></asp:Label>
                <asp:ImageButton ID="imgCurrentActivity" runat="server" onclick="CurrentActivityInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
                <asp:LinkButton ID="lnkWakeUp" runat="server" OnClick="WakeUp" Visible="false" Text="<%$ PxString: PxWebAdminFeaturesBackgroundWorkerWakeUp %>"></asp:LinkButton>
            </div>    
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="timerStatus" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="timerStatus" runat="server" Interval="1000" OnTick="UpdateStatus"></asp:Timer>
</asp:Content>
