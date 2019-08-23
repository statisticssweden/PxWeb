<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-Databases.aspx.cs" Inherits="PXWeb.Admin.Settings_General_Databases" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <p>
        <asp:Label ID="lblPxDatabases" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesPxDatabases %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgPxDatabases" runat="server" onclick="imgPxDatabases_Click" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" /><br />
        <asp:Repeater ID="rptPxDatabases" runat="server" onitemdatabound="rptDatabases_ItemDataBound">
            <HeaderTemplate>
                <table cellspacing="0">
                    <thead>
                        <tr>
                            <td class="databaseHeaderCell databaseSelectedCell"><asp:Literal ID="litPxEnabled" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesEnabled %>"></asp:Literal></td>
                            <td class="databaseHeaderCell databaseCreatedCell"><asp:Literal ID="litPxDbCreated" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesDbCreated %>"></asp:Literal></td>
                            <td class="databaseHeaderCell databaseIndexStatusCell"><asp:Literal ID="litPxIndexStatus" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesIndexStatus %>"></asp:Literal></td>
                            <td class="databaseHeaderCell databaseIndexCell"><asp:Literal ID="litPxAction" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesAction %>"></asp:Literal></td>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="oddRow">
                    <td class="databaseCell databaseSelectedCell"><asp:CheckBox ID="chkDbItem" runat="server" Checked='<%# IsPxSelected(Container.DataItem) %>' Text='<%# DataBinder.Eval(Container.DataItem,"id") %>' Enabled='<%# IsPxDbGenerated(Container.DataItem) %>' /></td>
                    <td class="databaseCell databaseCreatedCell"><asp:Label ID="lblUpdated" runat="server" Text='<%# PxLastUpdated(Container.DataItem) %>'></asp:Label></td>
                    <td class="databaseCell databaseIndexStatusCell"><asp:Label ID="lblIndexStatus" runat="server" Text='<%# IndexStatus(Container.DataItem) %>' Visible='<%# IsPxDbGenerated(Container.DataItem) %>'></asp:Label></td>
                    <td class="databaseCell databaseIndexCell"><asp:LinkButton ID="lnkIndex" runat="server" CommandName="CreateIndex" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' OnClick="CreateNewIndex" Visible='<%# IsPxDbGenerated(Container.DataItem) %>' Enabled='<%# CheckIndexStatus(Container.DataItem) %>' Text="<%$ PxString: PxWebAdminSettingsDatabasesCreateIndex %>"></asp:LinkButton></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td class="databaseCell databaseSelectedCell"><asp:CheckBox ID="chkDbItem" runat="server" Checked='<%# IsPxSelected(Container.DataItem) %>' Text='<%# DataBinder.Eval(Container.DataItem,"id") %>' Enabled='<%# IsPxDbGenerated(Container.DataItem) %>' /></td>
                    <td class="databaseCell databaseCreatedCell"><asp:Label ID="lblUpdated" runat="server" Text='<%# PxLastUpdated(Container.DataItem) %>'></asp:Label></td>
                    <td class="databaseCell databaseIndexStatusCell"><asp:Label ID="lblIndexStatus" runat="server" Text='<%# IndexStatus(Container.DataItem) %>' Visible='<%# IsPxDbGenerated(Container.DataItem) %>'></asp:Label></td>
                    <td class="databaseCell databaseIndexCell"><asp:LinkButton ID="lnkIndex" runat="server" CommandName="CreateIndex" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' OnClick="CreateNewIndex" Visible='<%# IsPxDbGenerated(Container.DataItem) %>' Enabled='<%# CheckIndexStatus(Container.DataItem) %>' Text="<%$ PxString: PxWebAdminSettingsDatabasesCreateIndex %>"></asp:LinkButton></td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </p>
    <br />
    <p>
        <asp:Label ID="lblCnmmDatabases" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesCnmmDatabases %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgCnmmDatabases" runat="server" 
            onclick="imgCnmmDatabases_Click"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/> <br />
        <asp:Repeater ID="rptCnmmDatabases" runat="server" onitemdatabound="rptDatabases_ItemDataBound">
            <HeaderTemplate>
                <table cellspacing="0">
                    <thead>
                        <tr>
                            <td class="databaseHeaderCell databaseSelectedCell"><asp:Literal ID="litCnmmEnabled" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesEnabled %>"></asp:Literal></td>
                            <td class="databaseHeaderCell databaseCreatedCell">&nbsp;</td>
                            <td class="databaseHeaderCell databaseIndexStatusCell"><asp:Literal ID="litCnmmIndexStatus" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesIndexStatus %>"></asp:Literal></td>
                            <td class="databaseHeaderCell databaseIndexCell"><asp:Literal ID="litCnmmAction" runat="server" Text="<%$ PxString: PxWebAdminSettingsDatabasesAction %>"></asp:Literal></td>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="oddRow">
                    <td class="databaseCell databaseSelectedCell"><asp:CheckBox ID="chkDbItem" runat="server" Checked='<%# IsCnmmSelected(Container.DataItem) %>' Text='<%# DataBinder.Eval(Container.DataItem,"id") %>' /></td>
                    <td class="databaseCell databaseCreatedCell">&nbsp;</td>
                    <td class="databaseCell databaseIndexStatusCell"><asp:Label ID="lblIndexStatus" runat="server" Text='<%# IndexStatus(Container.DataItem) %>'></asp:Label></td>
                    <td class="databaseCell databaseIndexCell"><asp:LinkButton ID="lnkIndex" runat="server" CommandName="CreateIndex" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' OnClick="CreateNewIndex" Text="<%$ PxString: PxWebAdminSettingsDatabasesCreateIndex %>" Enabled='<%# CheckIndexStatus(Container.DataItem) %>'></asp:LinkButton></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td class="databaseCell databaseSelectedCell"><asp:CheckBox ID="chkDbItem" runat="server" Checked='<%# IsCnmmSelected(Container.DataItem) %>' Text='<%# DataBinder.Eval(Container.DataItem,"id") %>' /></td>
                    <td class="databaseCell databaseCreatedCell">&nbsp;</td>
                    <td class="databaseCell databaseIndexStatusCell"><asp:Label ID="lblIndexStatus" runat="server" Text='<%# IndexStatus(Container.DataItem) %>'></asp:Label></td>
                    <td class="databaseCell databaseIndexCell"><asp:LinkButton ID="lnkIndex" runat="server" CommandName="CreateIndex" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' OnClick="CreateNewIndex" Text="<%$ PxString: PxWebAdminSettingsDatabasesCreateIndex %>" Enabled='<%# CheckIndexStatus(Container.DataItem) %>'></asp:LinkButton></td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </p>
</asp:Content>
