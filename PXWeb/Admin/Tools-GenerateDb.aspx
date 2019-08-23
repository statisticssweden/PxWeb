<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Tools-GenerateDb.aspx.cs" Inherits="PXWeb.Admin.Tools_GenerateDb" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblSelectDb" runat="server" Text="<%$ PxString: PxWebAdminToolsGenerateDbSelectDb %>">"></asp:Label>
        <asp:DropDownList ID="cboSelectDb" runat="server"></asp:DropDownList>
        <asp:ImageButton ID="imgSelectDbInfo" runat="server" onclick="imgSelectDb_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    
    <div class="setting-field">
        <asp:Label ID="lblLanguageDependent" runat="server" Text="<%$ PxString: PxWebAdminToolsGenerateDbLanguageDependent %>">"></asp:Label>
        <asp:DropDownList ID="cboLanguageDependent" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgLanguageDependentInfo" runat="server" onclick="imgLanguageDependent_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
        <asp:Label ID="lblSortOrder" runat="server" Text="<%$ PxString: PxWebAdminToolsGenerateDbSortOrder %>">"></asp:Label>
        <asp:DropDownList ID="cboSortOrder" runat="server">
            <asp:ListItem Value="FileName" Text="<%$ PxString: PxWebAdminSortOrderFileName %>"></asp:ListItem>
            <asp:ListItem Value="Matrix" Text="<%$ PxString: PxWebAdminSortOrderMatrix %>"></asp:ListItem>
            <asp:ListItem Value="Title" Text="<%$ PxString: PxWebAdminSortOrderTitle %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgSortOrderInfo" runat="server" onclick="imgSortOrder_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div id="divCreateIndex" runat="server">
<%--    <div class="setting-field">--%>
        <asp:CheckBox ID="chkCreateIndex" Text="<%$ PxString: PxWebAdminToolsGenerateDbCreateIndex %>" runat="server" />
        <asp:ImageButton ID="imgCreateIndexInfo" runat="server" onclick="CreateIndexInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>

    <div class="setting-field">
            <asp:Button ID="btnGenerate" runat="server" CssClass="toolbutton" onclick="btnGenerate_Click" Text="<%$ PxString: PxWebAdminGenerateButton %>" />
    </div>
    
    <div class="setting-field">
    </div>
   
    <div class="setting-field">
    </div>
    
<%--        <asp:Repeater ID="rptResults" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblType" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"MessageType") %>'></asp:Label>
                <asp:Label ID="lblMessage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message") %>'></asp:Label>
                <br />
            </ItemTemplate>
            <FooterTemplate/>
        </asp:Repeater>--%>
    <asp:GridView ID="grdResult" runat="server" AutoGenerateColumns="false">
    <Columns>
    <asp:BoundField DataField="MessageType"  />
    <asp:BoundField DataField="Message"  />
    </Columns>
    </asp:GridView>

    <div id="InfoBox" runat="server" class="infoBox" visible="false">
        <asp:Label ID="lblCreateIndexInfo" runat="server" Text="<%$ PxString: PxWebAdminToolsGenerateDbCreateIndexInitiated %>"></asp:Label>
    </div>
</asp:Content>
