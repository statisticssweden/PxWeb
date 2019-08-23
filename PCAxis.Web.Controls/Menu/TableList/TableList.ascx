<%@ control inherits="PCAxis.Web.Controls.TableListCodebehind" %>
<%@ Import Namespace="System.ComponentModel"%>
<asp:Panel runat="server" ID="MenulistPanel" EnableViewState="false">
    <asp:Repeater ID="LinkItemList" runat="server">
        <HeaderTemplate><ol class="tablelist_ol"></HeaderTemplate>
        <ItemTemplate>
            <li class="tablelist_li">
                <asp:Panel id="pnlPx" runat="server">
                <p>                               
                <asp:HyperLink runat="server" CssClass="tablelist_linkHeading" ID="lnkTableListItemText" Visible="false" />                
                </p>

                <asp:HyperLink runat="server" ID="lnkTableListLinkURLSelect" CssClass="tablelist_link" Visible="false" />
                    
                <asp:HyperLink runat="server" ID="lnkTableListShowDownloadLink" CssClass="tablelist_link" Visible="false" />                                                             
                 
                <asp:HyperLink runat="server" ID="lnkTableListSelectOption_View" CssClass="tablelist_link" Visible="false" />                
                
                <asp:HyperLink runat="server" ID="lnkTableListSelectOption_ViewDefaultValues" CssClass="tablelist_link" Visible="false" />
                
                <asp:HyperLink runat="server" ID="lnkTableListSelectOption_ViewWithCommandbar" CssClass="tablelist_link" Visible="false" />
                
                <asp:HyperLink runat="server" ID="lnkTableListSelectOption_ViewDefaultValuesWithCommandbar" CssClass="tablelist_link" Visible="false" />                

                <asp:Literal runat="server" ID="ltlTableListShowFileSizeHeading" Visible="false" />
                <asp:Literal runat="server" ID="ltlTableListShowFileSizeSize" Visible="false" />
                
                <asp:Literal runat="server" ID="ltlTableListShowModifiedDateHeading" Visible="false" />
                <asp:Literal runat="server" ID="ltlTableListShowModifiedDateModified" Visible="false" />
                
                <asp:Literal runat="server" ID="ltlTableListShowLastUpdatedHeading" Visible="false" />
                <asp:Literal runat="server" ID="ltlTableListShowLastUpdatedUpdated" Visible="false" />

                </p>
                
                <asp:Literal runat="server" ID="ltlTableListShowVariablesAndValues" Visible="false" />
             
                </asp:Panel>
                <asp:Panel id="pnlUrl" runat="server">
                   <a href="<%#Eval("LinkURLSelect")%>" class="tablelist_link" target="_new"><%#DataBinder.Eval(Container.DataItem, "Text")%></a>
                </asp:Panel>
            </li>
        </ItemTemplate>
        <SeparatorTemplate><hr /></SeparatorTemplate>
        <FooterTemplate></ol></FooterTemplate>
    </asp:Repeater>
    
</asp:Panel>