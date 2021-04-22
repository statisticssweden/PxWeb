<%@ control  inherits="PCAxis.Web.Controls.BreadcrumbCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<nav id="breadcrumb" aria-label="breadcrumb">
    <asp:HyperLink ID="lnkHome" runat="server">    
        <asp:Image ID="imgHome" runat="server" CssClass="breadcrumb_homebutton"/>
    </asp:HyperLink>
    <asp:Label ID="lblSep1" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkDb" runat="server" CssClass="pxweb-link breadcrumb_text"></asp:HyperLink>
    <asp:Label ID="lblSep2" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkPath1" runat="server" CssClass="pxweb-link breadcrumb_text" Visible="false"></asp:HyperLink>
    <asp:Label ID="lblSep3" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkPath2" runat="server" CssClass="pxweb-link breadcrumb_text" Visible="false"></asp:HyperLink>
    <asp:Label ID="lblSep4" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkPath3" runat="server" CssClass="pxweb-link breadcrumb_text" Visible="false"></asp:HyperLink>
    <asp:Label ID="lblSep5" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkPath4" runat="server" CssClass="pxweb-link breadcrumb_text" Visible="false"></asp:HyperLink>
    <asp:Label ID="lblSep6" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkPath5" runat="server" CssClass="pxweb-link breadcrumb_text" Visible="false"></asp:HyperLink>


    <asp:Label ID="lblSepBeforeTable" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:HyperLink ID="lnkTable" runat="server" CssClass="breadcrumb_text breadcrumb_tablelink" Visible="false"></asp:HyperLink>
    <asp:Label ID="lblSepBeforeSubPage" runat="server" Text="/" CssClass="breadcrumb_sep" Visible="false"></asp:Label>
    <asp:Label ID="lblSubPage" runat="server"  CssClass="breadcrumb_text_nolink breadcrumb_subpage" Visible="false"></asp:Label>
      
</nav>



