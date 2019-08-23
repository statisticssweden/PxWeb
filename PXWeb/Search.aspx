<%@ Page Title="<%$ PxString: PxWebTitleSearch %>" Language="C#" MasterPageFile="~/PxWeb.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="PXWeb.Search" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<%@ Register Src="~/UserControls/SearchControl.ascx" TagPrefix="uc1" TagName="SearchControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="searchTopLeftContent">
        <uc1:SearchControl runat="server" ID="pxSearch" />
    </div>
    <div class="break"></div>
        <asp:Panel ID="pnlSearch" runat="server" CssClass="search-settings">
            <asp:RadioButton ID="rbAll" Text="<%$ PxString: PxWebSearchAllFields %>" Value="all" Checked="true" runat="server" GroupName="searchOptions" />
            <asp:RadioButton ID="rbSelect" Text="<%$ PxString: PxWebSearchSelection %>" Value="select" runat="server" GroupName="searchOptions" />
            <asp:CheckBox ID="chkTitle" runat="server" Text="<%$ PxString: PxWebSearchFieldTitle %>" CssClass="search-option" />
            <asp:CheckBox ID="chkValues" runat="server" Text="<%$ PxString: PxWebSearchFieldValue %>" CssClass="search-option"/>
            <asp:CheckBox ID="chkCodes" runat="server" Text="<%$ PxString: PxWebSearchFieldCode %>" CssClass="search-option" />
        </asp:Panel>
    <asp:Label ID="lblSearchResult" CssClass="search_searchresulttext" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="repSearchResult"  runat="server">
        <HeaderTemplate>
            <table class="searchResultTable" cellspacing="0">
                <thead>
                    <tr>
                        <td class="searchHeaderCell searchCellTable"><%= Master.GetLocalizedString("PxWebSearchResultColumnTable") %></td>
                        <td class="searchHeaderCell searchCellPublished"><%= Master.GetLocalizedString("PxWebSearchResultColumnPublished") %></td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="searchCell searchCellTable"><a href="<%# ResolveUrl(CreateUrl(Container.DataItem)) %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></td>
                <td class="searchCell searchCellPublished"><%# GetPublished(Container.DataItem) %></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="tableAlternatingRow">
                <td class="searchCell searchCellTable"><a href="<%# ResolveUrl(CreateUrl(Container.DataItem)) %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></td>
                <td class="searchCell searchCellPublished"><%# GetPublished(Container.DataItem) %></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <script type="text/javascript" >

        jQuery(document).ready(function () {
            jQuery('[id$=rbAll]').click(function () {
                jQuery('.search-option').prop('disabled', true);
                jQuery('[id$=chkTitle]').prop('disabled', true);
                jQuery('[id$=chkValues]').prop('disabled', true);
                jQuery('[id$=chkCodes]').prop('disabled', true);
                return true;
            });
            jQuery('[id$=rbSelect]').click(function () {
                jQuery('.search-option').prop('disabled', false);
                jQuery('[id$=chkTitle]').prop('disabled', false);
                jQuery('[id$=chkValues]').prop('disabled', false);
                jQuery('[id$=chkCodes]').prop('disabled', false);
                return true;
            });
        });

    </script>    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>

