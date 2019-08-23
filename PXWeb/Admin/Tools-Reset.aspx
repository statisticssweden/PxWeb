<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-Reset.aspx.cs" Inherits="PXWeb.Admin.Tools_Reset" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
<div class="setting-field">
    <asp:LinkButton ID="btnResetAll" runat="server" 
                Text="<%$ PxString: PxWebAdminToolsResetAll %>" onclick="btnResetAll_Click" />
    <asp:ImageButton ID="imgResetAll" runat="server" onclick="imgResetAll_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
</div>
                
<div class="setting-field">
    <asp:LinkButton ID="btnResetLanguage" runat="server" 
                Text="<%$ PxString: PxWebAdminToolsResetLanguage %>" 
                onclick="btnResetLanguage_Click" />
    <asp:ImageButton ID="imgResetLanguage" runat="server" onclick="imgResetLanguage_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
</div>

<div class="setting-field">
    <asp:LinkButton ID="btnResetAggregation" runat="server" 
                Text="<%$ PxString: PxWebAdminToolsResetAggregation %>" 
                onclick="btnResetAggregation_Click" />
    <asp:ImageButton ID="imgResetAggregation" runat="server" onclick="imgResetAggregation_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
</div>

<div class="setting-field">
    <asp:LinkButton ID="btnResetDatabases" runat="server" 
                Text="<%$ PxString: PxWebAdminToolsResetDatabases %>" 
                onclick="btnResetDatabases_Click" />
    <asp:ImageButton ID="imgResetDatabases" runat="server" onclick="imgResetDatabases_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
</div>

<div class="setting-field">
    <asp:LinkButton ID="btnResetSavedQuery" runat="server" 
                Text="<%$ PxString: PxWebAdminToolsResetSavedQueryCache %>" 
                onclick="btnResetSavedQueryCache_Click" />
    <asp:ImageButton ID="ImageButton1" runat="server" onclick="imgResetSavedQueryCache_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
</div>
</asp:Content>
